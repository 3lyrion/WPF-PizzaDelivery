using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font;
using DTO = Interfaces.DTO;
using SV = Interfaces.Service;

namespace PizzaDelivery_Server
{
    struct StuckedOrder
    {
        public DTO.OrderStatus NextStatus { get; set; }
        public int Id { get; set; }
    }

    public class App
    {
        Timer updateTimer;
        Timer reportTimer;

        List<StuckedOrder> stuckedOrders;

        SV.IOrder orderService;
        SV.IReport reportService;

        public App(SV.IOrder theOrderService, SV.IReport theReportService)
        {
            orderService = theOrderService;
            reportService = theReportService;
            /*
            stuckedOrders = new List<StuckedOrder>();
            
            updateTimer = new Timer(2500);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (s, e) => dispatchOrders();
            updateTimer.Start();

            reportTimer = new Timer(30 * 60 * 1000); // Каждые 30 минут
            reportTimer.AutoReset = true;
            reportTimer.Elapsed += (s, e) => ReportDB();
            reportTimer.Start();*/

            reportDB();
        }

        public void Start()
        {
            while (true) ;
        }

        void dispatchOrders()
        {
            var allOrders = orderService.GetList();

            foreach (var order in allOrders)
            {
                if (order.Status == DTO.OrderStatus.Stucked)
                {
                    try
                    {
                        stuckedOrders.First(e => e.Id == order.Id);
                    }
                    catch
                    {
                        if (order.CookId == 0)
                        {
                            stuckedOrders.Add(new StuckedOrder { NextStatus = DTO.OrderStatus.Preparation, Id = order.Id });

                            Console.WriteLine($"Заказ #{order.Id} ожидает свободного повара");
                        }

                        else if (order.CourierId == 0)
                        {
                            stuckedOrders.Add(new StuckedOrder { NextStatus = DTO.OrderStatus.Delivery, Id = order.Id });

                            Console.WriteLine($"Заказ #{order.Id} ожидает свободного курьера");
                        }
                    }
                }
            }

            var removed = new List<int>();
            foreach (var so in stuckedOrders)
            {
                if (so.NextStatus == DTO.OrderStatus.Preparation)
                    orderService.PassToCook(so.Id);

                else
                    orderService.PassToCourier(so.Id);

                var order = orderService.GetList().Find(e => e.Id == so.Id);
                if (order.Status == so.NextStatus)
                {
                    removed.Add(so.Id);

                    if (so.NextStatus == DTO.OrderStatus.Preparation)
                        Console.WriteLine($"Заказ #{so.Id} передан повару #{order.CookId}");

                    else
                        Console.WriteLine($"Заказ #{so.Id} передан курьеру #{order.CourierId}");
                }
            }

            if (removed.Count > 0)
                stuckedOrders = stuckedOrders.Where(e => !removed.Contains(e.Id)).ToList();
        }

        void reportDB()
        {
            var dt = DateTime.Now;

            var month = dt.Month.ToString();
            if (month.Length == 1) month = '0' + month;

            var month = dt.Month.ToString();
            if (month.Length == 1) month = '0' + month;

            var hour = dt.

            var fmt =  $"{dt.Day}.{month}.{dt.Year} {dt.Hour}:{dt.Minute}";

            using var document = new Document
            (
                new PdfDocument
                (
                    new PdfWriter
                    (
                        $"{dt.Day}_{dt.Month}_{dt.Year}_{dt.Hour}_{dt.Minute}_report_clients_orders.pdf"
                    )
                ),
                PageSize.A4.Rotate()
            );

            var font = PdfFontFactory.CreateFont("C:/Windows/Fonts/Arial.ttf", PdfEncodings.IDENTITY_H);
            
            var text = new Text("Клиенты - заказы");
            text.SetFont(font);
            text.SetFontSize(48);
            text.SimulateBold();
            document.Add(new Paragraph(text));
            
            text = new Text($"cгенерировано {dt}");
            text.SetFont(font);
            text.SetFontSize(24);
            text.SimulateBold();
            document.Add(new Paragraph(text).SetMarginBottom(50));

            foreach (var co in reportService.GetClientsOrders())
            {
                var table = new Table(10);
                table.SetFont(font);
                table.SetWidth(UnitValue.CreatePercentValue(98));
                table.SetTextAlignment(TextAlignment.CENTER);
                table.SetMarginBottom(50);
                table.SetKeepTogether(true);

                table.AddCell("ID Клиента");
                table.AddCell("ID Заказа");
                table.AddCell("Дата и время");
                table.AddCell("Адрес");
                table.AddCell("Получатель");
                table.AddCell("Пицца");
                table.AddCell("Тесто");
                table.AddCell("Размер");
                table.AddCell("Количество");
                table.AddCell("Итог");

                table.AddCell(new Cell(co.OrderParts.Count, 1).Add(new Paragraph(co.ClientId.ToString())));
                table.AddCell(new Cell(co.OrderParts.Count, 1).Add(new Paragraph(co.OrderId.ToString())));
                table.AddCell(new Cell(co.OrderParts.Count, 1).Add(new Paragraph(co.DateTime)));
                table.AddCell(new Cell(co.OrderParts.Count, 1).Add(new Paragraph(co.Address)));
                table.AddCell(new Cell(co.OrderParts.Count, 1).Add(new Paragraph(co.RecipientName)));
                
                foreach (var op in co.OrderParts)
                {
                    table.AddCell(op.Pizza);
                    table.AddCell(op.Dough);
                    table.AddCell(op.Size);
                    table.AddCell(op.Quantity);
                    table.AddCell(op.Total);
                }

                table.AddCell(new Cell(1, 10).Add(new Paragraph($"Итоговая цена: {co.Total}")));
                
                document.Add(table);
            }
        }
    }
}

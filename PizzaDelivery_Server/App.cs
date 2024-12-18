using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
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

        List<StuckedOrder> stuckedOrders;

        SV.IOrder orderService;

        public App(SV.IOrder theOrderService)
        {
            orderService = theOrderService;

            stuckedOrders = new List<StuckedOrder>();

            updateTimer = new Timer(2500);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += (s, e) => dispatchOrders();
            updateTimer.Start();
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
                            stuckedOrders.Add(new StuckedOrder { NextStatus = DTO.OrderStatus.Preparation, Id = order.Id });

                        else if (order.CourierId == 0)
                            stuckedOrders.Add(new StuckedOrder { NextStatus = DTO.OrderStatus.Delivery, Id = order.Id });

                        Console.WriteLine($"Заказ #{order.Id}");
                    }
                }
            }

            var removed = new List<int>();
            foreach (var so in stuckedOrders)
            {
                if (so.NextStatus == DTO.OrderStatus.Preparation)
                    orderService.PassOrderToCook(so.Id);

                else
                    orderService.PassOrderToCourier(so.Id);

                if (orderService.GetList().Find(e => e.Id == so.Id).Status == so.NextStatus)
                    removed.Add(so.Id);
            }

            if (removed.Count > 0)
                stuckedOrders = stuckedOrders.Where(e => !removed.Contains(e.Id)).ToList();
        }
    }
}

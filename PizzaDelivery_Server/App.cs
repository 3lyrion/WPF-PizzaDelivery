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
        public DTO.OrderStatus Status;
        public int Id;
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

            updateTimer = new Timer(5000);
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
                Console.WriteLine(order.Id);

                if (order.Status == DTO.OrderStatus.Creation && order.CookId == 0
                    || order.Status == DTO.OrderStatus.Preparation && order.CourierId == 0)
                {
                    try
                    {
                        stuckedOrders.Find(e => e.Id == order.Id);
                        stuckedOrders.Add(new StuckedOrder { Status = order.Status, Id = order.Id });
                    }
                    catch { }
                }
            }

            var removed = new List<int>();
            for (int i = 0; i < stuckedOrders.Count; i++)
            {
                var so = stuckedOrders[i];

                if (so.Status == DTO.OrderStatus.Creation)
                {
                    orderService.PassOrderToCook(so.Id);

                    if (orderService.GetList().Find(e => e.Id == so.Id).Status == DTO.OrderStatus.Preparation)
                        removed.Add(so.Id);
                }

                else
                {
                    orderService.PassOrderToCourier(so.Id);

                    if (orderService.GetList().Find(e => e.Id == so.Id).Status == DTO.OrderStatus.Delivery)
                        removed.Add(so.Id);
                }
            }

            if (removed.Count > 0)
                stuckedOrders = stuckedOrders.Where(e => !removed.Contains(e.Id)).ToList();
        }
    }
}

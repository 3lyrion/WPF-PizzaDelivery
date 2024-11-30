using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Interfaces.DTO;

namespace PizzaDelivery_EM.Util
{
    public static class Misc
    {
        public static DTO.OrderStatus StringToOrderStatus(string str)
        {
            switch (str)
            {
                case "Отменён": return DTO.OrderStatus.Cancellation;

                case "Доставляется": return DTO.OrderStatus.Delivery;

                case "Готовится": return DTO.OrderStatus.Preparation;

                case "Доставлен": return DTO.OrderStatus.Success;

                default:
                    throw new ArgumentException();
            }
        }
    }
}

using System;

namespace TestASPNet.Models.DB
{
    /// <summary>
    /// Model from DataBase from task
    /// </summary>
    public class OrderModelDb
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public float? TotalMoney { get; set; }
        public DateTime? Ordered { get; set; } // I just added, why not?:)

        public OrderModelDb(int orderId, int customerId, float totalMoney, DateTime ordered)
        {
            OrderId = orderId;
            CustomerId = customerId;
            TotalMoney = totalMoney;
            Ordered = ordered;
        }

    }
}
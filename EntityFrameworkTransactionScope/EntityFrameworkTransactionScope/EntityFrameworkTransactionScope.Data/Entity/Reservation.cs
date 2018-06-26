using System;
using System.Collections.Generic;

namespace EntityFrameworkTransactionScope.Data.Entity
{
    /// <summary>
    /// 预订
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// 预订Id
        /// </summary>
        public int BookingId { get; set; }

        /// <summary>
        /// 预订人
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预订日期
        /// </summary>
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}

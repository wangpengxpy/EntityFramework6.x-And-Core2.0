using System;
using System.Collections.Generic;

namespace EntityFrameworkTransactionScope.Data.Entity
{
    /// <summary>
    /// 航班
    /// </summary>
    public class FlightBooking
    {
        /// <summary>
        /// 航班Id
        /// </summary>
        public int FlightId { get; set; }

        /// <summary>
        /// 航班名称
        /// </summary>
        public string FilghtName { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 出行日期
        /// </summary>
        public DateTime TravellingDate { get; set; }
    }
}

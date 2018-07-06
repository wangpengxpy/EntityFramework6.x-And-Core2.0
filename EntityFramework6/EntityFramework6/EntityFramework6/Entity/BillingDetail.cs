namespace EntityFramework6.Entity
{
    /// <summary>
    /// 账户明细
    /// </summary>
    public abstract class BillingDetail
    {
        /// <summary>
        /// 账户明细Id
        /// </summary>
        public int BillingDetailId { get; set; }

        /// <summary>
        /// 账户所属者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Number { get; set; }
    }


    /// <summary>
    /// 账号
    /// </summary>
    public class BankAccount : BillingDetail
    {
        /// <summary>
        /// 账户名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 所属金融银行
        /// </summary>
        public string Swift { get; set; }
    }

    /// <summary>
    /// 信用卡
    /// </summary>
    public class CreditCard : BillingDetail
    {
        /// <summary>
        /// 信用卡类型
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        /// 信用卡失效月份
        /// </summary>
        public string ExpiryMonth { get; set; }

        /// <summary>
        /// 信用卡失效年份
        /// </summary>
        public string ExpiryYear { get; set; }
    }
}

namespace MeDirect.Exchange.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; set; }

        public int TargetCurrencyId { get; set; }
        public virtual Currency TargetCurrency { get; set; }

        public int? ExchangeRateId { get; set; }
        public ExchangeRate ExchangeRate { get; set; }
    }
}

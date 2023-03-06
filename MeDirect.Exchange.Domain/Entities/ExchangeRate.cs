namespace MeDirect.Exchange.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {

        public int BaseCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public decimal Rate { get; set; }
    }
}

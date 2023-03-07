namespace MeDirect.Exchange.Domain.Contracts
{
    public class TransactionDto
    {
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string UserName { get; set; }


        public string BaseCurrency { get; set; }


        public string TargetCurrency { get; set; }




    }
}

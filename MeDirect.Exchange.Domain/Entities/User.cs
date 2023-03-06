namespace MeDirect.Exchange.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int TradesPerHourLimit { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

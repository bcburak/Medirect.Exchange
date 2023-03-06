﻿namespace MeDirect.Exchange.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<ExchangeRate> SourceRates { get; set; }
        public ICollection<ExchangeRate> TargetRates { get; set; }
    }
}

using System;

namespace LiftPassPricing.Application.Ports
{
    public class LiftPassCommand
    {
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public int? Age { get; set; }
    }
}

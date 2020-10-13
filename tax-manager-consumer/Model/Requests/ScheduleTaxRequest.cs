using System;
using System.ComponentModel.DataAnnotations;

namespace tax_manager_consumer
{
    public class ScheduleTaxRequest
    {
        public char? Type { get; set; }
        public float? Value { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }

        public ScheduleTaxRequest(char Type, float Value, DateTime InitialDate, DateTime FinalDate)
        {
            this.Type = Type;
            this.Value = Value;
            this.InitialDate = InitialDate;
            this.FinalDate = FinalDate;
        }

        public ScheduleTaxRequest()
        {
        }
    }
}

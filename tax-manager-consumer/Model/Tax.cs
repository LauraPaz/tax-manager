using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager_consumer
{
    public class Tax
    {
        public long Id { get; set; }
        public float Value { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public Tax(float Value, DateTime InitialDate, DateTime FinalDate) 
        {
            this.Value = Value;
            this.InitialDate = InitialDate;
            this.FinalDate = FinalDate;
        }
        public Tax() {}

        public bool Equals(Tax otherTax)
        {
            return this.Value == otherTax.Value && this.InitialDate.Equals(otherTax.InitialDate) && this.FinalDate.Equals(otherTax.FinalDate);
        }
    }
}

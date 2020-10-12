using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager
{
    public class Tax
    {
        public long Id { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public DateTime InitialDate { get; set; }
        [Required]
        public DateTime FinalDate { get; set; }

        public Tax(float Value, DateTime InitialDate, DateTime FinalDate) 
        {
            this.Value = Value;
            this.InitialDate = InitialDate;
            this.FinalDate = FinalDate;
        }
        public Tax() {}
    }
}

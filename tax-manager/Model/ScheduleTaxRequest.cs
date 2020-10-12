using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager
{
    public class ScheduleTaxRequest
    {
        [Required]
        public char Type { get; set; }
        [Required]
        public float Value { get; set; }
        [Required]
        public DateTime InitialDate { get; set; }
        [Required]
        public DateTime FinalDate { get; set; }

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

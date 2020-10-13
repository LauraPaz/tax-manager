using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager
{
    public class ScheduleTaxRequest
    {
        [Required(ErrorMessage = "For a tax to be scheduled, its type must be provided")]
        public char? Type { get; set; }
        [Required(ErrorMessage = "For a tax to be scheduled, its value must be provided")]
        public float? Value { get; set; }
        [Required(ErrorMessage = "For a tax to be scheduled, its initial date must be provided")]
        public DateTime? InitialDate { get; set; }
        [Required(ErrorMessage = "For a tax to be scheduled, its final date must be provided")]
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

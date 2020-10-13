using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tax_manager
{
    public class UpdateMunicipalityRequest
    {
        public string Name { get; set; }
        public List<Tax> YearlyTaxes { get; set; }
        public List<Tax> MonthlyTaxes { get; set; }
        public List<Tax> WeeklyTaxes { get; set; }
        public List<Tax> DailyTaxes { get; set; }

        public UpdateMunicipalityRequest()
        {
        }
        public UpdateMunicipalityRequest(string Name, List<Tax> YearlyTaxes, List<Tax> MonthlyTaxes, List<Tax> WeeklyTaxes, List<Tax> DailyTaxes)
        {
            this.Name = Name;
            this.YearlyTaxes = YearlyTaxes;
            this.MonthlyTaxes = MonthlyTaxes;
            this.WeeklyTaxes = WeeklyTaxes;
            this.DailyTaxes = DailyTaxes;
        }
    }
}

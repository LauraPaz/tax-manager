﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tax_manager
{
    public class Municipality
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Tax> YearlyTaxes { get; set; }
        public List<Tax> MonthlyTaxes { get; set; }
        public List<Tax> WeeklyTaxes { get; set; }
        public List<Tax> DailyTaxes { get; set; }

        public Municipality()
        {
            this.YearlyTaxes = new List<Tax>();
            this.MonthlyTaxes = new List<Tax>();
            this.WeeklyTaxes = new List<Tax>();
            this.DailyTaxes = new List<Tax>();
        }
        public Municipality(string Name, List<Tax> YearlyTaxes, List<Tax> MonthlyTaxes, List<Tax> WeeklyTaxes, List<Tax> DailyTaxes)
        {
            this.Name = Name;
            this.YearlyTaxes = YearlyTaxes;
            this.MonthlyTaxes = MonthlyTaxes;
            this.WeeklyTaxes = WeeklyTaxes;
            this.DailyTaxes = DailyTaxes;
        }
    }
}

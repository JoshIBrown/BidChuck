using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class BaseBidEditItem
    {
        [Required]
        public int ScopeId { get; set; }
        public string ScopeDescription { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }

    public class ComputedBidEditItem
    {
        [Required]
        public int ScopeId { get; set; }
        [Required]
        public decimal RiskFactor { get; set; }
    }
}
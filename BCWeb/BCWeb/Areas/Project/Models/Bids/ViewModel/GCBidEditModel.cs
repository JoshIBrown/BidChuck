﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BCWeb.Areas.Project.Models.Bids.ViewModel
{
    public class GCBidEditModel
    {
        [Required]
        [RegularExpression(@"Save|Submit")]
        public string btn { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int BidPackageId { get; set; }
        public string ProjectName { get; set; }

        public IEnumerable<BaseBidEditItem> BaseBids { get; set; }
    }
}
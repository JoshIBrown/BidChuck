﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.Projects
{
    public class Bid
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BidPackageId { get; set; }
        [ForeignKey("BidPackageId")]
        [IgnoreDataMember]
        public virtual BidPackage BidPackage { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [IgnoreDataMember]
        public virtual UserProfile User { get; set; }

        // more meta data
    }
}
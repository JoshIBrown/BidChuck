﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel
{
    public class Scope
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Description { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [IgnoreDataMember]
        public virtual Scope Parent { get; set; }

        [IgnoreDataMember]
        public ICollection<Scope> Children { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<UserProfile> Users { get; set; }
    }
}
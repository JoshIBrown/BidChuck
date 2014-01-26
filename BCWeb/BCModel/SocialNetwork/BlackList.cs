using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BCModel.SocialNetwork
{
    public class BlackList
    {
        [Key, Column(Order = 0)]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), IgnoreDataMember]
        public virtual CompanyProfile Company { get; set; }

        [Key, Column(Order = 1)]
        public int BlackListedCompanyId { get; set; }
        [ForeignKey("BlackListedCompanyId"), IgnoreDataMember]
        public virtual CompanyProfile BlackListedCompany { get; set; }

        [Required]
        public DateTime BlackListDate { get; set; }

        public string Notes { get; set; }
    }
}

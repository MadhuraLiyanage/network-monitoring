using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOSMonitoring.Models
{
    public class RemorteHostName
    {
        public Int64 RecNo { get; set; }
        [Key]
        public string RemortTPOSHostName { get; set; }
        public string RemortHostType { get; set; }
        public string RemortHostDescription { get; set; }
        public string Parent { get; set; }
        public bool Active { get; set; }
    }
}

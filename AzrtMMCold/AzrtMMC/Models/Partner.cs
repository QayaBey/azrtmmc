using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzrtMMC.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public string ImagePath { get; set; }
        public string Link { get; set; }
    }
}
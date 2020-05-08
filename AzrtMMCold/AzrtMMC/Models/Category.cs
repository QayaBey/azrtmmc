using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string ImagePath { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int ShopId { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

    }
}
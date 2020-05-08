using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Models
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string ImagePath { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] OtherImages { get; set; }

        [AllowHtml]
        public string Description { get; set; }
        [StringLength(200)]
        public string VideoLink { get; set; }
        [StringLength(200)] 
        public string PdfLink { get; set; }

        [AllowHtml]
        public string Property { get; set; }
        [AllowHtml]
        public string KullanimAlanlari { get; set; }
        [AllowHtml]
        public string Sektorler { get; set; }
        public int SubCategoryId { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
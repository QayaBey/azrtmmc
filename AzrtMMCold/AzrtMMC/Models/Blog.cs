using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AzrtMMC.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Title { get; set; }
        [StringLength(200)]
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
    }
}
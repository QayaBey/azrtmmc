using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzrtMMC.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public Product Product { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzrtMMC.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [StringLength(40)]
        public string NameSurname { get; set; }
        [StringLength(40),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(40)]
        public string Subject { get; set; }
        [StringLength(40)]
        public string Company { get; set; }
        public string Message { get; set; }
    }
}
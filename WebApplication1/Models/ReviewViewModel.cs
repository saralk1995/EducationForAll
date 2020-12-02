using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ReviewViewModel
    {
        [Key]
        public int id { get; set; }
        public Review review { get; set; }
        public List<String> courseList { get; set; }
        public string selectedCourse { get; set; }
    }
}
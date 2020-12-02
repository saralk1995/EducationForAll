using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class studentDetailPdf
    {
        [Key]
        public int id { get; set; }
        public String studentName { get; set; }
        public String studentEmail { get; set; }
        public String studentQualifiction { get; set; }
        public String studentPhoneNumber { get; set; }
        public String studentLocation { get; set; }
        public DateTime studentDob { get; set; }
        public String studentUser_id { get; set; }
    }
}
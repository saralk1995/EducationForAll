using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class aggregateRatings
    {
        public int id { get; set; }
        public List<String> courseList { get; set; }
        public string selectedCourse { get; set; }
        public int highestRating { get; set; }
        public int lowestRating { get; set; }
        public double averagerating { get; set; }
    }
}
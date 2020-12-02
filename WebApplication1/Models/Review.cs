namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public int ReviewId { get; set; }

        [Required]
        public string ReviewComment { get; set; }

        public int ReviewStar { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReviewDate { get; set; }

        public int CourseId { get; set; }

        public int StudentId { get; set; }

        public virtual Cours Cours { get; set; }

        public virtual Student Student { get; set; }
    }
}

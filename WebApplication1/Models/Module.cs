namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Module")]
    public partial class Module
    {
        public int ModuleId { get; set; }

        [Required]
        public string ModuleContent { get; set; }

        public int CourseId { get; set; }

        public virtual Cours Cours { get; set; }
    }
}

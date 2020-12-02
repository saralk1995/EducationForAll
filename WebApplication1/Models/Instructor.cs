namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Instructor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instructor()
        {
            Courses = new HashSet<Cours>();
        }

        public int InstructorId { get; set; }

        [Required]
        public string InstructorName { get; set; }

        [Required]
        public string InstructorEmail { get; set; }

        [Column(TypeName = "date")]
        public DateTime InstructorDob { get; set; }

        [Required]
        public string InstructorQualification { get; set; }

        [Required]
        public string InstructorLocation { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        [Required]
        public string InstructorSuburb { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cours> Courses { get; set; }
    }
}

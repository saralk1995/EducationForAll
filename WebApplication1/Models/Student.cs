namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Enrolments = new HashSet<Enrolment>();
            Reviews = new HashSet<Review>();
        }

        public int StudentId { get; set; }

        [Required]
        public string StudentName { get; set; }

        [Required]
        public string StudentEmail { get; set; }

        [Required]
        public string StudentQualification { get; set; }

        [Required]
        public string StudentPhoneNo { get; set; }

        [Required]
        public string StudentCity { get; set; }

        [Column(TypeName = "date")]
        public DateTime StudentDob { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        [Required]
        public string StudentSuburb { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrolment> Enrolments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}

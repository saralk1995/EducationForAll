namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Suburb : DbContext
    {
        public Suburb()
            : base("name=Suburb")
        {
        }

        public virtual DbSet<postcodes_location> postcodes_location { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<postcodes_location>()
                .Property(e => e.latitude)
                .HasPrecision(6, 3);

            modelBuilder.Entity<postcodes_location>()
                .Property(e => e.longitude)
                .HasPrecision(6, 3);
        }
    }
}

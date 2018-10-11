using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using UNamur.Models;


namespace UNamur.DAL
{

    public class SchoolContext : DbContext
    {
        // Your context has been configured to use a 'SchoolContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'UNamur.DAL.SchoolContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SchoolContext' 
        // connection string in the application configuration file.
        public SchoolContext() : base("SchoolContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Prevents table names from being pluralized

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }


    }
}
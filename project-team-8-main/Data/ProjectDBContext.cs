

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Project_Authentication.Model;
using System.Collections.Generic;

namespace Project_Authentication.Data
{
    public class ProjectDBContext : DbContext
    {

        public ProjectDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Profile> Profiles { get; set; }    


        ////public DbSet<Award> Awards { get; set; }

        
        ////protected override void OnModelCreating(ModelBuilder modelBuilder)
        ///{
        /// modelBuilder.Entity<Award>()
        //// .HasOne(p => p.Project).WithOne(a => a.Award).HasForeignKey<Project>(w => w.ProjectID);
        ///}
    }
}

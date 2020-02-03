using Blog.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Blog.Models.DataContext
{
    public class BlogDBContext:DbContext
    {
        public BlogDBContext() : base("BlogDB")
        {

        }


        public DbSet<About> About { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasMany(b => b.Blogss)
                .WithMany(b => b.Tags)
                .Map(m => m.ToTable("BlogTag").MapLeftKey("EtiketId").MapRightKey("BlogId"));
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Blogs")]
    public class Blogs
    {
        public Blogs()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }

        [Key]
        public int BlogId { get; set; }

        [Required]
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string FotoURL { get; set; }

        public DateTime Tarih { get; set; }

        public int? KategoriId { get; set; }

        public int? UserId { get; set; }

        public int Oxunma { get; set; }


        public Category Category { get; set; }

        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }   

        public ICollection<Tag> Tags { get; set; }   

    }
}
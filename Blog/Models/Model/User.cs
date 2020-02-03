using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Blogss = new HashSet<Blogs>();
            Comments = new HashSet<Comment>();
        }

        [Key]
        public int UserId { get; set; }

        [Required,StringLength(50,ErrorMessage ="En çox 50 simvoldan ibarət ola bilər!")]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required,StringLength(50,ErrorMessage = "En çox 50 simvoldan ibarət ola bilər!")]
        public string Sifre { get; set; }

        [Required]
        public string AdSoyad { get; set; }

        public string FotoURL { get; set; }

        public int? YetkiId { get; set; }


        public Role Role { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Blogs> Blogss { get; set; }      //?


    }

}
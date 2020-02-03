using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int YorumId { get; set; }

        [DisplayName("Adınız Soyadınız")]
        public string AdSoyad { get; set; }

        [DisplayName("Şərhiniz")]
        public string Icerik { get; set; }

        public DateTime? Tarih { get; set; }

        public string Email { get; set; }

        public bool Onay { get; set; }

        public int? UserId { get; set; }

        public int? BlogId { get; set; }


        public User User { get; set; }

        public Blogs Blogs { get; set; }

    }
}
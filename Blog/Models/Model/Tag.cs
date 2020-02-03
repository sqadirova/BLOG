using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Tag")]
    public class Tag
    {
        public Tag()
        {
            Blogss = new HashSet<Blogs>();
        }

        [Key]
        public int EtiketId { get; set; }

        public string EtiketAdi { get; set; }

        public ICollection<Blogs> Blogss { get; set; }




    }
}
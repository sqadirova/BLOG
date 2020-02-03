using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            Blogss = new HashSet<Blogs>();
        }

        [Key]
        public int KategoriId { get; set; }

        [Required, StringLength(50, ErrorMessage = "50 simvoldan ibaret olmalidir")]
        public string KategoriAd { get; set; }

        public ICollection<Blogs> Blogss { get; set; }





    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }


        [Key]
        public int YetkiId { get; set; }

        [Required]
        public string Yetki { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
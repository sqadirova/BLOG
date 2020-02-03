using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("About")]
    public class About
    {
        [Key]
        public int HaqqimdaId { get; set; }

        public string Aciqlama { get; set; }

        
        public string FotoURL { get; set; }


    }
}
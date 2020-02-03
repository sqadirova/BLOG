using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models.Model
{
    [Table("Slider")]
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }

        [DisplayName("Slider Başliq"), StringLength(30, ErrorMessage = "30 simvoldan ibarət olmalıdır")]
        public string Basliq { get; set; }

        [DisplayName("Slider Rəsim")]
        public string FotoURL { get; set; }

    }
}
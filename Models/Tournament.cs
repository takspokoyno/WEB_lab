using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labka1.Models
{
    public partial class Tournament
    {
        public Tournament()
        {
            Participations = new HashSet<Participation>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Винагорода")]
        public decimal? Reward { get; set; }
        [Display(Name = "Широта")]
        [DisplayFormat(DataFormatString = @"{0:F2}", HtmlEncode = false, ApplyFormatInEditMode = true)]
        public double Lat { get; set; }
        [Display(Name = "Довгота")]
        [DisplayFormat(DataFormatString = @"{0:F2}", HtmlEncode = false, ApplyFormatInEditMode = true)]
        public double Lng { get; set; }

        public virtual ICollection<Participation> Participations { get; set; }
    }
}

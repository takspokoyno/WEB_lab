using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labka1.Models
{
    public partial class Sponsor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;
        [Display(Name = "Команда")]
        public int TeamId { get; set; }
        [Display(Name = "Команда")]
        public virtual Team? Team { get; set; } = null!;
    }
}

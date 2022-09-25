using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labka1.Models
{
    public partial class Part
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва")]
        public string? Name { get; set; } = null!;
        public int CarId { get; set; }
        [Display(Name = "Автомобіль")]
        public virtual Car? Car { get; set; } = null!;
    }
}

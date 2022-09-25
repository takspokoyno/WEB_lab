using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Labka1.Models
{
    public partial class Participation
    {
        public int Id { get; set; }
        [Display(Name = "Гонщик")]
        public int? RacerId { get; set; }
        [Display(Name = "Турнір")]
        public int? TournamentId { get; set; }
        [Display(Name = "Гонщик")]
        public virtual Racer? Racer { get; set; } = null!;
        [Display(Name = "Турнір")]
        public virtual Tournament? Tournament { get; set; } = null!;
    }
}

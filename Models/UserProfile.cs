using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexBackOffice.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [Required]
        public decimal Money { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        // Navegação para o utilizador correspondente (1:1)
        public virtual User User { get; set; }

        // Navegação para o utilizador que criou este perfil
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        // Navegação para o utilizador que atualizou este perfil
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
    }
}

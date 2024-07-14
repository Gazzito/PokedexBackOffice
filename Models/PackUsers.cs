using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexBackOffice.Models
{
    public class PackUsers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PackId { get; set; }

        [Required]
        public DateTime OpenedOn { get; set; }

        // Navegação para o utilizador associado
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Navegação para o pack associado
        [ForeignKey("PackId")]
        public virtual Pack Pack { get; set; }
    }
}

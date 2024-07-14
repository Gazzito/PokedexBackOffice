using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexBackOffice.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        // Navegação para o utilizador associado
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Navegação para o role associado
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokedexBackOffice.Models
{
    public class UserPokemons
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PokemonId { get; set; }

        [Required]
        public int ActualAttackPoints { get; set; }

        [Required]
        public int ActualHealthPoints { get; set; }

        [Required]
        public int ActualDefensePoints { get; set; }

        [Required]
        public int ActualSpeedPoints { get; set; }

        [Required]
        public int TotalCombatPoints { get; set; }

        [Required]
        public string Rarity { get; set; }

        [Required]
        public int PackId { get; set; }

        [Required]
        public bool IsFavourite { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        // Navegação para o utilizador associado
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        // Navegação para o Pokémon associado
        [ForeignKey("PokemonId")]
        public virtual Pokemon Pokemon { get; set; }

        // Navegação para o pack associado
        [ForeignKey("PackId")]
        public virtual Pack Pack { get; set; }

        // Navegação para o utilizador que criou este registro
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        // Navegação para o usuário que atualizou este registro
        [ForeignKey("UpdatedBy")]
        public virtual User UpdatedByUser { get; set; }
    }

    public class UserPokemonDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PokemonId { get; set; }
        public string PokemonName { get; set; } = string.Empty;
        public int ActualAttackPoints { get; set; }
        public int ActualHealthPoints { get; set; }
        public int ActualDefensePoints { get; set; }
        public int ActualSpeedPoints { get; set; }
        public int TotalCombatPoints { get; set; }
        public string Rarity { get; set; } = string.Empty;
        public int PackId { get; set; }
        public bool IsFavourite { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }

}

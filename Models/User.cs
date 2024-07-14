using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokedexBackOffice.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }
 
        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        public DateTime? NextOpenExpected { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? LastLogin { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual TotalDiamondPokemonsRanking TotalDiamondPokemonsRanking { get; set; }

        public virtual TotalPacksOpenedRanking TotalPacksOpenedRanking { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<PackUsers> PackUsers { get; set; } 

        public virtual ICollection<UserPokemons> UserPokemons { get; set; }

         public virtual ICollection<UserPokemons> UserPokemonsCreatedBy { get; set; } 
         public virtual ICollection<UserPokemons> UserPokemonsUpdatedBy { get; set; }
    }


     public class UserDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        public DateTime? NextOpenExpected { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } 

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? LastLogin { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}

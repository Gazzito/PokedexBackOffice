using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Models;

namespace PokedexBackOffice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TotalDiamondPokemonsRanking> TotalDiamondPokemonsRankings { get; set; }
        public DbSet<TotalPacksOpenedRanking> TotalPacksOpenedRankings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PackUsers> PackUsers { get; set; }
        public DbSet<PokemonInPack> PokemonInPacks { get; set; }
        public DbSet<UserPokemons> UserPokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TotalDiamondPokemonsRanking)
                .WithOne(tdpr => tdpr.User)
                .HasForeignKey<TotalDiamondPokemonsRanking>(tdpr => tdpr.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.TotalPacksOpenedRanking)
                .WithOne(tpor => tpor.User)
                .HasForeignKey<TotalPacksOpenedRanking>(tpor => tpor.Id);

            // Configure many-to-many relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<PackUsers>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.PackUsers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<PackUsers>()
                .HasOne(pu => pu.Pack)
                .WithMany(p => p.PackUsers)
                .HasForeignKey(pu => pu.PackId);

            modelBuilder.Entity<PokemonInPack>()
                .HasOne(pip => pip.Pack)
                .WithMany(p => p.PokemonInPacks)
                .HasForeignKey(pip => pip.PackId);

            modelBuilder.Entity<PokemonInPack>()
                .HasOne(pip => pip.Pokemon)
                .WithMany(p => p.PokemonInPacks)
                .HasForeignKey(pip => pip.PokemonId);

      
           
        modelBuilder.Entity<UserPokemons>()
            .HasOne(up => up.Pack)
            .WithMany(p => p.UserPokemons)
            .HasForeignKey(up => up.PackId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserPokemons>()
            .HasOne(up => up.Pokemon)
            .WithMany(p => p.UserPokemons)
            .HasForeignKey(up => up.PokemonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserPokemons>()
            .HasOne(up => up.CreatedByUser)
            .WithMany(u => u.UserPokemonsCreatedBy)
            .HasForeignKey(up => up.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserPokemons>()
            .HasOne(up => up.UpdatedByUser)
            .WithMany(u => u.UserPokemonsUpdatedBy)
            .HasForeignKey(up => up.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

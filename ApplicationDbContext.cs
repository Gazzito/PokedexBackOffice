using Microsoft.EntityFrameworkCore;  // Importa o namespace para funcionalidades do Entity Framework Core
using PokedexBackOffice.Models;  // Importa o namespace para os modelos

namespace PokedexBackOffice.Data  // Define o namespace para o contexto de dados
{
    public class ApplicationDbContext : DbContext  // Define a classe ApplicationDbContext que herda de DbContext
    {
        // Construtor que inicializa o contexto de dados com opções específicas
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets para cada entidade do modelo
        public DbSet<User> Users { get; set; }  // Define um conjunto de entidades do tipo User
        public DbSet<UserProfile> UserProfiles { get; set; }  // Define um conjunto de entidades do tipo UserProfile
        public DbSet<TotalDiamondPokemonsRanking> TotalDiamondPokemonsRankings { get; set; }  // Define um conjunto de entidades do tipo TotalDiamondPokemonsRanking
        public DbSet<TotalPacksOpenedRanking> TotalPacksOpenedRankings { get; set; }  // Define um conjunto de entidades do tipo TotalPacksOpenedRanking
        public DbSet<Role> Roles { get; set; }  // Define um conjunto de entidades do tipo Role
        public DbSet<UserRole> UserRoles { get; set; }  // Define um conjunto de entidades do tipo UserRole
        public DbSet<Pack> Packs { get; set; }  // Define um conjunto de entidades do tipo Pack
        public DbSet<Region> Regions { get; set; }  // Define um conjunto de entidades do tipo Region
        public DbSet<Pokemon> Pokemons { get; set; }  // Define um conjunto de entidades do tipo Pokemon
        public DbSet<PackUsers> PackUsers { get; set; }  // Define um conjunto de entidades do tipo PackUsers
        public DbSet<PokemonInPack> PokemonInPacks { get; set; }  // Define um conjunto de entidades do tipo PokemonInPack
        public DbSet<UserPokemons> UserPokemons { get; set; }  // Define um conjunto de entidades do tipo UserPokemons

        // Método para configurar o modelo de dados e as relações entre as entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Chama a implementação base do método

            // Configuração de relacionamentos um-para-um
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)  // Um User tem um UserProfile
                .WithOne(up => up.User)  // Um UserProfile tem um User
                .HasForeignKey<UserProfile>(up => up.Id);  // Chave estrangeira é o Id do UserProfile

            modelBuilder.Entity<User>()
                .HasOne(u => u.TotalDiamondPokemonsRanking)  // Um User tem um TotalDiamondPokemonsRanking
                .WithOne(tdpr => tdpr.User)  // Um TotalDiamondPokemonsRanking tem um User
                .HasForeignKey<TotalDiamondPokemonsRanking>(tdpr => tdpr.Id);  // Chave estrangeira é o Id do TotalDiamondPokemonsRanking

            modelBuilder.Entity<User>()
                .HasOne(u => u.TotalPacksOpenedRanking)  // Um User tem um TotalPacksOpenedRanking
                .WithOne(tpor => tpor.User)  // Um TotalPacksOpenedRanking tem um User
                .HasForeignKey<TotalPacksOpenedRanking>(tpor => tpor.Id);  // Chave estrangeira é o Id do TotalPacksOpenedRanking

            // Configuração de relacionamentos muitos-para-muitos
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)  // Um UserRole tem um User
                .WithMany(u => u.UserRoles)  // Um User tem muitos UserRoles
                .HasForeignKey(ur => ur.UserId);  // Chave estrangeira é o UserId do UserRole

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)  // Um UserRole tem um Role
                .WithMany()  // Um Role pode ter muitos UserRoles
                .HasForeignKey(ur => ur.RoleId);  // Chave estrangeira é o RoleId do UserRole

            modelBuilder.Entity<PackUsers>()
                .HasOne(pu => pu.User)  // Um PackUsers tem um User
                .WithMany(u => u.PackUsers)  // Um User tem muitos PackUsers
                .HasForeignKey(pu => pu.UserId);  // Chave estrangeira é o UserId do PackUsers

            modelBuilder.Entity<PackUsers>()
                .HasOne(pu => pu.Pack)  // Um PackUsers tem um Pack
                .WithMany(p => p.PackUsers)  // Um Pack tem muitos PackUsers
                .HasForeignKey(pu => pu.PackId);  // Chave estrangeira é o PackId do PackUsers

            modelBuilder.Entity<PokemonInPack>()
                .HasOne(pip => pip.Pack)  // Um PokemonInPack tem um Pack
                .WithMany(p => p.PokemonInPacks)  // Um Pack tem muitos PokemonInPacks
                .HasForeignKey(pip => pip.PackId);  // Chave estrangeira é o PackId do PokemonInPack

            modelBuilder.Entity<PokemonInPack>()
                .HasOne(pip => pip.Pokemon)  // Um PokemonInPack tem um Pokemon
                .WithMany(p => p.PokemonInPacks)  // Um Pokemon tem muitos PokemonInPacks
                .HasForeignKey(pip => pip.PokemonId);  // Chave estrangeira é o PokemonId do PokemonInPack

            // Configuração de relacionamentos muitos-para-um com comportamento restritivo na eliminação
            modelBuilder.Entity<UserPokemons>()
                .HasOne(up => up.Pack)  // Um UserPokemons tem um Pack
                .WithMany(p => p.UserPokemons)  // Um Pack tem muitos UserPokemons
                .HasForeignKey(up => up.PackId)  // Chave estrangeira é o PackId do UserPokemons
                .OnDelete(DeleteBehavior.Restrict);  // Configura para restringir a eliminação

            modelBuilder.Entity<UserPokemons>()
                .HasOne(up => up.Pokemon)  // Um UserPokemons tem um Pokemon
                .WithMany(p => p.UserPokemons)  // Um Pokemon tem muitos UserPokemons
                .HasForeignKey(up => up.PokemonId)  // Chave estrangeira é o PokemonId do UserPokemons
                .OnDelete(DeleteBehavior.Restrict);  // Configura para restringir a eliminação

            modelBuilder.Entity<UserPokemons>()
                .HasOne(up => up.CreatedByUser)  // Um UserPokemons tem um User que o criou
                .WithMany(u => u.UserPokemonsCreatedBy)  // Um User pode ter criado muitos UserPokemons
                .HasForeignKey(up => up.CreatedBy)  // Chave estrangeira é o CreatedBy do UserPokemons
                .OnDelete(DeleteBehavior.Restrict);  // Configura para restringir a eliminação

            modelBuilder.Entity<UserPokemons>()
                .HasOne(up => up.UpdatedByUser)  // Um UserPokemons tem um User que o atualizou
                .WithMany(u => u.UserPokemonsUpdatedBy)  // Um User pode ter atualizado muitos UserPokemons
                .HasForeignKey(up => up.UpdatedBy)  // Chave estrangeira é o UpdatedBy do UserPokemons
                .OnDelete(DeleteBehavior.Restrict);  // Configura para restringir a eliminação
        }
    }
}

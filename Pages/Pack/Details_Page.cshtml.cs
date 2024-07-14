using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokedexBackOffice.Data;

namespace PokedexBackOffice.Pages.Packs
{
    public class DetailsModelPack : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModelPack(ApplicationDbContext context)
        {
            _context = context;
        }

        public PackDTO Pack { get; set; }
        public List<SelectListItem> Pokemons { get; set; }
        public List<PokemonDTO> AssociatedPokemons { get; set; }

        [BindProperty]
        public int PokemonId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pack = await _context.Packs
                .Include(p => p.PokemonInPacks)
                .ThenInclude(pp => pp.Pokemon)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pack == null)
            {
                return NotFound();
            }

            Pack = new PackDTO
            {
                Id = pack.Id,
                Name = pack.Name,
                Price = pack.Price,
                BronzeChance = pack.BronzeChance,
                SilverChance = pack.SilverChance,
                GoldChance = pack.GoldChance,
                PlatinumChance = pack.PlatinumChance,
                DiamondChance = pack.DiamondChance,
                TotalBought = pack.TotalBought,
                CreatedOn = pack.CreatedOn,
                UpdatedOn = pack.UpdatedOn,
                Image = pack.Image != null ? Convert.ToBase64String(pack.Image) : null
            };

            Pokemons = await _context.Pokemons
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToListAsync();

            AssociatedPokemons = pack.PokemonInPacks
                .Select(pp => new PokemonDTO
                {
                    Id = pp.Pokemon.Id,
                    Name = pp.Pokemon.Name
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAssociatePokemonAsync(int id)
        {
            var pack = await _context.Packs
                .Include(p => p.PokemonInPacks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pack == null)
            {
                return NotFound();
            }

            if (PokemonId != 0)
            {
                var pokemonInPack = new PokemonInPack
                {
                    PackId = id,
                    PokemonId = PokemonId,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 1 // Substitua pelo ID do usuário logado
                };

                _context.PokemonInPacks.Add(pokemonInPack);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }

        public async Task<IActionResult> OnPostRemovePokemonAsync(int id, int pokemonId)
        {
            var pack = await _context.Packs
                .Include(p => p.PokemonInPacks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pack == null)
            {
                return NotFound();
            }

            var pokemonInPack = pack.PokemonInPacks.FirstOrDefault(p => p.PokemonId == pokemonId);
            if (pokemonInPack != null)
            {
                _context.PokemonInPacks.Remove(pokemonInPack);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = id });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Pokemons
{
    public class DeleteModelPokemon : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModelPokemon(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PokemonDTO Pokemon { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pokemon = await _context.Pokemons
                .Include(p => p.Region)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pokemon == null)
            {
                return NotFound();
            }

            Pokemon = new PokemonDTO
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                RegionName = pokemon.Region.Name,
                BaseAttackPoints = pokemon.BaseAttackPoints,
                BaseHealthPoints = pokemon.BaseHealthPoints,
                BaseDefensePoints = pokemon.BaseDefensePoints,
                BaseSpeedPoints = pokemon.BaseSpeedPoints
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);

            if (pokemon == null)
            {
                return NotFound();
            }

            _context.Pokemons.Remove(pokemon);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

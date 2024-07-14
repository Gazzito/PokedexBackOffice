using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Pokemons
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
                RegionId = pokemon.RegionId,
                RegionName = pokemon.Region.Name,
                BaseAttackPoints = pokemon.BaseAttackPoints,
                BaseHealthPoints = pokemon.BaseHealthPoints,
                BaseDefensePoints = pokemon.BaseDefensePoints,
                BaseSpeedPoints = pokemon.BaseSpeedPoints,
                CreatedOn = pokemon.CreatedOn,
                UpdatedOn = pokemon.UpdatedOn,
                Image = pokemon.Image != null ? Convert.ToBase64String(pokemon.Image) : null
            };

            return Page();
        }
    }
}

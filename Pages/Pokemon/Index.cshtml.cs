using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Pokemons
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PokemonDTO> Pokemon { get; set; }

        public async Task OnGetAsync()
        {
            Pokemon = await _context.Pokemons
                .Include(p => p.Region) // Inclui a região associada
                .Select(p => new PokemonDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    RegionName = p.Region.Name, // Seleciona o nome da região
                    BaseAttackPoints = p.BaseAttackPoints,
                    BaseHealthPoints = p.BaseHealthPoints,
                    BaseDefensePoints = p.BaseDefensePoints,
                    BaseSpeedPoints = p.BaseSpeedPoints
                })
                .ToListAsync();
        }
    }
}

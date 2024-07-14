using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Models;
using PokedexBackOffice.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Packs
{
    public class IndexModelPack : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModelPack(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PackDTO> Packs { get; set; }

        public async Task OnGetAsync()
        {
            Packs = await _context.Packs
                .Select(p => new PackDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    BronzeChance = p.BronzeChance,
                    SilverChance = p.SilverChance,
                    GoldChance = p.GoldChance,
                    PlatinumChance = p.PlatinumChance,
                    DiamondChance = p.DiamondChance,
                    TotalBought = p.TotalBought
                })
                .ToListAsync();
        }
    }
}

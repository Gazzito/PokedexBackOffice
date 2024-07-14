using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pack = await _context.Packs.FirstOrDefaultAsync(m => m.Id == id);

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

            return Page();
        }
    }
}

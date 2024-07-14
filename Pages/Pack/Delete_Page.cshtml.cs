using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Models;
using PokedexBackOffice.Data;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Packs
{
    public class DeleteModelPack : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModelPack(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
                TotalBought = pack.TotalBought
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var pack = await _context.Packs.FindAsync(id);

            if (pack == null)
            {
                return NotFound();
            }

            _context.Packs.Remove(pack);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

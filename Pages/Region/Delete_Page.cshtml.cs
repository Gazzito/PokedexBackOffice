using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Threading.Tasks;

namespace PokedexBackOffice.Pages.Regions
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegionDTO Region { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            Region = new RegionDTO
            {
                Id = region.Id,
                Name = region.Name,
                CreatedOn = region.CreatedOn,
                CreatedBy = region.CreatedBy,
                UpdatedOn = region.UpdatedOn,
                UpdatedBy = region.UpdatedBy
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

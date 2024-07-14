using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PokedexBackOffice.Pages.Regions
{
    public class CreateModelRegion : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModelRegion(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegionDTO Region { get; set; } = new RegionDTO();

        public IActionResult OnGet()
        {
            Region = new RegionDTO(); // Inicializa a propriedade Region aqui
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var systemUser = _context.Users
                                     .Include(u => u.UserRoles)
                                     .ThenInclude(ur => ur.Role)
                                     .FirstOrDefault(u => u.UserRoles.Any(ur => ur.Role.Name == "System"));

            if (systemUser == null)
            {
                ModelState.AddModelError(string.Empty, "System user not found.");
                return Page();
            }

            var region = new Models.Region
            {
                Name = Region.Name,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = systemUser.Id,
                UpdatedOn = Region.UpdatedOn?.ToUniversalTime(),
                UpdatedBy = systemUser.Id
            };


            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

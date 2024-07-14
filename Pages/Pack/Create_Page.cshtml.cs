using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PokedexBackOffice.Models;
using PokedexBackOffice.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PokedexBackOffice.Pages.Packs
{
    public class CreateModelPack : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModelPack(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PackDTO Pack { get; set; } = new PackDTO();

        [BindProperty]
        public IFormFile Upload { get; set; }

        public IActionResult OnGet()
        {
            Pack = new PackDTO(); // Inicializa a propriedade Pack aqui
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

            var pack = new Models.Pack
            {
                Name = Pack.Name,
                Price = Pack.Price,
                BronzeChance = Pack.BronzeChance,
                SilverChance = Pack.SilverChance,
                GoldChance = Pack.GoldChance,
                PlatinumChance = Pack.PlatinumChance,
                DiamondChance = Pack.DiamondChance,
                TotalBought = Pack.TotalBought,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = systemUser.Id,
                UpdatedOn = Pack.UpdatedOn?.ToUniversalTime(),
                UpdatedBy = systemUser.Id
            };

            if (Upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);
                    pack.Image = memoryStream.ToArray();
                }
            }

            _context.Packs.Add(pack);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

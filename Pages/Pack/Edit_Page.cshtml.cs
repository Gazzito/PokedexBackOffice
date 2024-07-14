using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PokedexBackOffice.Models;
using PokedexBackOffice.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PokedexBackOffice.Pages.Packs
{
    public class EditModelPack : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModelPack> _logger;

        public EditModelPack(ApplicationDbContext context, ILogger<EditModelPack> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public PackDTO Pack { get; set; } = new PackDTO();

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var pack = await _context.Packs.FindAsync(id);

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
                CreatedBy = pack.CreatedBy,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = 1,
                Image = pack.Image != null ? Convert.ToBase64String(pack.Image) : null
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync called.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var pack = await _context.Packs.FindAsync(Pack.Id);

            if (pack == null)
            {
                _logger.LogError($"Pack with ID {Pack.Id} not found.");
                return NotFound();
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

            _logger.LogInformation($"Updating Pack with ID {Pack.Id}.");
            pack.Name = Pack.Name;
            pack.Price = Pack.Price;
            pack.BronzeChance = Pack.BronzeChance;
            pack.SilverChance = Pack.SilverChance;
            pack.GoldChance = Pack.GoldChance;
            pack.PlatinumChance = Pack.PlatinumChance;
            pack.DiamondChance = Pack.DiamondChance;
            pack.TotalBought = Pack.TotalBought;
            pack.UpdatedOn = DateTime.UtcNow;
            pack.UpdatedBy = systemUser.Id;

            if (Upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);
                    pack.Image = memoryStream.ToArray();
                    Pack.Image = Convert.ToBase64String(pack.Image);
                }
                _logger.LogInformation("File uploaded successfully.");
            }

            _context.Packs.Update(pack);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pack updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Pack: {ex.Message}");
            }

            return RedirectToPage("./Index");
        }
    }
}

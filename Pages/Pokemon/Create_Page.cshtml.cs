using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace PokedexBackOffice.Pages.Pokemons
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PokemonDTO Pokemon { get; set; } = new PokemonDTO();

        [BindProperty]
        public IFormFile Upload { get; set; }

        public SelectList Regions { get; set; }

        public IActionResult OnGet()
        {
            Regions = new SelectList(_context.Regions.ToList(), "Id", "Name");
            Pokemon = new PokemonDTO(); // Inicializa a propriedade Pokemon aqui
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Regions = new SelectList(_context.Regions.ToList(), "Id", "Name");
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

            var pokemon = new Models.Pokemon
            {
                Name = Pokemon.Name,
                RegionId = Pokemon.RegionId,
                BaseAttackPoints = Pokemon.BaseAttackPoints,
                BaseHealthPoints = Pokemon.BaseHealthPoints,
                BaseDefensePoints = Pokemon.BaseDefensePoints,
                BaseSpeedPoints = Pokemon.BaseSpeedPoints,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = systemUser.Id,
                UpdatedOn = Pokemon.UpdatedOn?.ToUniversalTime(),
                UpdatedBy = systemUser.Id
            };

            if (Upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);
                    pokemon.Image = memoryStream.ToArray();
                }
            }

            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

}

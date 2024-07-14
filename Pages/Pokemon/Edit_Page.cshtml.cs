using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace PokedexBackOffice.Pages.Pokemons
{
    public class EditModelPokemon : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModelPokemon> _logger;

        public EditModelPokemon(ApplicationDbContext context, ILogger<EditModelPokemon> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public PokemonDTO Pokemon { get; set; } = new PokemonDTO();

        [BindProperty]
        public IFormFile ?Upload { get; set; }

        public SelectList Regions { get; set; }

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
                CreatedBy = 1,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = 1,
                Image = pokemon.Image != null ? Convert.ToBase64String(pokemon.Image) : null
            };

            Regions = new SelectList(_context.Regions.ToList(), "Id", "Name", Pokemon.RegionId);

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
                Regions = new SelectList(_context.Regions.ToList(), "Id", "Name", Pokemon.RegionId);
                return Page();
            }

            var pokemon = await _context.Pokemons.FindAsync(Pokemon.Id);

            if (pokemon == null)
            {
                _logger.LogError($"Pokemon with ID {Pokemon.Id} not found.");
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

            _logger.LogInformation($"Updating Pokemon with ID {Pokemon.Id}.");
            pokemon.Name = Pokemon.Name;
            pokemon.RegionId = Pokemon.RegionId;
            pokemon.BaseAttackPoints = Pokemon.BaseAttackPoints;
            pokemon.BaseHealthPoints = Pokemon.BaseHealthPoints;
            pokemon.BaseDefensePoints = Pokemon.BaseDefensePoints;
            pokemon.BaseSpeedPoints = Pokemon.BaseSpeedPoints;
            pokemon.UpdatedOn = DateTime.UtcNow;
            pokemon.UpdatedBy = systemUser.Id;

            if (Upload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Upload.CopyToAsync(memoryStream);
                    pokemon.Image = memoryStream.ToArray();
                    Pokemon.Image = Convert.ToBase64String(pokemon.Image);
                }
                _logger.LogInformation("File uploaded successfully.");
            }

            _context.Pokemons.Update(pokemon);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Pokemon updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Pokemon: {ex.Message}");
            }

            return RedirectToPage("./Index");
        }
    }
}

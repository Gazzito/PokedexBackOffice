using Microsoft.AspNetCore.Mvc.RazorPages;
using PokedexBackOffice.Data;
using PokedexBackOffice.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PokedexBackOffice.Pages.Regions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<RegionDTO> Regions { get; set; }

        public async Task OnGetAsync()
        {
            Regions = await _context.Regions
                .Select(r => new RegionDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedOn = r.CreatedOn,
                    CreatedBy = r.CreatedBy,
                    UpdatedOn = r.UpdatedOn,
                    UpdatedBy = r.UpdatedBy
                }).ToListAsync();
        }
    }
}

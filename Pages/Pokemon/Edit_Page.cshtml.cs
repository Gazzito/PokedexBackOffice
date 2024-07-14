using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PokedexBackOffice.Pages.Pokemon
{
    public class Edit_Page : PageModel
    {
        private readonly ILogger<Edit_Page> _logger;

        public Edit_Page(ILogger<Edit_Page> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
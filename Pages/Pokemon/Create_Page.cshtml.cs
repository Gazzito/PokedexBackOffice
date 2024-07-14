using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PokedexBackOffice.Pages.Pokemon
{
    public class Create_Page : PageModel
    {
        private readonly ILogger<Create_Page> _logger;

        public Create_Page(ILogger<Create_Page> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
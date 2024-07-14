using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PokedexBackOffice.Pages.Pokemon
{
    public class Delete_Page : PageModel
    {
        private readonly ILogger<Delete_Page> _logger;

        public Delete_Page(ILogger<Delete_Page> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
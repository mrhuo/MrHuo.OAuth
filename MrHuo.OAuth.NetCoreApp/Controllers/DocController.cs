using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MrHuo.OAuth.NetCoreApp.Controllers
{
    public class DocController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

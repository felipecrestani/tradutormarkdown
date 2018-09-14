using Microsoft.AspNetCore.Mvc;

namespace TranslateApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index","Translate");
        }
    }
}

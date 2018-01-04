using System.Web.Mvc;

namespace Baner.Recepcion.Services.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "Servicios CRM";
            return View();
        }
    }
}
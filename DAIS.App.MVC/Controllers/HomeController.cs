using System.Web.Mvc;

namespace DAIS.App.MVC.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
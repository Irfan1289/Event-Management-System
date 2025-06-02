using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagementFrontend.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            base.OnActionExecuting(context);
        }
    }
}

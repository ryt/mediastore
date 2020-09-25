using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mediastore.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext) 
        {
            if ( ViewBag.Teacher == null )
            {
                ViewBag.Teacher = Request.Cookies["teacher"];
            }
        }
    }
}
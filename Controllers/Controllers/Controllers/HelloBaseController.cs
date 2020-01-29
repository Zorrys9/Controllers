using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers.Controllers
{
    public class HelloBaseController :Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var userAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                if(userAgent.Contains("MSIE")|| userAgent.Contains("Trident"))
                {
                    context.Result = Content("Данный браузер не поддерживается нашим сайтом");
                }
            }
            base.OnActionExecuting(context);
        }
    }
}

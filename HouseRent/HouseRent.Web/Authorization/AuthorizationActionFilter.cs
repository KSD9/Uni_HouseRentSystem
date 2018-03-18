using HouseRent.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HouseRent.Web.Authorization
{
    public class AuthorizationActionFilter : ActionFilterAttribute
    {
        private bool user;

        public AuthorizationActionFilter(bool user)
        {
            this.user = user;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AuthenticationController.logged == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {{"controller", "Home"}, {"action", "Error"}});

                filterContext.HttpContext.Response.StatusCode = 401;

                base.OnActionExecuting(filterContext);

                return;
            }

            //if it is not logged admin and it is forbidden for user
            if (!AuthenticationController.IsLoggedAdmin)
            {
                if (!user)
                {
                    filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                        {{"controller", "Home"}, {"action", "Error"}});

                    filterContext.HttpContext.Response.StatusCode = 401;

                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}

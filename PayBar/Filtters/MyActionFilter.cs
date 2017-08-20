using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayBar.Filtters
{
    public class MyActionFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(r => r.Value == null) || !actionContext.ModelState.IsValid)
                throw new Exception(Texts.InvalidData);

            base.OnActionExecuting(actionContext);
        }
    }
}
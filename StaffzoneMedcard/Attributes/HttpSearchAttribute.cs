using System.Reflection;
using System.Web.Mvc;

namespace StaffZoneMaster.Attributes
{
    public class HttpSearchAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request.HttpMethod.ToUpper() == "SEARCH";
        }
    }

}


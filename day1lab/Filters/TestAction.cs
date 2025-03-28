using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoAPI.Filters
{
    public class TestAction:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"Before Action : {context.ActionDescriptor.DisplayName}");
            Console.WriteLine($"Arguments : {context.ActionArguments["Dept"]}");
        }
    }
}

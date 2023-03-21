using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace PyroDB.Helpers
{
    public static class Rendering
    {
        //https://weblog.west-wind.com/posts/2022/Jun/21/Back-to-Basics-Rendering-Razor-Views-to-String-in-ASPNET-Core
        public static async Task<string> RenderPartialViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            var actionContext = controllerContext as ActionContext;
            var serviceProvider = controllerContext.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetService<IRazorViewEngine>();
            var tempDataProvider = serviceProvider.GetService<ITempDataProvider>();

            if (razorViewEngine == null)
                return "Error 500 razorViewEngine";

            if (tempDataProvider == null)
                return "Error 500 tempDataProvider";

            using var sw = new StringWriter();

            var viewResult = razorViewEngine.FindView(actionContext, viewName, false);  //false because partial
            if (viewResult?.View == null)
                return "404 Partial not found";

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();

        }
    }
}

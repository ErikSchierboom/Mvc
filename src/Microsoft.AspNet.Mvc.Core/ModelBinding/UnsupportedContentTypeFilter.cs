using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    public class UnsupportedContentTypeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (HasUnsupportedContentTypeError(context))
            {
                context.Result = new HttpStatusCodeResult(415);
            }
        }

        private bool HasUnsupportedContentTypeError(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (modelState.IsValid)
            {
                return false;
            }

            var unsupportedContentTypeExceptions = modelState.SelectMany(
                kvp => kvp.Value.Errors.Select(e => new KeyValuePair<string, Exception>(kvp.Key, e.Exception))
                    .Where(e => e.Value != null && e.Value is UnsupportedContentTypeException));

            return unsupportedContentTypeExceptions.Any();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

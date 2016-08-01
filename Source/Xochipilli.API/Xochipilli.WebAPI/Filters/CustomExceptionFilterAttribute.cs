using Xochipilli.WebAPI.Classes;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Xochipilli.WebAPI.Filters
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            bool naturalException = true;

            if (context.Exception is AggregateException)
            {
                naturalException = false;

                foreach (var e in ((AggregateException)context.Exception).InnerExceptions)
                {
                    if (e is Exception)
                    {
                        Trace.TraceError(string.Format("Exception: {0}. ", ExceptionManager.GetExceptionText(context.Exception)));

                        var message = new HttpResponseMessage();
                        message.ReasonPhrase = string.Format("Se presentó un problema del lado del servidor: {0}.", ExceptionManager.GetExceptionText(context.Exception));
                        message.StatusCode = HttpStatusCode.BadRequest;
                        context.Response = message;
                    }
                }
            }

            if (naturalException)
            {
                if (context.Exception is Exception)
                {
                    Trace.TraceError(string.Format("Exception: {0}. ", ExceptionManager.GetExceptionText(context.Exception)));

                    var message = new HttpResponseMessage();
                    //message.ReasonPhrase = string.Format("Se presentó un problema del lado del servidor: {0}.", ExceptionManager.GetExceptionText(context.Exception));
                    message.StatusCode = HttpStatusCode.BadRequest;
                    context.Response = message;
                }
            }
        }
    }
}
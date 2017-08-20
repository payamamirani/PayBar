using PayBar.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Net.Http;

namespace PayBar.Filtters
{
    public class CustomerExceptionHandle : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var customObject = new Result
            {
                error_message = context.Exception.Message,
                success = false,
                data = null
            };

            var jsonType = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonType.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, customObject, jsonType);

            context.Result = new ResponseMessageResult(response);

            return Task.FromResult(0);
        }
    }
}
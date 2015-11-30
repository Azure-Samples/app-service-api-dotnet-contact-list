using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using CompanyContacts.API.Filters;
using System.Security.Claims;
using CompanyContacts.API.Models;

namespace CompanyContacts.API.Filters
{
    using System.Web.Http.Filters;

    public class HttpOperationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Microsoft.Rest.HttpOperationException)
            {
                var ex = (Microsoft.Rest.HttpOperationException)context.Exception;
                context.Response = ex.Response;
            }
        }
    }
}

namespace CompanyContacts.API.Controllers
{
    [HttpOperationExceptionFilterAttribute]
    public class ContactsController : ApiController
    {

        public ContactsController()
        {
        }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<Contact>))]
        public async Task<IEnumerable<Contact>> Get()
        {
            var contacts = new Contact[]{
                        new Contact { Id = 1, EmailAddress = "nancy@contoso.com", Name = "Nancy Davolio"},
                        new Contact { Id = 2, EmailAddress = "alexander@contoso.com", Name = "Alexander Carson"}
                    };

            return contacts;
        }

    }
}

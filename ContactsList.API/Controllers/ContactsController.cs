using ContactsList.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.Cors;
using ContactsList.API.Filters;
using System.Security.Claims;
using System;
using System.Configuration;
using System.Net.Http.Headers;

namespace ContactsList.API.Filters
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

namespace ContactsList.API.Controllers
{
    [HttpOperationExceptionFilterAttribute]
    // Uncomment the following line to enable CORS
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContactsController : ApiController
    {
        private const string FILENAME = "contacts.json";
        private GenericStorage _storage;

        private static CompanyContactsAPI CompanyContactsAPIClientWithAuth()
        {
            var client = new CompanyContactsAPI(new Uri(ConfigurationManager.AppSettings["CompanyContactsAPIUrl"]));
            client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ServicePrincipal.GetS2SAccessTokenForProdMSA().AccessToken);
            return client;
        }



        public ContactsController()
        {
            _storage = new GenericStorage();
        }

        private async Task<IEnumerable<Contact>> GetContacts()
        {
            var contacts = await _storage.Get(FILENAME);

            if (contacts == null)
            {
                contacts = await _storage.Save(new Contact[]{
                        new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                        new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                        new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"}
                    }
                , FILENAME);

            }

            var contactsList = contacts.ToList<Contact>();
            //Uncomment the following using block to call the CompanyContacts API
            //using (var client = CompanyContactsAPIClientWithAuth())
            //{
            //    var results = await client.Contacts.GetAsync();
            //    foreach (Contact c in results)
            //    {
            //        contactsList.Add(c);
            //    }
            //}

            return contactsList;
        }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(IEnumerable<Contact>))]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await GetContacts();
        }

        /// <summary>
        /// Gets a specific contact
        /// </summary>
        /// <param name="id">Identifier for the contact</param>
        /// <returns>The requested contact</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(IEnumerable<Contact>))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(IEnumerable<Contact>))]
        [SwaggerOperation("GetContactById")]
        public async Task<Contact> Get([FromUri] int id)
        {
            var contacts = await GetContacts();
            return contacts.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The new contact</param>
        /// <returns>The saved contact</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created,
            Description = "Created",
            Type = typeof(Contact))]
        public async Task<Contact> Post([FromBody] Contact contact)
        {
            var contacts = await GetContacts();
            var contactList = contacts.ToList();

            contact.CreatedBy = ((ClaimsIdentity)User.Identity)?.FindFirst(ClaimTypes.Email)?.Value;
            contactList.Add(contact);
            await _storage.Save(contactList, FILENAME);
            return contact;
        }

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="contact">The new contact values</param>
        /// <returns>The updated contact</returns>
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "Updated",
            Type = typeof(Contact))]
        public async Task<Contact> Put([FromBody] Contact contact)
        {
            await Delete(contact.Id.Value);
            await Post(contact);
            return contact;
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Identifier of the contact to be deleted</param>
        /// <returns>True if the contact was deleted</returns>
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(bool))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(bool))]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var contacts = await GetContacts();
            var contactList = contacts.ToList();

            if (!contactList.Any(x => x.Id == id))
            {
                return Request.CreateResponse<bool>(HttpStatusCode.NotFound, false);
            }
            else
            {
                contactList.RemoveAll(x => x.Id == id);
                await _storage.Save(contactList, FILENAME);
                return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
            }
        }
    }
}

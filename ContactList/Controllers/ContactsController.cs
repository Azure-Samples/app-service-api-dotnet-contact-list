using ContactList.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace ContactList.Controllers
{
    public class ContactsController : ApiController
    {
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return new Contact[]
            {
                new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"}
            };
        }

        [HttpPost]
        public Contact Post([FromBody] Contact contact)
        {
            return contact;
        }
    }
}

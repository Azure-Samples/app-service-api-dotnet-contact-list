using ContactList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Linq;

namespace ContactList.Controllers
{
    // Comment out this line if you prefer to control CORS using the Azure Portal
    [EnableCors(origins:"*", headers:"*", methods: "*")] /* -- NEW CODE -- */
    public class ContactsController : ApiController
    {
        private const string FILENAME = "contacts.json";
        private GenericStorage _storage;

        public ContactsController()
        {
            _storage = new GenericStorage();
        }

        private async Task<IEnumerable<Contact>> GetContact()
        {
            var contacts = await _storage.Get(FILENAME);

            if (contacts == null)
            {
                await _storage.Save(new Contact[]{
                        new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                        new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                        new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"}
                    }
                , FILENAME);
            }

            return contacts;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<Contact>))]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await GetContact();
        }

        [HttpGet]
        [ResponseType(typeof(Contact))]
        public async Task<Contact> GetById([FromUri] int id)
        {
            var contacts = await GetContact();
            return contacts.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        [ResponseType(typeof(Contact))]
        public async Task<Contact> Post([FromBody] Contact contact)
        {
            var contacts = await GetContact();
            var contactList = contacts.ToList();
            contactList.Add(contact);
            await _storage.Save(contactList, FILENAME);
            return contact;
        }

        [HttpDelete]
        [ResponseType(typeof(bool))]
        public async Task<bool> Delete([FromUri] int id)
        {
            var contacts = await GetContact();
            var contactList = contacts.ToList();
            contactList.RemoveAll(x => x.Id == id);
            await _storage.Save(contactList, FILENAME);
            return true;
        }
    }
}

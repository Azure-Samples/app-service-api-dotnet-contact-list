using ContactList.Models;
using Microsoft.Azure.AppService.ApiApps.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactList
{
    public class GenericStorage
    {
        CloudIsolatedStorage storage;

        public GenericStorage()
        {
            storage = Runtime.FromAppSettings().IsolatedStorage;
        }

        public async Task Save(IEnumerable<Contact> target, string filename)
        {
            var json = JsonConvert.SerializeObject(target);
            var data = Encoding.ASCII.GetBytes(json);
            await storage.WriteAsync(filename, data);
        }

        public async Task<IEnumerable<Contact>> Get(string filename)
        {
            var json = await storage.ReadAsStringAsync(filename);
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<IEnumerable<Contact>>(json);
        }
    }
}

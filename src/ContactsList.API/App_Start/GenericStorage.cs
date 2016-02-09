using ContactsList.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList.API
{
    public class GenericStorage
    {
        private string _filePath;

        public GenericStorage()
        {
            var webAppsHome = Environment.GetEnvironmentVariable("HOME")?.ToString();
            if (String.IsNullOrEmpty(webAppsHome))
            {
                _filePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "\\";
            }
            else
            {
                _filePath = webAppsHome + "\\site\\wwwroot\\";
            }
        }

        public async Task<IEnumerable<Contact>> Save(IEnumerable<Contact> target, string filename)
        {
            var json = JsonConvert.SerializeObject(target);
            File.WriteAllText(_filePath + filename, json);
            return target;
        }

        public async Task<IEnumerable<Contact>> Get(string filename)
        {
            var contactsText = String.Empty;
            if (File.Exists(_filePath + filename))
            {
                contactsText = File.ReadAllText(_filePath + filename);
            }

            var contacts = JsonConvert.DeserializeObject<Contact[]>(contactsText);
            return contacts;
        }
    }
}

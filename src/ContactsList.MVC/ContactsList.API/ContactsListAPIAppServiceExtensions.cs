using System;
using System.Net.Http;
using Microsoft.Azure.AppService;

namespace ContactsList.MVC
{
    public static class ContactsListAPIAppServiceExtensions
    {
        public static ContactsListAPI CreateContactsListAPI(this IAppServiceClient client)
        {
            return new ContactsListAPI(client.CreateHandler());
        }

        public static ContactsListAPI CreateContactsListAPI(this IAppServiceClient client, params DelegatingHandler[] handlers)
        {
            return new ContactsListAPI(client.CreateHandler(handlers));
        }

        public static ContactsListAPI CreateContactsListAPI(this IAppServiceClient client, Uri uri, params DelegatingHandler[] handlers)
        {
            return new ContactsListAPI(uri, client.CreateHandler(handlers));
        }

        public static ContactsListAPI CreateContactsListAPI(this IAppServiceClient client, HttpClientHandler rootHandler, params DelegatingHandler[] handlers)
        {
            return new ContactsListAPI(rootHandler, client.CreateHandler(handlers));
        }
    }
}

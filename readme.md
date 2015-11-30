---
services: app-service\api
platforms: dotnet
author: bradygaster
---

# Contact List Azure API app Sample #

The Contact List sample is used by several tutorials that show how to create, deploy, consume, and debug [API apps](http://azure.microsoft.com/en-us/documentation/articles/app-service-api-apps-why-best-platform/ "What are API Apps?") in Azure App Service.

For the simplest scenario without authentication the example includes a Web API back end and two simple clients, one HTML/AngularJS and one ASP.NET MVC 5.  

For the authentication scenario the example includes an AngularJS client configured for Azure Active Directory and an additional Web API back-end. The Angular.AAD client calls the ContactsList.API backend using a token for an individual user, and the ContactsList.API backend calls the AzureADUsers.API backend using a service principal token.

## More Information ##

- [Get started with API Apps and ASP.NET in Azure App Service](https://azure.microsoft.com/documentation/articles/app-service-api-dotnet-get-started/)

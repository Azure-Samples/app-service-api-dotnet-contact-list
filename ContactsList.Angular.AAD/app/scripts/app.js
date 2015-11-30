'use strict';
angular.module('contactsListApp', ['ngRoute','AdalAngular'])
.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {

    $routeProvider.when("/Home", {
        controller: "homeCtrl",
        templateUrl: "/App/Views/Home.html",
    }).when("/Contacts", {
        controller: "contactsCtrl",
        templateUrl: "/App/Views/Contacts.html",
        requireADLogin: true,
    }).when("/UserData", {
        controller: "userDataCtrl",
        templateUrl: "/App/Views/UserData.html",
    }).otherwise({ redirectTo: "/Home" });

    var endpoints = { 
        //"https://{your api app name}.azurewebsites.net/": "{yourclientid}"
        "https://localhost:44300/": "{your client id}"
    };

    adalProvider.init(
        {
            instance: 'https://login.microsoftonline.com/', 
            tenant: '{your tenant url}',
            clientId: '{your client id}',
            extraQueryParameter: 'nux=1',
            endpoints: endpoints
            //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
        },
        $httpProvider
        );
   
}]);

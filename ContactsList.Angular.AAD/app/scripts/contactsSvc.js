'use strict';
angular.module('contactsListApp')
.factory('contactsSvc', ['$http', function ($http) {
    //var apiEndpoint = "https://{your api app name}.azurewebsites.net";
    var apiEndpoint = "https://localhost:44300";

    $http.defaults.useXDomain = true;
    delete $http.defaults.headers.common['X-Requested-With']; 

    return {
        getItems: function () {
            return $http.get(apiEndpoint + '/api/contacts');
        },
        getItem : function(id){
            return $http.get(apiEndpoint + '/api/contacts/' + id);
        },
        postItem : function(item){
            return $http.post(apiEndpoint + '/api/contacts', item);
        },
        putItem : function(item){
            return $http.put(apiEndpoint + '/api/contacts/', item);
        },
        deleteItem : function(id){
            return $http({
                method: 'DELETE',
                url: apiEndpoint + '/api/contacts/' + id
            });
        }
    };
}]);
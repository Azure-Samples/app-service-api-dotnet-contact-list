'use strict';
angular.module('contactsListApp')
.controller('contactsCtrl', ['$scope', '$location', 'contactsSvc', 'adalAuthenticationService', function ($scope, $location, contactsSvc, adalService) {
    $scope.error = "";
    $scope.loadingMessage = "Loading...";
    $scope.contactsList = null;
    $scope.editingInProgress = false;
    $scope.newId = "";
    $scope.newName = "";
    $scope.newEmail = "";

    $scope.editInProgressContact = {
        Id: 0,
        Name: "",
        EmailAddress: ""
    };

    

    $scope.editSwitch = function (contact) {
        contact.edit = !contact.edit;
        if (contact.edit) {
            $scope.editInProgressContact.Id = contact.Id;
            $scope.editInProgressContact.Name = contact.Name;
            $scope.editInProgressContact.EmailAddress = contact.EmailAddress;
            $scope.editingInProgress = true;
        } else {
            $scope.editingInProgress = false;
        }
    };

    $scope.populate = function () {
        contactsSvc.getItems().then(function (results) {
            $scope.contactsList = results.data;
            $scope.loadingMessage = "";
        }, function (err) {
            $scope.loadingMessage = "";
            $scope.error = "Error: " + err;
        });
    };
    $scope.delete = function (id) {
        contactsSvc.deleteItem(id).then(function (results) {
            $scope.loadingMessage = "";
            $scope.populate();
        }, function (err) {
            $scope.error = "Error: " + err;
            $scope.loadingMessage = "";
        })
    };
    $scope.update = function (contact) {
        contactsSvc.putItem($scope.editInProgressContact).success(function (results) {
            $scope.loadingMsg = "";
            $scope.populate();
            $scope.editSwitch(contact);
        }).error(function (err) {
            $scope.error = "Error: " + err;
            $scope.loadingMessage = "";
        })
    };
    $scope.create = function () {
        contactsSvc.postItem({
            'Id': $scope.newId,
            'Name': $scope.newName,
            'EmailAddress': $scope.newEmailAddress,
            'CreatedBy': adalService.userInfo.userName
        }).then(function (results) {
            $scope.loadingMsg = "";
            $scope.newId = "";
            $scope.newName = "";
            $scope.newEmailAddress = "";
            $scope.populate();
        }, function (err) {
            $scope.loadingMsg = "Error: " + err;
        })
    };
}]);
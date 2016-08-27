(function () {
    'use strict';

    var searchFriendDirectiveController = function searchFriendDirectiveController($scope, data, signalR) {
        var vm = this;

        signalR.initialize();

        $scope.getUsers = function(value) {
            return data.get('Account/Users').then(function(response){
                return substringMatcher(value, response.data)
            });
        };

        this.onSelect = function ($item) {
            signalR.sendRequest($item);
            vm.selectedFriend = '';
        };
    };

    function substringMatcher(q, strs) {
        var matches = [];

        var substrRegex = new RegExp(q, 'i');
        strs.forEach(function(entry) {
            if (substrRegex.test(entry)) {
                matches.push(entry);
            }
        });

        return matches;
    }

    angular
        .module('ToDoApp.controllers')
        .controller('SearchFriendDirectiveController', ['$scope', 'data', 'signalR', searchFriendDirectiveController]);
}());
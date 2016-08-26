(function () {
    'use strict';

    var searchFriendDirectiveController = function searchFriendDirectiveController($scope, data) {
        var vm = this;
        $scope.getUsers = function(value) {
        //     $.ajax({
        //         method: 'GET',
        //         url: BASE_URL + ACCOUNT_URL + 'Users',
        //         headers: {'Authorization': 'Bearer ' + authToken}
        //     })
        //         .done(function (response) {
        //             let arr = [];
        //
        //             for (let user of response) {
        //                 arr.push(user)
        //             }
        //
        //             asyncResults(substringMatcher(query, arr));
        //         });
        // }, 700);
            return data.get('Account/Users').then(function(response){
                return substringMatcher(value, response.data)
            });
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
        .controller('SearchFriendDirectiveController', ['$scope', 'data', searchFriendDirectiveController]);
}());
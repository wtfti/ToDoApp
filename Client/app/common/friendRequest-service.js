(function() {
    'use strict';

    var friendRequestService = function friendRequestService(data, $q) {
        var FRIEND_KEY = 'Friend/';
        var deferred = $q.defer();

        return {
            getFriendRequests: function () {
                data.get(FRIEND_KEY + 'PendingFriendRequests').then(function (response) {
                    return deferred.resolve(response.data)
                }, function (error) {
                    return deferred.reject(error.data);
                });

                return deferred.promise
            },
            addFriendRequest: function () {

            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('friendRequestService', ['data', '$q', friendRequestService]);
}());
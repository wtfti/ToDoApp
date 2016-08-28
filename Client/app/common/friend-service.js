(function() {
    'use strict';

    var friendService = function friendService(data, $q, signalR) {
        var FRIEND_KEY = 'Friend/';

        return {
            getFriendRequests: function () {
                var deferred = $q.defer();

                data.get(FRIEND_KEY + 'PendingFriendRequests').then(function (response) {
                    return deferred.resolve(response.data)
                }, function (error) {
                    return deferred.reject(error.data);
                });

                return deferred.promise
            },
            acceptFriendRequest: function (name) {
                signalR.acceptRequest(name);
            },
            declineFriendRequest: function (name) {
                signalR.declineRequest(name);
            },
            getFriends: function () {
                var deferred = $q.defer();

                data.get(FRIEND_KEY + 'Friends').then(function (response) {
                    return deferred.resolve(response.data);
                }, function (error) {
                    return deferred.reject(error.data);
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('friendService', ['data', '$q', 'signalR', friendService]);
}());
(function() {
    'use strict';

    var signalRService = function signalRService(jQuery, $rootScope, globalConstants, notifier, $q, auth) {
        var proxy = null;
        var deferred = $q.defer();
        var isInit = false;

        var initialize = function () {
            if (!isInit) {
                jQuery.signalR.ajaxDefaults = jQuery.extend(true, {}, $.signalR.ajaxDefaults, {
                    headers: {
                        "Authorization": "Bearer " + auth.getToken()
                    }
                });
                //Getting the connection object
                var connection = jQuery.hubConnection(globalConstants.signalRUrl, {useDefaultPath: false});

                //Creating proxy
                proxy = connection.createHubProxy('friend');


                proxy.on('acceptedRequest', function (name) {
                    notifier.success('You and ' + name + ' are now friends :)');
                });

                proxy.on('newFriendRequest', function (fromFullName) {
                    deferred.resolve();
                });

                proxy.on('declinedRequest', function (name) {
                    notifier.warning(name + ' has declined your friend request. You are not able to send a new request again.');
                });

                //Starting connection
                connection.start();
                isInit = true;
            }
        };

        var sendRequest = function (name) {
            proxy.invoke('friendRequest', name);
            notifier.success('Friend request is sent to ' + name);
        };

        var declineRequest = function (name) {
            proxy.invoke('declineRequest', name);
        };

        var acceptRequest = function (name) {
            proxy.invoke('acceptRequest', name);
        };

        var newRequest = function () {
            return deferred.promise;
        };

        return {
            initialize: initialize,
            sendRequest: sendRequest,
            declineRequest: declineRequest,
            acceptRequest: acceptRequest,
            newRequest: newRequest
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('signalR', ['jQuery', '$rootScope', 'globalConstants', 'notifier', '$q', 'auth', signalRService]);
}());
(function() {
    'use strict';

    var signalRService = function signalRService(jQuery, $rootScope, globalConstants, notifier) {
        var proxy = null;

        var initialize = function () {
            //Getting the connection object
            var connection = jQuery.hubConnection(globalConstants.signalRUrl, {useDefaultPath: false});

            //Creating proxy
            proxy = connection.createHubProxy('friend');


            proxy.on('acceptedRequest', function (name) {
                notifier.success('You and ' + name + ' are now friends :)');
            });

            proxy.on('newFriendRequest', function (fromFullName) {
                console.log('new friend request')
            });

            proxy.on('declinedRequest', function (name) {
                notifier.warning(name + ' has declined your friend request. You are not able to send a new request again.');
            });

            //Starting connection
            connection.start();
        };

        var sendRequest = function (name) {
            //Invoking greetAll method defined in hub
            proxy.invoke('friendRequest', name);
            notifier.success('Friend request is sent to ' + name);
        };

        var declineRequest = function (name) {
            proxy.invoke('declineRequest', name);
        };

        var acceptRequest = function (name) {
            proxy.invoke('acceptRequest', name);
        };

        return {
            initialize: initialize,
            sendRequest: sendRequest,
            declineRequest: declineRequest,
            acceptRequest: acceptRequest
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('signalR', ['jQuery', '$rootScope', 'globalConstants', 'notifier', signalRService]);
}());
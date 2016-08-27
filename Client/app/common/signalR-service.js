(function() {
    'use strict';

    var signalRService = function signalRService(jQuery, $rootScope, globalConstants, notifier) {
        var proxy = null;

        var initialize = function () {
            //Getting the connection object
            var connection = jQuery.hubConnection(globalConstants.signalRUrl, {useDefaultPath: false});

            //Creating proxy
            proxy = connection.createHubProxy('friend');


            proxy.on('acceptedRequest', function () {
                console.log('accepted request')
            });

            proxy.on('newFriendRequest', function (fromFullName) {
                console.log('new friend request')
            });

            //Starting connection
            connection.start();
        };

        var sendRequest = function (name) {
            //Invoking greetAll method defined in hub
            proxy.invoke('friendRequest', name + '123');
            notifier.success('Friend request is sent to ' + name);
        };

        var declineRequest = function (name) {

        };

        var acceptRequest = function (name) {

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
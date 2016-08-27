(function () {
    'use strict';

    var friendRequestDirectiveController = function friendRequestDirectiveController(friendRequestService, notifier) {
        var vm= this;

        friendRequestService.getFriendRequests().then(function (friends) {
            vm.friendRequests = friends;
            vm.friendRequestCount = friends.length;
        }, function (error) {
            notifier.error(error);
        })
    };

    angular
        .module('ToDoApp.controllers')
        .controller('FriendRequestDirectiveController', ['friendRequestService', 'notifier', friendRequestDirectiveController]);
}());
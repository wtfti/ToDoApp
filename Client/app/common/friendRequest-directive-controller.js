(function () {
    'use strict';

    var friendRequestDirectiveController = function friendRequestDirectiveController(friendService, notifier, $timeout) {
        var vm = this;
        var getFriendRequests = function () {
            friendService.getFriendRequests().then(function (friends) {
                vm.friendRequests = friends;
                vm.friendRequestCount = friends.length > 0 ? friends.length : '';
            }, function (error) {
                notifier.error(error);
            });
        };

        this.acceptFriendRequest = function (name) {
            friendService.acceptFriendRequest(name);
            $timeout(getFriendRequests, 2000);
        };

        this.declineFriendRequest = function (name) {
            friendService.declineFriendRequest(name);
            $timeout(getFriendRequests, 2000);
        };

        getFriendRequests();
        friendService.newFriendRequest().then(function () {
            getFriendRequests();
        })
    };

    angular
        .module('ToDoApp.controllers')
        .controller('FriendRequestDirectiveController', ['friendService', 'notifier', '$timeout',
            friendRequestDirectiveController]);
}());
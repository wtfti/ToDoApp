(function () {
    'use strict';

    var friendRequestDirective = function friendRequestDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/common/friendRequest-directive.html',
            controller: 'FriendRequestDirectiveController',
            controllerAs: 'friendRequestDirectiveCtrl'
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('friendRequest', [friendRequestDirective]);
}());
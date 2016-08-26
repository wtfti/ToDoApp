(function () {
    'use strict';

    var searchFriendDirective = function searchFriendDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/common/searchFriend-directive.html',
            controller: 'SearchFriendDirectiveController',
            controllerAs: 'searchFriendDirectiveCtrl'
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('searchFriend', [searchFriendDirective]);
}());
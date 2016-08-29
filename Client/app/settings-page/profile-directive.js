(function () {
    'use strict';

    var profileTabDirective = function profileTabDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/settings-page/profile-view.html',
            controller: 'ProfileTabDirectiveController',
            controllerAs: 'profileTabDirectiveCtrl'
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('profileTab', [profileTabDirective]);
}());
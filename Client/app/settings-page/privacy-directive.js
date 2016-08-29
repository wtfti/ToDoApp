(function () {
    'use strict';

    var privacyTabDirective = function privacyTabDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/settings-page/privacy-view.html',
            controller: 'PrivacyTabDirectiveController',
            controllerAs: 'privacyTabDirectiveCtrl'
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('privacyTab', [privacyTabDirective]);
}());
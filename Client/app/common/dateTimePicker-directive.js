(function () {
    'use strict';

    var dateTimePickerDirective = function dateTimePickerDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/common/dateTimePicker-directive.html',
            controller: 'DateTimePickerDirectiveController',
            controllerAs: 'dateTimepickerCtrl'
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('dateTimePicker', [dateTimePickerDirective]);
}());
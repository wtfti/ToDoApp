(function () {
    'use strict';

    var notesDirective = function notesDirective() {
        return {
            restrict: 'A',
            templateUrl: 'app/dashboard-page/notes-directive.html',
            link: function(scope, element, attrs) {
            }
        };
    };

    angular
        .module('ToDoApp.directives')
        .directive('notes', [notesDirective]);
}());
(function() {
    'use strict';

    var dateTimePickerService = function dateTimePickerService() {
        var date;
        var time;

        return {
            getDate: function () {
                return date;
            },
            setDate: function (value) {
                date = value;
            },
            getTime: function () {
                return time;
            },
            setTime: function (value) {
                time = value;
            },
            getDateTime: function () {
                return date === undefined ? undefined : date + ' ' + time;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('dateTimePickerService', [dateTimePickerService]);
}());
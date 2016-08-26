(function () {
    'use strict';

    var dateTimePickerDirectiveController = function dateTimePickerDirectiveController(dateTimePickerService) {
        var vm = this;
        var now = new Date();
        this.formatDate = 'dd/MM/yyyy';
        var DATEFORMAT = this.formatDate.toUpperCase();
        var TIMEFORMAT = 'HH:mm';
        this.time = new Date(now.getYear(), now.getMonth(), now.getDay(), now.getHours(), now.getMinutes() + 30);

        this.dateOnChange = function () {
            dateTimePickerService.setDate(moment(vm.datepicker).format(DATEFORMAT));
            dateTimePickerService.setTime(moment(vm.time).format(TIMEFORMAT));
        };

        this.timeOnChange = function () {
            dateTimePickerService.setTime(moment(vm.time).format(TIMEFORMAT));
        };

        this.openDatePicker = function() {
            this.dp.opened = true;
        };

        this.dp = {
            opened: false
        };

        this.inlineOptions = {
            minDate: new Date(),
            showWeeks: true
        };
    };

    angular
        .module('ToDoApp.controllers')
        .controller('DateTimePickerDirectiveController', ['dateTimePickerService', dateTimePickerDirectiveController]);
}());
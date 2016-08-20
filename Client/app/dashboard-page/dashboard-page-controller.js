(function () {
    'use strict';

    var dashboardPageController = function dashboardPageController() {
        this.welcomeText = 'veche si v dashboard direktoriqta';
    };

    angular.module('ToDoApp.controllers')
        .controller('DashboardPageController', [dashboardPageController]);
}());
(function () {
    'use strict';

    function config($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'app/home-page/home-page-view.html',
                controller: 'HomePageController',
                controllerAs: 'homeCtrl'
            })
            .when('/dashboard', {
                templateUrl: 'app/dashboard-page/dashboard-page-view.html',
                controller: 'DashboardPageController',
                controllerAs: 'dashboardCtrl'
            })
            .otherwise({redirectTo: '/'});
    }

    angular.module('ToDoApp.services', []);
    angular.module('ToDoApp.data', []);
    angular.module('ToDoApp.controllers', ['ToDoApp.data', 'ToDoApp.services']);

    angular.module('ToDoApp', ['ngRoute', 'ngCookies', 'ngAnimate', 'angular-loading-bar', 'ui.bootstrap', 'ToDoApp.controllers'])
        .config(['$routeProvider', config])
        .value('toastr', toastr)
        .constant('baseUrl', 'http://localhost:33178/api/');
}());
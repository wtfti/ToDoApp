(function () {
    'use strict';

    function config($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'app/home-page/home-page-view.html',
                controller: 'HomePageController',
                controllerAs: 'home'
            })
            .otherwise({ redirectTo: '/' });
    }

    function run(auth) {
        if (auth.isAuthenticated()) {
            auth.getIdentity();
        }
    }

    angular.module('ToDoApp.services', []);
    angular.module('ToDoApp.data', []);
    angular.module('ToDoApp.controllers', ['ToDoApp.data', 'ToDoApp.services']);

    angular.module('ToDoApp', ['ngRoute', 'ngCookies', 'angular-loading-bar', 'ToDoApp.controllers'])
        .config(['$routeProvider', config])
        .run(['auth', run])
        .value('toastr', toastr)
        .constant('baseUrl', 'http://localhost:33178/api');
}());
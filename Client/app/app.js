(function () {
    'use strict';

    function config($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: 'app/home-page/home-page-view.html',
                controller: 'HomePageController',
                controllerAs: 'homeCtrl',
                title: 'ToDoApp - Make your thing in time',
                css: 'content/home.css'
            })
            .when('/dashboard', {
                templateUrl: 'app/dashboard-page/dashboard-page-view.html',
                controller: 'DashboardPageController',
                controllerAs: 'dashboardCtrl',
                title: 'Dashboard',
                css: ['content/dashboard.css', 'content/typeaheadjs.css', 'content/awesome-bootstrap-checkbox.css']
            })
            .otherwise({redirectTo: '/'});
    }

    var run = function ($rootScope, auth, notifier) {
        $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
            $rootScope.title = current.$$route.title;
        });

        if (auth.isAuthenticated()) {
            auth.getIdentity().then(function (identity) {
                //notifier.success('Welcome back, ' + identity.fullName + '!');
            });
        }
    };

    angular.module('ToDoApp.services', []);
    angular.module('ToDoApp.directives', []);
    angular.module('ToDoApp.data', []);
    angular.module('ToDoApp.controllers', ['ToDoApp.data', 'ToDoApp.services']);

    angular.module('ToDoApp', ['ngRoute', 'ngCookies', 'ngAnimate', 'ngSanitize', 'angular-loading-bar', 'ui.bootstrap',
        'angularMoment', 'ToDoApp.controllers', 'angularCSS', 'ToDoApp.directives'])
        .run(['$rootScope', 'auth', 'notifier', run])
        .config(['$routeProvider', config])
        .value('toastr', toastr)
        .constant('baseUrl', 'http://localhost:33178/api/');
}());
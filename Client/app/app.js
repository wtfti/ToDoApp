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
            .when('/settings', {
                templateUrl: 'app/settings-page/settings-page-view.html',
                controller: 'SettingsPageController',
                controllerAs: 'settingsCtrl',
                title: 'Settings',
                css: ['content/settings.css', 'content/typeaheadjs.css', 'content/awesome-bootstrap-checkbox.css',
                    'content/colorpicker.min.css']
            })
            .otherwise({redirectTo: '/'});
    }

    var run = function ($rootScope, auth, notifier, $location) {
        $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
            $rootScope.title = current.$$route.title;
        });

        if (auth.isAuthenticated()) {
            auth.getIdentity().then(function (identity) {
                notifier.success('Welcome back, ' + identity.fullName + '!');
                //$location.path('/dashboard')
            });
        }
        else {
            notifier.error('You are not logged in!');
            $location.path('/');
        }
    };

    angular.module('ToDoApp.services', []);
    angular.module('ToDoApp.directives', []);
    angular.module('ToDoApp.data', []);
    angular.module('ToDoApp.controllers', ['ToDoApp.data', 'ToDoApp.services']);

    angular.module('ToDoApp', ['ngRoute', 'ngCookies', 'ngAnimate', 'ngSanitize', 'angular-loading-bar', 'ui.bootstrap',
        'angularMoment', 'ToDoApp.controllers', 'angularCSS', 'colorpicker.module', 'ToDoApp.directives'])
        .run(['$rootScope', 'auth', 'notifier', '$location', run])
        .config(['$routeProvider', config])
        .value('jQuery', jQuery)
        .value('toastr', toastr)
        .constant('globalConstants', {
            'baseUrl': 'http://localhost:33178/api/',
            'signalRUrl': 'http://localhost:33178/signalr'
        });
}());
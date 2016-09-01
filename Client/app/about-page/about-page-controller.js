(function () {
    'use strict';

    var aboutPageController = function aboutPageController($scope, $uibModal, auth, $location) {
        var vm = this;

        this.isLogged = function () {
            if (auth.isAuthenticated()) {
                return true;
            }
            else {
                return false;
            }
        };

        $scope.openLoginModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/home-page/home-login-view.html',
                controller: 'LoginInstanceController',
                controllerAs: 'loginInstanceCtrl'
            });
        };

        $scope.openRegisterModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/home-page/home-register-view.html',
                controller: 'RegisterInstanceController',
                controllerAs: 'registerInstanceCtrl'
            });
        };

        this.redirectToHome = function () {
            if (auth.isAuthenticated()) {
                $location.path('/dashboard');
            }
            else {
                $location.path('/');
            }
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('AboutPageController', ['$scope', '$uibModal', 'auth', '$location', aboutPageController]);
}());
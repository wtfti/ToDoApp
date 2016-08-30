(function () {
    'use strict';

    var aboutPageController = function aboutPageController($scope, $uibModal) {
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
        }
    };

    angular.module('ToDoApp.controllers')
        .controller('AboutPageController', ['$scope', '$uibModal', aboutPageController]);
}());
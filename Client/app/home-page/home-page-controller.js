(function () {
    'use strict';

    var homePageController = function homePageController($scope, $uibModal) {
        $scope.openLoginModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/home-page/home-login-view.html',
                controller: 'LoginInstanceController'
            });
        };

        $scope.openRegisterModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/home-page/home-register-view.html',
                controller: 'RegisterInstanceController'
            });
        }
    };

    angular.module('ToDoApp.controllers')
        .controller('HomePageController', ['$scope', '$uibModal', homePageController]);
}());
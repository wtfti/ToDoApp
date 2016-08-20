(function () {
    'use strict';

    var loginInstanceController = function loginInstanceController($scope, $uibModalInstance, $location, auth, notifier) {
        $scope.login = function () {
            var mail = $scope.mail;
            var password = $scope.password;
            var rememberMe = $scope.rememberCheckbox;

            if (mail && password) {
                var user = {
                    username: $scope.mail,
                    password: $scope.password
                };

                auth.login(user, rememberMe).then(function () {
                    $uibModalInstance.close('close');
                    $uibModalInstance.closed.then(function () {
                        $location.path('/dashboard')
                    })
                }, function () {
                    notifier.error('Invalid login data')
                })
            }
            else {
                notifier.error('Invalid login data')
            }
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('LoginInstanceController', ['$scope', '$uibModalInstance', '$location', 'auth', 'notifier', loginInstanceController]);
}());
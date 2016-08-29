(function () {
    'use strict';

    var loginInstanceController = function loginInstanceController($scope, $uibModalInstance, $location, auth, notifier) {
        var vm = this;

        this.login = function () {
            var mail = vm.mail;
            var password = vm.password;
            var rememberMe = vm.rememberCheckbox;

            if (mail && password) {
                var user = {
                    username: vm.mail,
                    password: vm.password
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

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('LoginInstanceController', ['$scope', '$uibModalInstance', '$location', 'auth', 'notifier', loginInstanceController]);
}());
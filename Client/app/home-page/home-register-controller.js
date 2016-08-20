(function () {
    'use strict';

    var registerInstanceController = function registerInstanceController($scope, $uibModalInstance, auth, notifier) {
        $scope.register = function () {
            var mail = $scope.mail;
            var fullName = $scope.fullName;
            var password = $scope.password;
            var confirmPassword = $scope.confirmPassword;

            if (password != confirmPassword) {
                notifier.error('Password and confirm password are not the same')
            }
            else if(mail && fullName) {
                var user = {
                    Email: mail,
                    FullName: fullName,
                    Password: password,
                    ConfirmPassword: confirmPassword
                };

                auth.register(user).then(function (response) {
                    notifier.success(response);
                    $uibModalInstance.close('close');
                }, function (err) {
                    notifier.error(err)
                })
            }
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('RegisterInstanceController', ['$scope', '$uibModalInstance', 'auth', 'notifier', registerInstanceController]);
}());
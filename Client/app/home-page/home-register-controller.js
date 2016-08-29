(function () {
    'use strict';

    var registerInstanceController = function registerInstanceController($uibModalInstance, auth, notifier) {
        var vm = this;

        this.register = function () {
            var mail = vm.mail;
            var fullName = vm.fullName;
            var password = vm.password;
            var confirmPassword = vm.confirmPassword;

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

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('RegisterInstanceController', ['$uibModalInstance', 'auth', 'notifier', registerInstanceController]);
}());
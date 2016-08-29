(function () {
    'use strict';

    var privacyTabDirectiveController = function privacyTabDirectiveController(data, notifier) {
        var vm = this;

        this.saveChanges = function () {
            var currentPass = vm.password;
            var newPass = vm.newPassword;
            var confirmPass = vm.confirmPassword;

            if (newPass == confirmPass) {
                data.put('Account/ChangePassword?' +'currentPassword=' + currentPass + '&newPassword=' + newPass
                + '&confirmNewPassword=' + confirmPass).then(function (response) {
                    vm.password = '';
                    vm.newPassword = '';
                    vm.confirmPassword = '';
                    
                    notifier.success(response.data);
                }, function (error) {
                    notifier.error(error.data);
                })
            }
            else {
                notifier.error('New password and confirm password are not the same!')
            }
        }
    };

    angular
        .module('ToDoApp.controllers')
        .controller('PrivacyTabDirectiveController', ['data', 'notifier', privacyTabDirectiveController]);
}());
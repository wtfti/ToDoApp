(function () {
    'use strict';

    var profileTabDirectiveController = function profileTabDirectiveController(background, data, notifier, $location) {
        var vm = this;
        this.backgroundDropdown = 'Background color';

        background.getBackground().then(function (background) {
            if(background.lenght > 10) {
                vm.currentBackground = background;
            }
            else {
                vm.colorPicker = background;
            }
        });

        data.get('Account/Details').then(function (response) {
            var details = response.data;
            vm.age = details.Age;
            vm.fullName = details.FullName;
            vm.gender = details.Gender;
            vm.email = details.Email;
        });

        this.sendChanges = function () {
            var image;

            if (vm.backgroundDropdown == 'Background color') {
                image = vm.colorPicker;
            }
            else {
                // convert image to base64
            }

            var details = {
                FullName: vm.fullName,
                Age: vm.age,
                Gender: vm.gender,
                Image: image
            };

            data.put('Account/Edit', details).then(function (response) {
                notifier.success(response.data);
                $location.path('/settings');
            }, function (error) {
                notifier.error(error.data);
            })
        }
    };

    angular
        .module('ToDoApp.controllers')
        .controller('ProfileTabDirectiveController', ['background', 'data', 'notifier', '$location', profileTabDirectiveController]);
}());
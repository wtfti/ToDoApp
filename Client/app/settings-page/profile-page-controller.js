(function () {
    'use strict';

    var profileTabDirectiveController = function profileTabDirectiveController(background, data, notifier, $scope) {
        var vm = this;
        this.backgroundDropdown = 'Background color';

        background.getBackground().then(function (back) {
            if (back.indexOf('base64') > 0) {
                vm.currentBackground = back;
            }
            else {
                vm.colorPicker = back;
            }
        });

        data.get('Account/Details').then(function (response) {
            var details = response.data;
            vm.age = details.Age;
            vm.fullName = details.FullName;
            vm.gender = details.Gender;
            vm.email = details.Email;
        });

        $scope.file_changed = function (element) {
            var photofile = element.files[0];
            var reader = new FileReader();

            reader.onload = function (e) {
                $scope.$apply(function () {
                    $scope.prev_img = e.target.result;
                    vm.imagePreview = $scope.prev_img;
                });
            };

            if (photofile)
            reader.readAsDataURL(photofile);
        };

        this.sendChanges = function () {
            var image;
            var hasError = false;

            if (vm.backgroundDropdown == 'Background color') {
                var isOk = /^#[0-9A-F]{6}$/i.test(vm.colorPicker);
                if (vm.colorPicker) {
                    if (isOk) {
                        image = vm.colorPicker;
                    }
                    else {
                        notifier.error('Color value is not valid!');
                        hasError = true;
                    }
                }
            }
            else {
                if ($scope.prev_img) {
                    image = $scope.prev_img;
                }
            }

            var details = {
                FullName: vm.fullName,
                Age: vm.age,
                Gender: vm.gender,
                Image: image
            };

            if (!hasError) {
                data.put('Account/Edit', details).then(function (response) {
                    notifier.success(response.data);

                    if (image) {
                        background.setBackground(image);
                        background.loadBackground();
                    }
                }, function (error) {
                    notifier.error(error.data);
                })
            }
        }
    };

    angular
        .module('ToDoApp.controllers')
        .controller('ProfileTabDirectiveController', ['background', 'data', 'notifier', '$scope', profileTabDirectiveController]);
}());
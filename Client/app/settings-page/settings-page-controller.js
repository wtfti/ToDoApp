(function () {
    'use strict';

    var settingsPageController = function settingsPageController($scope, background, notifier, identity) {
        var vm = this;
        this.currentPage = 1;
        this.activeTab = {
            active: 0
        };

        background.getBackground().then(function (backgroundBase64Image) {
            if (backgroundBase64Image.lenght > 10) {
                $scope.backgroundImage = 'url(' + backgroundBase64Image + ')';
            }
            else {
                $scope.backgroundColor = backgroundBase64Image;
            }
        }, function (error) {
            notifier.error(error);
        });

        identity.getUser().then(function (user) {
            vm.fullName = user.fullName;
        });

        this.changeTab = function (id) {
            switch (id) {
                case 0: // Profile
                    vm.activeTab.active = 0;
                    break;
                case 1: // Privacy
                    vm.activeTab.active = 1;
                    break;
                default:
                    // TODO add error
                    break;
            }
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('SettingsPageController', ['$scope', 'background', 'notifier', 'identity', settingsPageController]);
}());
(function () {
    'use strict';

    var settingsPageController = function settingsPageController($scope, background, notifier, identity) {
        var vm = this;

        background.getBackground().then(function (backgroundBase64Image) {
            $scope.backgroundImage = 'url(' + backgroundBase64Image + ')';
        }, function (error) {
            notifier.error(error);
        });

        identity.getUser().then(function (user) {
            vm.fullName = user.fullName;
        });
    };

    angular.module('ToDoApp.controllers')
        .controller('SettingsPageController', ['$scope', 'background', 'notifier', 'identity', settingsPageController]);
}());
(function () {
    'use strict';

    var settingsPageController = function settingsPageController($rootScope, background, notifier, identity,
                                                                 auth, $location, signalR) {
        var vm = this;
        this.currentPage = 1;
        this.activeTab = {
            active: 0
        };

        this.redirectToHome = function () {
            if (auth.isAuthenticated()) {
                $location.path('/dashboard');
            }
            else {
                $location.path('/');
            }
        };

        background.loadBackgroundFromCache().then(null, function (error) {
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

        this.logout = function () {
            auth.logout();
            signalR.stop();
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('SettingsPageController', ['$rootScope', 'background', 'notifier', 'identity', 'auth',
            '$location', 'signalR', settingsPageController]);
}());
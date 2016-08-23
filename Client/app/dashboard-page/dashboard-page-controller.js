(function () {
    'use strict';

    var dashboardPageController = function dashboardPageController(background, $scope, identity, notesService) {
        var vm = this;
        var base64Image = background.getBackground();
        $scope.backgroundImage = 'url(' + base64Image + ')';

        notesService.getNotes().then(function (data) {
            $scope.notesData = data;
        });

        identity.getUser().then(function (user) {
            vm.fullName = user.fullName;
        })
    };

    angular.module('ToDoApp.controllers')
        .controller('DashboardPageController', ['background', '$scope', 'identity', 'notesService', dashboardPageController]);
}());
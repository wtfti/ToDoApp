(function () {
    'use strict';

    var dashboardPageController = function dashboardPageController(background, $scope, identity, notesService, $uibModal) {
        var vm = this;
        var base64Image = background.getBackground();
        $scope.backgroundImage = 'url(' + base64Image + ')';

        this.openAddNoteModal = function () {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/dashboard-page/dashboard-new-note-view.html',
                controller: 'NewNoteModalInstanceController',
                controllerAs: 'newNoteModalInstanceCtrl'
            });
        };

        this.openEditNoteModal = function (note) {
            $uibModal.open({
                animation: true,
                templateUrl: 'app/dashboard-page/dashboard-edit-note-view.html',
                controller: 'EditNoteModalInstanceController',
                controllerAs: 'editNoteModalInstanceCtrl',
                resolve: {
                    noteDetails: function () {
                        return note;
                    }
                }
            });
        };

        this.changeTab = function (id) {
            switch (id) {
                case 0: // All notes
                    notesService.getNotes().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 1: // Today
                    notesService.getNotesFromToday().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 2: //Assigned to me
                    notesService.getSharedNotes().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 3: // Expired
                    notesService.getNotesWithExpirationDate().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 4: // Finished
                    notesService.getCompletedNotes().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                default:
                    // TODO add error
                    break;
            }
        };

        identity.getUser().then(function (user) {
            vm.fullName = user.fullName;
        });

        this.changeTab(0);
    };

    angular.module('ToDoApp.controllers')
        .controller('DashboardPageController', ['background', '$scope', 'identity', 'notesService', '$uibModal',  dashboardPageController]);
}());
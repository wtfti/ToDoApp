(function () {
    'use strict';

    var dashboardPageController = function dashboardPageController(background, $scope, identity, notesService, $uibModal) {
        var vm = this;
        var sort = 0;
        var base64Image = background.getBackground();

        this.sortByText = 'Title Asc';
        $scope.backgroundImage = 'url(' + base64Image + ')';

        this.openAddNoteModal = function () {
            var instance = $uibModal.open({
                animation: true,
                templateUrl: 'app/dashboard-page/dashboard-new-note-view.html',
                controller: 'NewNoteModalInstanceController',
                controllerAs: 'newNoteModalInstanceCtrl'
            });

            instance.result.then(function () {
                vm.changeTab(vm.activeTab.active);
            })
        };

        this.activeTab = {
            active: 1
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

        this.sortBy = function (index) {
            sort = index;
            switch (sort) {
                case 0: // Title Asc
                    vm.sortByText = 'Title Asc';
                    $scope.notesData.sort(function (a, b) {
                        return a['Content'].localeCompare(b['Content']);
                    });
                    break;
                case 1: // Title Desc
                    vm.sortByText = 'Title Desc';
                    $scope.notesData.sort(function (a, b) {
                        if (a['Content'] > b['Content']) {
                            return -1;
                        }
                        else if (a['Content'] < b['Content']) {
                            return 1;
                        }
                        else {
                            return 0;
                        }
                    });
                    break;
                case 2: // Date Asc
                    vm.sortByText = 'Date Asc';
                    $scope.notesData.sort(function (a, b) {
                        if (a['CreatedOn'] > b['CreatedOn']) {
                            return 1;
                        }
                        else if (a['CreatedOn'] < b['CreatedOn']) {
                            return -1;
                        }
                        else {
                            return 0;
                        }
                    });
                    break;
                case 3: // Date Desc
                    vm.sortByText = 'Date Desc';
                    $scope.notesData.sort(function (a, b) {
                        if (a['CreatedOn'] > b['CreatedOn']) {
                            return -1;
                        }
                        else if (a['CreatedOn'] < b['CreatedOn']) {
                            return 1;
                        }
                        else {
                            return 0;
                        }
                    });
                    break;
            }
        };

        this.changeTab = function (id) {
            switch (id) {
                case 0: // All notes
                    vm.activeTab.active = 0;
                    notesService.getNotes().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 1: // Today
                    vm.activeTab.active = 1;
                    notesService.getNotesFromToday().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 2: //Assigned to me
                    vm.activeTab.active = 2;
                    notesService.getSharedNotes().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 3: // Expired
                    vm.activeTab.active = 3;
                    notesService.getNotesWithExpirationDate().then(function (data) {
                        $scope.notesData = data;
                    });
                    break;
                case 4: // Finished
                    vm.activeTab.active = 4;
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
(function () {
    'use strict';

    var editNoteModalInstanceController = function editNoteModalInstanceController($uibModalInstance, auth, notifier, noteDetails) {
        this.createdFrom = noteDetails.createdFrom;
        this.title = noteDetails.title;
        this.content = noteDetails.content;
        this.created = noteDetails.created;
        this.expired = noteDetails.expired;
        this.sharedWith = noteDetails.sharedWith;

        this.sendChanges = function () {

        };

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('EditNoteModalInstanceController', ['$uibModalInstance', 'auth', 'notifier', 'noteDetails', editNoteModalInstanceController]);
}());
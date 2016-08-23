(function () {
    'use strict';

    var newNoteMOdalInstanceController = function newNoteMOdalInstanceController($uibModalInstance, auth, notifier) {
        this.addNote = function () {

        };

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('NewNoteModalInstanceController', ['$uibModalInstance', 'auth', 'notifier', newNoteMOdalInstanceController]);
}());
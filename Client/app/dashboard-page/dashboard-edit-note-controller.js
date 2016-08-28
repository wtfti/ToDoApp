(function () {
    'use strict';

    var editNoteModalInstanceController = function editNoteModalInstanceController(
        $uibModalInstance,
        auth,
        notifier,
        noteDetails,
        dateTimePickerService,
        moment,
        notesService) {
        var vm = this;
        var DATETIMEFORMAT = 'DD/MM/YYYY HH:mm';
        this.title = noteDetails.title;
        this.createdFrom = noteDetails.createdFrom;
        this.content = noteDetails.content;
        this.created = moment(noteDetails.created).format(DATETIMEFORMAT);
        this.expired = noteDetails.expired ? moment(noteDetails.expired).format(DATETIMEFORMAT) : undefined;
        this.sharedWith = noteDetails.sharedWith;

        this.sendChanges = function () {
            var note = {
                Id: noteDetails.id,
                Title: vm.title,
                Content: vm.content,
                ExpiredOn: vm.expired === undefined ? '' : vm.expired,
                SharedWith: noteDetails.sharedWith
            };

            notesService.editNote(note).then(function (response) {
                notifier.success(response);
                $uibModalInstance.close('close');
            }, function (error) {
                notifier.error(error);
            });
        };

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('EditNoteModalInstanceController', ['$uibModalInstance', 'auth', 'notifier', 'noteDetails',
            'dateTimePickerService', 'moment', 'notesService', editNoteModalInstanceController]);
}());
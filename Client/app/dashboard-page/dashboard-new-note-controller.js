(function () {
    'use strict';

    var newNoteMOdalInstanceController = function newNoteMOdalInstanceController($uibModalInstance, notesService, notifier) {
        this.addNote = function () {
            var title = this.title;
            var content = this.content;
            var expired = this.expired;

            if (title.length < 3 || title.length > 30) {
                notifier.error('Title is too short or too long. [3, 30] chars long')
            }

            if (content < 1 || content > 100) {
                notifier.error('Content is too short or too long. [1, 100] chars long')
            }

            var note = {
                Title: title,
                Content: content,
                ExpiredOn: angular.isUndefined(expired) ? "" : expired,
                SharedWith: []
            };

            notesService.addNote(note).then(function (response) {
                $uibModalInstance.close('close');
                notifier.success(response);
            }, function (error) {
                notifier.error(error);
            })
        };

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('NewNoteModalInstanceController', ['$uibModalInstance', 'notesService', 'notifier', newNoteMOdalInstanceController]);
}());
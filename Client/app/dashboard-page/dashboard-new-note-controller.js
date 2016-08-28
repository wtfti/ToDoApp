(function () {
    'use strict';

    var newNoteMOdalInstanceController = function newNoteMOdalInstanceController(
        $uibModalInstance,
        notesService,
        notifier,
        dateTimePickerService,
        friendService) {
        var vm = this;
        var selectedFriends = [];
        this.selectedUsers = selectedFriends;

        this.addNote = function () {
            var title = this.title;
            var content = this.content;
            var expiredDate = dateTimePickerService.getDateTime();

            if (!title || (title.length < 3 || title.length > 30)) {
                notifier.error('Title is too short or too long. [3, 30] chars long')
            }

            if (!content || (content < 1 || content > 100)) {
                notifier.error('Content is too short or too long. [1, 100] chars long')
            }

            var note = {
                Title: title,
                Content: content,
                ExpiredOn: expiredDate ? expiredDate : '',
                SharedWith: selectedFriends
            };

            notesService.addNote(note).then(function (response) {
                $uibModalInstance.close('close');
                notifier.success(response);
            }, function (error) {
                notifier.error(error.Message);
            })
        };

        friendService.getFriends().then(function (response) {
            vm.friends = response;
        }, function () {
            notifier.error('Cannot load friends :(')
        });

        this.addFriend = function (name) {
            selectedFriends.push(name);
            var index = vm.friends.indexOf(name);
            vm.friends.splice(index, 1);
        };

        this.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    angular.module('ToDoApp.controllers')
        .controller('NewNoteModalInstanceController', ['$uibModalInstance', 'notesService', 'notifier',
            'dateTimePickerService', 'friendService', newNoteMOdalInstanceController]);
}());
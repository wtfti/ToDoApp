(function() {
    'use strict';

    var notesService = function notesService(data, $q) {
        var NOTE_PREFIX = 'Note';

        return {
            getNotes: function () {
                var deferred = $q.defer();

                data.get(NOTE_PREFIX + '/GetNotes').then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            addNewNote: function (note) {
                var deferred = $q.defer();

                data.post(NOTE_PREFIX + 'AddNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            },
            editNote: function (note) {
                var deferred = $q.defer();

                data.update(NOTE_PREFIX + 'AddNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            },
            removeNote: function (id) {
                var deferred = $q.defer();

                data.delete(NOTE_PREFIX + 'AddNote', id).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('notesService', ['data', '$q', notesService]);
}());
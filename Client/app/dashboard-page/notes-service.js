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
            getNotesFromToday: function () {
                var deferred = $q.defer();

                data.get(NOTE_PREFIX + '/GetNotesFromToday').then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSharedNotes: function () {
                var deferred = $q.defer();

                data.get(NOTE_PREFIX + '/GetSharedNotes').then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getNotesWithExpirationDate: function () {
                var deferred = $q.defer();

                data.get(NOTE_PREFIX + '/GetNotesWithExpirationDate').then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getCompletedNotes: function () {
                var deferred = $q.defer();

                data.get(NOTE_PREFIX + '/GetCompletedNotes').then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            addNote: function (note) {
                var deferred = $q.defer();

                data.post(NOTE_PREFIX + '/AddNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            },
            editNote: function (note) {
                var deferred = $q.defer();

                data.put(NOTE_PREFIX + '/ChangeNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            },
            removeNote: function (id) {
                var deferred = $q.defer();

                data.delete(NOTE_PREFIX + '/RemoveNoteById', id).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data);
                });

                return deferred.promise;
            },
            setComplete: function (id) {
                var deferred = $q.defer();

                data.put(NOTE_PREFIX + '/SetComplete', id).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data)
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('notesService', ['data', '$q', notesService]);
}());
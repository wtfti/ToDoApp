(function() {
    'use strict';

    var notesService = function notesService(data, $q) {
        var NOTE_PREFIX = 'Note';

        return {
            getNotes: function (page) {
                var deferred = $q.defer();
                data.get(NOTE_PREFIX + '/GetNotes?page=' + page).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getNotesFromToday: function (page) {
                var deferred = $q.defer();
                data.get(NOTE_PREFIX + '/GetNotesFromToday?page=' + page).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getSharedNotes: function (page) {
                var deferred = $q.defer();
                data.get(NOTE_PREFIX + '/GetSharedNotes?page=' + page).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getNotesWithExpirationDate: function (page) {
                var deferred = $q.defer();
                data.get(NOTE_PREFIX + '/GetNotesWithExpirationDate?page=' + page).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            getCompletedNotes: function (page) {
                var deferred = $q.defer();
                data.get(NOTE_PREFIX + '/GetCompletedNotes?page=' + page).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            },
            addNote: function (note) {
                var deferred = $q.defer();
                data.post(NOTE_PREFIX + '/AddNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data.Message);
                });

                return deferred.promise;
            },
            editNote: function (note) {
                var deferred = $q.defer();
                data.put(NOTE_PREFIX + '/ChangeNote', note).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data.Message);
                });

                return deferred.promise;
            },
            removeNote: function (id) {
                var deferred = $q.defer();
                data.delete(NOTE_PREFIX + '/RemoveNoteById/' + id).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data.Message);
                });

                return deferred.promise;
            },
            setComplete: function (id) {
                var deferred = $q.defer();
                data.put(NOTE_PREFIX + '/SetComplete/' + id).then(function (response) {
                    deferred.resolve(response.data);
                }, function (response) {
                    deferred.reject(response.data.Message)
                });

                return deferred.promise;
            },
            getNotesCount: function (page) {
                var deferred = $q.defer();
                switch (page) {
                    case 0:
                        data.get(NOTE_PREFIX + '/GetNotesCount').then(function (response) {
                            deferred.resolve(response.data);
                        });
                        break;
                    case 1:
                        data.get(NOTE_PREFIX + '/GetNotesFromTodayCount').then(function (response) {
                            deferred.resolve(response.data);
                        });
                        break;
                    case 2:
                        data.get(NOTE_PREFIX + '/GetSharedNotesCount').then(function (response) {
                            deferred.resolve(response.data);
                        });
                        break;
                    case 3:
                        data.get(NOTE_PREFIX + '/GetNotesWithExparationDateCount').then(function (response) {
                            deferred.resolve(response.data);
                        });
                        break;
                    case 4:
                        data.get(NOTE_PREFIX + '/GetCompletedNotesCount').then(function (response) {
                            deferred.resolve(response.data);
                        });
                        break;
                    default:
                        deferred.reject();
                        break;
                }

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('notesService', ['data', '$q', notesService]);
}());
(function() {
    'use strict';

    var backgroundService = function backgroundService(data, $q) {
        var BACKGROUND_KEY = 'background';
        var deferred = $q.defer();

        return {
            getBackground: function () {
                var background = sessionStorage.getItem(BACKGROUND_KEY);

                if (background) {
                    deferred.resolve(background);
                }

                data.get('Account/Background').then(function (response) {
                    var base64Image = response.data;
                    sessionStorage.setItem(BACKGROUND_KEY, base64Image);
                    deferred.resolve(base64Image);
                }, function () {
                    deferred.reject('Failed to get background');
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('background', ['data', '$q', backgroundService]);
}());
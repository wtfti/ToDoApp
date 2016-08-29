(function() {
    'use strict';

    var backgroundService = function backgroundService(data, $q, $rootScope) {
        var BACKGROUND_KEY = 'background';

        return {
            getBackground: function () {
                var deferred = $q.defer();
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
            },
            setBackground: function (value) {
                sessionStorage.setItem(BACKGROUND_KEY, value);
            },
            loadBackground: function () {
                var deferred = $q.defer();

                this.getBackground().then(function (backgroundBase64Image) {
                    if (backgroundBase64Image.indexOf('base64') > 0) {
                        $rootScope.backgroundImage = 'url(' + backgroundBase64Image + ')';
                        deferred.resolve();
                    }
                    else {
                        $rootScope.backgroundColor = backgroundBase64Image;
                        deferred.resolve();
                    }
                }, function () {
                    deferred.reject('Failed to load background');
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('background', ['data', '$q', '$rootScope', backgroundService]);
}());
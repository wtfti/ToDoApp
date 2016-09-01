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
                        $rootScope.backgroundColor = '';
                        deferred.resolve();
                    }
                    else {
                        $rootScope.backgroundImage = '';
                        $rootScope.backgroundColor = backgroundBase64Image;
                        deferred.resolve();
                    }
                }, function () {
                    deferred.reject('Failed to load background');
                });

                return deferred.promise;
            },
            loadBackgroundFromCache: function () {
                var deferred = $q.defer();
                var background;

                this.getBackground().then(function (backgroundSrc) {
                    background = backgroundSrc;

                    if (background) {
                        if (background.indexOf('base64') > 0) {
                            $rootScope.backgroundImage = 'url(' + background + ')';
                            deferred.resolve();
                        }
                        else {
                            $rootScope.backgroundColor = background;
                            deferred.resolve();
                        }
                    }
                    else {
                        deferred.reject()
                    }
                }, function () {
                    deferred.reject();
                });

                return deferred.promise;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('background', ['data', '$q', '$rootScope', backgroundService]);
}());
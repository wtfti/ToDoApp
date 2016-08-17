(function () {
    'use strict';

    var baseData = function baseData($http, $q, baseUrl, notifier, identity) {
        var headers = {
                'Content-Type': 'application/json'
            },
            authorizationErrorMessage = 'You must be logged in to do that';

        function get(url, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = baseUrl.serverPath + url;

                $http.get(URL)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        function getOData(url, authorize) {
            var deferred = $q.defer();
            var URL = baseUrl.odataServerPath + url;

            $http.get(URL)
                .success(function (data) {
                    deferred.resolve(data);
                })
                .error(function (err) {
                    deferred.reject(err);
                });

            return deferred.promise;
        }

        function post(url, data, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = baseUrl.serverPath + url;

                $http.post(URL, data, headers)
                    .success(function (data) {
                        deferred.resolve(data);
                    })
                    .error(function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        return {
            get: get,
            getOData: getOData,
            post: post
        };
    };

    angular
        .module('ToDoApp.data')
        .factory('data', ['$http', '$q', 'baseUrl', 'notifier', 'identity', baseData]);
}());
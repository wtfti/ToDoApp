(function () {
    'use strict';

    var baseData = function baseData($http, $q, globalConstants, notifier, identity) {
        var headers = {
                'Content-Type': 'application/json'
            };

        function get(url, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = globalConstants.baseUrl + url;

                $http.get(URL)
                    .then(function (data) {
                        deferred.resolve(data);
                    }, function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        function post(url, data, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = globalConstants.baseUrl + url;

                $http.post(URL, data, headers)
                    .then(function (data) {
                        deferred.resolve(data);
                    }, function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        function del(url, data, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = globalConstants.baseUrl + url;

                $http.delete(URL, data, headers)
                    .then(function (data) {
                        deferred.resolve(data);
                    }, function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        function put(url, data, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = globalConstants.baseUrl + url;

                $http.put(URL, data, headers)
                    .then(function (data) {
                        deferred.resolve(data);
                    }, function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        return {
            get: get,
            post: post,
            delete: del,
            put: put
        };
    };

    angular
        .module('ToDoApp.data')
        .factory('data', ['$http', '$q', 'globalConstants', 'notifier', 'identity', baseData]);
}());
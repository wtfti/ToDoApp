(function () {
    'use strict';

    var baseData = function baseData($http, $q, baseUrl, notifier, identity) {
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
                var URL = baseUrl + url;

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
                var URL = baseUrl + url;

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
                var URL = baseUrl + url;

                $http.delete(URL, data, headers)
                    .then(function (data) {
                        deferred.resolve(data);
                    }, function (err) {
                        deferred.reject(err);
                    });
            }

            return deferred.promise;
        }

        function update(url, data, authorize) {
            var deferred = $q.defer();

            if (authorize && !identity.isAuthenticated()) {
                notifier.error(authorizationErrorMessage);
                deferred.reject();
            }
            else {
                var URL = baseUrl + url;

                $http.update(URL, data, headers)
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
            update: update
        };
    };

    angular
        .module('ToDoApp.data')
        .factory('data', ['$http', '$q', 'baseUrl', 'notifier', 'identity', baseData]);
}());
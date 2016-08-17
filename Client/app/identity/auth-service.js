(function () {
    'use strict';

    var authService = function authService($http, $q, $cookies, identity, baseUrl) {
        var TOKEN_KEY = 'authentication';

        var register = function register(user) {
            var defered = $q.defer();

            $http
                .post(baseUrl + '/api/Account/Register', user)
                .then(function () {
                    defered.resolve(true);
                }, function (err) {
                    defered.reject(err);
                });

            return defered.promise;
        };

        var login = function login(user) {
            var defered = $q.defer();

            var data = "grant_type=password&username=" + (user.username || '') + '&password=' + (user.password || '');

            $http.post(baseUrl + '/api/account/login', data, {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
                .success(function (response) {
                    var tokenValue = response.access_token;

                    var expire = new Date();
                    expire.setHours(14 * 24);

                    $cookies.put(TOKEN_KEY, tokenValue, {expires: expire});

                    $http.defaults.headers.common.Authorization = 'Bearer ' + tokenValue;

                    getIdentity().then(function () {
                        defered.resolve(response);
                    });
                })
                .error(function (err) {
                    defered.reject(err);
                });

            return defered.promise;
        };

        var getIdentity = function () {
            var deferred = $q.defer();

            $http.get('/api/users/identity')
                .success(function (identityResponse) {
                    identity.setUser(identityResponse);
                    deferred.resolve(identityResponse);
                });

            return deferred.promise;
        };

        return {
            register: register,
            login: login,
            getIdentity: getIdentity,
            isAuthenticated: function () {
                return !!$cookies.get(TOKEN_KEY);
            },
            logout: function () {
                $cookies.remove(TOKEN_KEY);
                $http.defaults.headers.common.Authorization = null;
                identity.removeUser();
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('auth', ['$http', '$q', '$cookies', 'identity', 'baseUrl', authService]);
}());
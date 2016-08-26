(function () {
    'use strict';

    var authService = function authService($http, $q, $cookies, identity, baseUrl) {
        var TOKEN_KEY = 'authentication';

        var register = function register(user) {
            var defered = $q.defer();

            $http
                .post(baseUrl + 'Account/Register', user)
                .then(function (response) {
                    defered.resolve(response.data);
                }, function (err) {
                    defered.reject(err.data.Message);
                });

            return defered.promise;
        };

        var login = function login(user, rememberMe) {
            var defered = $q.defer();

            var data = "grant_type=password&username=" + (user.username || '') + '&password=' + (user.password || '');

            $http.post(baseUrl + 'Account/login', data, {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
                .then(function (response) {
                    var responseData = response.data;
                    var tokenValue = responseData.access_token;
                    var userInfo = {
                        fullName: responseData.full_name,
                        username: responseData.userName
                    };

                    var expire = new Date();
                    expire.setHours(14 * 24);

                    if (rememberMe) {
                        $cookies.put(TOKEN_KEY, tokenValue, {expires: expire})
                    }
                    else {
                        sessionStorage.setItem(TOKEN_KEY, tokenValue);
                    }

                    $http.defaults.headers.common.Authorization = 'Bearer ' + tokenValue;
                    identity.setUser(userInfo);
                    defered.resolve();
                }, function (err) {
                    defered.reject(err);
                });

            return defered.promise;
        };

        var getIdentity = function () {
            var deferred = $q.defer();

            $http.get(baseUrl + 'Account/Identity')
                .success(function (identityResponse) {
                    var user = {
                        userName: identityResponse.Username,
                        fullName: identityResponse.FullName
                    };

                    identity.setUser(user);
                    deferred.resolve(user);
                });

            return deferred.promise;
        };

        return {
            register: register,
            login: login,
            getIdentity: getIdentity,
            isAuthenticated: function () {
                return !!$cookies.get(TOKEN_KEY) || sessionStorage.getItem(TOKEN_KEY);
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
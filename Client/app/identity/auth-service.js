(function () {
    'use strict';

    var authService = function authService($http, $q, $cookies, identity, globalConstants, $location) {
        var TOKEN_KEY = 'authentication';

        var register = function register(user) {
            var defered = $q.defer();

            $http
                .post(globalConstants.baseUrl + 'Account/Register', user)
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

            $http.post(globalConstants.baseUrl + 'Account/login', data, {
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
            $http.defaults.headers.common.Authorization = 'Bearer ' + sessionStorage.getItem(TOKEN_KEY);

            $http.get(globalConstants.baseUrl + 'Account/Identity').then(function (identityResponse) {
                    var user = {
                        userName: identityResponse.data.Username,
                        fullName: identityResponse.data.FullName
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
                $http.post(globalConstants.baseUrl + 'Account/Logout').then(function () {
                    $cookies.remove(TOKEN_KEY);
                    sessionStorage.removeItem(TOKEN_KEY);
                    sessionStorage.removeItem('background');
                    $http.defaults.headers.common.Authorization = null;
                    identity.removeUser();
                    $location.path('/');
                })
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('auth', ['$http', '$q', '$cookies', 'identity', 'globalConstants', '$location', authService]);
}());
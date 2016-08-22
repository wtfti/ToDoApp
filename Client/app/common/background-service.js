(function() {
    'use strict';

    var backgroundService = function backgroundService(data, notifier) {
        var BACKGROUND_KEY = 'background';
        return {
            getBackground: function () {
                var background = sessionStorage.getItem(BACKGROUND_KEY);

                if (background) {
                    return background
                }

                data.get('Account/Background').then(function (response) {
                    var base64Image = response.data;
                    sessionStorage.setItem(BACKGROUND_KEY, base64Image);
                    background = base64Image;
                }, function () {
                    notifier.error('Failed to get background')
                });

                return background;
            }
        };
    };

    angular
        .module('ToDoApp.services')
        .factory('background', ['data', 'notifier', backgroundService]);
}());
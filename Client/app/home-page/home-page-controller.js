(function() {
    'use strict';

    var homePageController = function homePageController(auth) {
        this.text = 'test';

        console.log(auth);
        this.register = function () {
            
        }
    };

    angular
        .module('ToDoApp.controllers')
        .controller('HomePageController', ['auth', 'notifier', homePageController]);
}());
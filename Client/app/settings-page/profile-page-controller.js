(function () {
    'use strict';

    var profileTabDirectiveController = function profileTabDirectiveController(background) {
        var vm = this;
        this.backgroundDropdown = 'Background color';

        background.getBackground().then(function (background) {
            if (background.length > 10) {
                vm.currentBackground = background;
            }
        })
    };

    angular
        .module('ToDoApp.controllers')
        .controller('ProfileTabDirectiveController', ['background', profileTabDirectiveController]);
}());
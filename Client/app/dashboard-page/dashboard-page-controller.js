(function () {
    'use strict';

    var dashboardPageController = function dashboardPageController(background, $scope) {
        var base64Image = background.getBackground();
        $scope.backgroundImage = 'url(' + base64Image + ')';

        $scope.notesData = [
            {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }, {
                id: 5,
                title: 'title',
                content: 'helllo from angular',
                created: '28934789234',
                expired: '234234',
                isExpired: '32453254435',
                isCompleted: true,
                createdFrom: 'Gulit',
                sharedWith: ['az', 'you', 'mybrother']
            }];
    };

    angular.module('ToDoApp.controllers')
        .controller('DashboardPageController', ['background', '$scope', dashboardPageController]);
}());
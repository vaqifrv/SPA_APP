var headerDir = function () {
    return {
        restrict: 'E',
        scope: {
            data: '='
        },
        replace: true,
        templateUrl: '/Scripts/AngularJS/app/header/templates/header-dir.html',
        link: function ($scope) {

        }
    }
};


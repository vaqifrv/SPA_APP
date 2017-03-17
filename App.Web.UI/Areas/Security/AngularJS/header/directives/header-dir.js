var headerDir = function () {
    return {
        restrict: 'E',
        scope: {
            data: '='
        },
        replace: true,
        templateUrl: '/Areas/Security/AngularJS/header/templates/header-dir.html',
        link: function ($scope) {

        }
    }
};


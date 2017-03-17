var footerDir = function () {
    return {
        restrict: 'E',
        scope: {
            data: '='
        },
        templateUrl: '/Areas/Security/AngularJS/footer/templates/footer-dir.html',
        link: function ($scope) {
            var currentDate = new Date();
            $scope.currentYear = currentDate.getFullYear();
        }
    }
};


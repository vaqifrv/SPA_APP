var footerDir = function () {
    return {
        restrict: 'E',
        scope: {
            data: '='
        },
        templateUrl: '/Scripts/AngularJS/app/footer/templates/footer-dir.html',
        link: function ($scope) {
            var currentDate = new Date();
            $scope.currentYear = currentDate.getFullYear();
        }
    }
};


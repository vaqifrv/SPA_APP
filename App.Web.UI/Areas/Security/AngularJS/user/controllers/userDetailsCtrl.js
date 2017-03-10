var userDetailsCtrl = function ($scope, $routeParams, UserService, Notification) {

    $scope.data = {
        userProfile: {},
        notification: null
    }

    UserService.getUserDetailsByUseName($routeParams["username"]).then(function (data) {
        if (data.Success) {
            $scope.data.userProfile = data.Value;

        }
    });

}
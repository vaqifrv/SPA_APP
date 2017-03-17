var userEditCtrl = function ($scope, $routeParams, UserService, Notification) {

    $scope.data = {
        adminEdit: {},
        notification: null
    }

    UserService.getUserDataByUsername($routeParams["username"]).then(function (data) {
        if (data.Success) {
            $scope.data.adminEdit = data.Value;
         
        }
    });

    $scope.saveAdminEdit = function() {
        UserService.update($scope.data.adminEdit).then(function(data) {
            if (data.Success) {
                $scope.data.notification = Notification.showSuccess("</b> Məlumatlar dəyşdirildi.", "Siyahıya qayıt", "/Security#user/index");
            }
        });
    }
}
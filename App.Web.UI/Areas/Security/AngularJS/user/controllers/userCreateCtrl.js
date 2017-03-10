var userCreateCtrl = function ($scope, $routeParams, UserService, Notification) {
    $scope.data = {
        user: {
            Username: null,
            Password: null
        },
        notification: null
    };

    //UserService.getUserDataByUsername($routeParams["username"]).then(function (data) {
    //    if (data.Success) {
    //        $scope.data.user = data.Value.user;
    //        $scope.data.user.CheckedRights = data.Value.CheckedRights;
    //    }
    //});

    var clear = function () {
        $scope.data = {
            user: {
                Username: null,
                Password: null
            },
            notification: null
        };
    }

    $scope.save = function () {
       
        UserService.create($scope.data.user).then(function (data) {
            if (data.Success) {
                if ($routeParams.id) {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.user.Username + "</b> İstifadəçi dəyşdirildi.", "Siyahıya qayıt", "/security/user/index");
                } else {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.user.Username + "</b> İstifadəçi əlavə edildi.", "Siyahıya qayıt", "/security/user/index");
                    clear();
                }
            } else {
                $scope.data.notification = Notification.showError("Səhv oldu. Yenidən yoxlayın.");
            }
        });
    };

}
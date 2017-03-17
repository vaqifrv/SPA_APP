var roleCreateCtrl = function ($scope, $routeParams, RoleService, Notification) {
    $scope.data = {
        role: {
            Name: null,
            Description: null,
            CheckedRights: []
        },
        notification: null
    };

    RoleService.getDataForCreateOrEdit($routeParams.id).then(function(data) {
        if (data.Success) {
            $scope.data.role = data.Value.Role;
            $scope.data.role.CheckedRights = data.Value.CheckedRights;
        }
    });

    var clear = function() {
        $scope.data = {
            role: {
                Name: null,
                Description: null,
                CheckedRights: []
            },
            notification: null
        };
    }

    $scope.save = function () {
        var model = {
            Role: $scope.data.role,
            CheckedRights: $scope.data.role.CheckedRights
        }
        RoleService.save(model).then(function (data) {
            if (data.Success) {
                if ($routeParams.id) {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.role.Name + "</b> Rol dəyşdirildi.", "Siyahıya qayıt", "/Security#role/index");
                } else {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.role.Name + "</b> Rol əlavə edildi.", "Siyahıya qayıt", "/Security#role/index");
                    clear();
                }
            } else {
                $scope.data.notification = Notification.showError("Səhv oldu. Yenidən yoxlayın.");
            }
        });
    };

}
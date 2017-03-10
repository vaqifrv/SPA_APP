var rightCreateCtrl = function ($scope, $routeParams, RightService, Notification) {

    $scope.data = {
        right: {
            Id: $routeParams.id,
            Name: null,
            Description: null
        },
        notification: null
    };

    if ($routeParams.id) {
        RightService.getItem($routeParams.id).then(function (data) {
            $scope.data.right = data.Value;
        });
    }

    var clear = function () {
        $scope.data.right = {
            Id: $routeParams.id,
            Name: null,
            Description: null
        };
    };

    $scope.save = function () {
        RightService.save($scope.data.right).then(function (data) {
            if (data.Success) {
                if ($scope.data.right.Id) {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.right.Name + "</b> səlahiyyəti dəyşdirildi.", "Siyahıya qayıt", "/security/right/index");
                } else {
                    $scope.data.notification = Notification.showSuccess("<b>" + $scope.data.right.Name + "</b> səlahiyyəti əlavə edildi.", "Siyahıya qayıt", "/security/right/index");
                    clear();
                }
            } else {
                $scope.data.notification = Notification.showError("Səhv oldu. Yenidən yoxlayın.");
            }
        });
    };

};
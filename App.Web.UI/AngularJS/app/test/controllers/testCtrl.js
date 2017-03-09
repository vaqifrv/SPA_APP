var testCtrl = function ($scope, TestService) {
    $scope.data = {
        title: "Test Page",
        testList: []
    };

    TestService.getAll().then(function(data) {
        $scope.data.testList = data;
    });
}

testCtrl.$inject = ['$scope', 'TestService'];
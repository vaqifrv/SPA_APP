var roleListCtrl = function ($scope, RoleService) {

    $scope.data = {
        roleList: [],
        filter: {
            roleName: null
        },
        showLoader: true,
        totalPages: [],
        totalCount: 0,
        currentPage: 1,
        currentSize: 10,
        pageSizes: [{ Code: 5, Name: "5" }, { Code: 10, Name: "10" }, { Code: 25, Name: "25" }, { Code: 50, Name: "50" }, { Code: 100, Name: "100" }, { Code: 100, Name: "Hamısını göstər" }]
    };

    $scope.data.gotoPage = function (pageSize, pageNumber) {
        $scope.data.showLoader = true;
        RoleService.getAll(pageSize, pageNumber, $scope.data.filter.roleName).then(function (data) {
            $scope.data.totalPages = [];
            for (var i = 1; i <= data.TotalPages; i++) {
                $scope.data.totalPages.push(i);

            }
            $scope.data.totalCount = data.TotalCount;
            $scope.data.roleList = data.List;

            $scope.data.showLoader = false;
        });
        $scope.data.currentPage = pageNumber;
        $scope.data.currentSize = pageSize;
    }

    $scope.data.search = function () {
        $scope.data.gotoPage(10, 1);
        jQuery('.ag-btn-close-search').click();
    }

    $scope.data.gotoPage(10, 1);

    $scope.data.clear = function (event) {
        event.preventDefault();

        $scope.data.filter = {
            roleName: null
        };
        $scope.data.gotoPage(10, 1);
    };

    $scope.data.refresh = function () {
        $scope.data.gotoPage($scope.data.currentSize, $scope.data.currentPage);
    };
};
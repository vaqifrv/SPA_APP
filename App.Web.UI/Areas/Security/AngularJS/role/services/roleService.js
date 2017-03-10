var roleService = function ($http) {
    var urlApi = "api/role/";

    this.getAllRoles = function () {
        return $http.get(urlApi + "getAllRoles").then(function (result) {
            return result.data;
        });
    }

    // GET ALL FOR PAGER
    this.getAll = function (pageSize, pageNumber, roleName) {
        var resultUrl = urlApi + "getAll/" + pageSize + "/" + pageNumber + "/" + roleName;
        return $http.get(resultUrl).then(function (result) {
            return result.data;
        });
    }

    this.getDataForCreateOrEdit = function (id) {
        return $http.get(urlApi + "GetDataForCreateOrEdit/" + id).then(function (result) {
            return result.data;
        });
    }

    // CREATE OR UPDATE role
    this.save = function (role) {
        return $http.post(urlApi + "Save", role).then(function (result) {
            return result.data;
        });
    }


    // DELETE BY ID
    this.delete = function (id) {
        return $http.delete(urlApi + "delete/" + id).then(function (result) {
            return result.data;
        });
    }
}
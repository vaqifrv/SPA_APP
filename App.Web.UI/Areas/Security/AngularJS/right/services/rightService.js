var rightService = function ($http) {
    var urlApi = "api/right/";

    this.getAllRights = function () {
        return $http.get(urlApi + "getAllRights").then(function (result) {
            return result.data;
        });
    }

    // GET ALL FOR PAGER
    this.getAll = function (pageSize, pageNumber, rightName) {
        var resultUrl = urlApi + "getAll/" + pageSize + "/" + pageNumber + "/" + rightName;
        return $http.get(resultUrl).then(function (result) {
            return result.data;
        });
    }

    // CREATE OR UPDATE RIGHT
    this.save = function(right) {
        return $http.post(urlApi + "save", right).then(function(result) {
            return result.data;
        });
    }


    // DELETE BY ID
    this.delete = function(id) {
        return $http.delete(urlApi + "delete/" + id).then(function(result) {
            return result.data;
        });
    }

    this.getItem = function (id) {
        return $http.get(urlApi + "GetItem/" + id).then(function (result) {
            return result.data;
        });
    }
};
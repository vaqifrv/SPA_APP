var userService = function ($http) {
    var urlApi = "api/user/";

    this.getAllUsers = function () {
        return $http.get(urlApi + "getAllUsers").then(function (result) {
            return result.data;
        });
    }

    // GET ALL FOR PAGER
    this.getAll = function (pageSize, pageNumber, userName) {
        var resultUrl = urlApi + "getAll/" + pageSize + "/" + pageNumber + "/" + userName;
        return $http.get(resultUrl).then(function (result) {
            return result.data;
        });
    }

    this.getUserDataByUsername = function (username) {
        return $http.get(urlApi + "GetUserDataByUsername/" + username).then(function (result) {
            return result.data;
        });
    }

    // CREATE OR UPDATE User
    this.create = function (user) {
        return $http.post(urlApi + "Create", user).then(function (result) {
            return result.data;
        });
    }

    // CREATE OR UPDATE User
    this.update = function (adminEditModel) {
        return $http.post(urlApi + "Update", adminEditModel).then(function (result) {
            return result.data;
        });
    }

    // DELETE BY ID
    this.delete = function (username) {
        return $http.delete(urlApi + "delete/" + username).then(function (result) {
            return result.data;
        });
    }


    // LOCK USER OR NOT
    this.lock = function (username, checked) {
        return $http.get(urlApi + "lock/" + username + "/" + checked).then(function (result) {
            return result.data;
        });
    }

    // RESET PASSWORD
    this.resetPassword = function (username, newPassword, confirmPassword) {
        return $http.get(urlApi + "ResetPassword/" + username + "/" + newPassword + "/" + confirmPassword)
            .then(function (result) {
                return result.data;
            });
    }

    // GET PROFILE DATA BY USERNAME
    this.getProfileDataByUsername = function(username) {
        return $http.get(urlApi + "GetProfileDataByUsername/" + username).then(function(result) {
            return result.data;
        });
    }

    // GET USER DETAILS BY USER NAME
    this.getUserDetailsByUseName = function(username) {
        return $http.get(urlApi + "GetUserDetailsbyUseName/" + username).then(function (result) {
            return result.data;
        });
    }
}
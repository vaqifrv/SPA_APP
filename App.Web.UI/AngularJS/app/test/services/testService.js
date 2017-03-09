var testService = function ($http) {
    var apiUrl = 'api/test/';

    this.getAll = function () {
        return $http.get(apiUrl + 'getAll').then(function (response) {
            return response.data;
        });
    };
};


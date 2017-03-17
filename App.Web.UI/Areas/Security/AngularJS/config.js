var config = function ($locationProvider, $routeProvider) {

    $locationProvider.hashPrefix('');

    $routeProvider
        .when('/dashboard',
        {
            // For Home Page  
            templateUrl: '/Areas/Security/AngularJS/home/views/home.html',
            controller: 'HomeCtrl'
        })
        // Security
        .when('/right/index',
        {
            // RIGHT INDEX
            templateUrl: '/Areas/Security/AngularJS/right/views/right-list.html',
            controller: 'RightListCtrl'
        })
        .when('/right/create/:id?',
        {
            // RIGHT INDEX
            templateUrl: '/Areas/Security/AngularJS/right/views/right-create.html',
            controller: 'RightCreateCtrl'
        })
        .when('/role/index',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/role/views/role-list.html',
            controller: 'RoleListCtrl'
        })
        .when('/role/create/:id?',
        {
            // ROLE create
            templateUrl: '/Areas/Security/AngularJS/role/views/role-create.html',
            controller: 'RoleCreateCtrl'
        })
        .when('/user/index',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-list.html',
            controller: 'UserListCtrl'
        })
        .when('/user/edit/:username?',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-edit.html',
            controller: 'UserEditCtrl'
        })
        .when('/user/create',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-create.html',
            controller: 'UserCreateCtrl'
        })
        .when('/user/details/:username?',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-details.html',
            controller: 'UserDetailsCtrl'
        })
        .otherwise({ redirectTo: '/dashboard' });
}

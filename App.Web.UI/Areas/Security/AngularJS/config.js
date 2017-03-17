var config = function ($locationProvider, $routeProvider) {

    $locationProvider.hashPrefix('');

    $routeProvider
        .when('/home',
        {
            // For Home Page  
            templateUrl: '/Scripts/angularJs/app/home/views/home.html',
            controller: 'HomeCtrl'
        })
        // Security
        .when('/security/right/index',
        {
            // RIGHT INDEX
            templateUrl: '/Areas/Security/AngularJS/right/views/right-list.html',
            controller: 'RightListCtrl'
        })
        .when('/security/right/create/:id?',
        {
            // RIGHT INDEX
            templateUrl: '/Areas/Security/AngularJS/right/views/right-create.html',
            controller: 'RightCreateCtrl'
        })
        .when('/security/role/index',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/role/views/role-list.html',
            controller: 'RoleListCtrl'
        })
        .when('/security/role/create/:id?',
        {
            // ROLE create
            templateUrl: '/Areas/Security/AngularJS/role/views/role-create.html',
            controller: 'RoleCreateCtrl'
        })
        .when('/security/user/index',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-list.html',
            controller: 'UserListCtrl'
        })
        .when('/security/user/edit/:username?',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-edit.html',
            controller: 'UserEditCtrl'
        })
        .when('/security/user/create',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-create.html',
            controller: 'UserCreateCtrl'
        })
        .when('/security/user/details/:username?',
        {
            // ROLE INDEX
            templateUrl: '/Areas/Security/AngularJS/user/views/user-details.html',
            controller: 'UserDetailsCtrl'
        })
        .otherwise({ redirectTo: '/home' });
}

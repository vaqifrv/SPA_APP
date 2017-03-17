var app = angular.module('AppSecurity', ['ngRoute']);

app.config(['$locationProvider', '$routeProvider', config]);


// ADDING DIRECTIVES

// BEGIN MAIN
app.directive('footerDir', footerDir);
app.directive('headerDir', headerDir);
app.directive('leftSidebarDir', leftSidebarDir);
app.directive("loader", loader);
// END MAIN

// END ADDING DIRECTIVES

// ADDING SERVICE

// BEGIN MAIN
// END MAIN

// BEGIN SECURITY

// BEGIN RIGHT
app.service("RightService", rightService);
app.service("RoleService", roleService);
app.service("UserService", userService);
app.service("Notification", notification);
// END RIGHT

// END SECURITY

// END ADDING SERVICE

// ADDING CONTROLLERS

// BEGIN MAIN
app.controller("MainCtrl", mainCtrl);
app.controller("HomeCtrl", homeCtrl);
// END MAIN

// BEGIN SECURITY
// BEGIN RIGHT
app.controller('RightListCtrl', rightListCtrl);
app.controller('RightCreateCtrl', rightCreateCtrl);
app.controller('RoleListCtrl', roleListCtrl);
app.controller('RoleCreateCtrl', roleCreateCtrl);
app.controller('UserListCtrl', userListCtrl);
app.controller('UserCreateCtrl', userCreateCtrl);
app.controller('UserEditCtrl', userEditCtrl);
app.controller('UserDetailsCtrl', userDetailsCtrl);
// END RIGHT
// END SECURITY

// END ADDING CONTROLLERS
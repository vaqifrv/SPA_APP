var app = angular.module('App', ['ngRoute']);

// Begin Services
app.service('TestService', testService);
// End Services

// Begin Controllers
app.controller('MainCtrl', mainCtrl);
app.controller('HomeCtrl', homeCtrl);
app.controller('TestCtrl', testCtrl);
// End Controllers

app.config(['$locationProvider', '$routeProvider', configFunction]);
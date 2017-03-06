var App = angular.module('App', ['ngRoute']);

App.controller('MainCtrl', mainCtrl);
App.controller('HomeCtrl', homeCtrl);
App.controller('TestCtrl', testCtrl);

var configFunction = function ($locationProvider, $routeProvider) {

    $routeProvider.
          when('/home', {
              templateUrl: '/AngularJS/app/home/views/home.html',
              controller: homeCtrl
          }).
          when('/test', {
              templateUrl: '/AngularJS/app/test/views/test.html',
              controller: testCtrl
          }).
          otherwise({
              redirectTo: function () {
                  return '/home';
              }
          });
}

App.config(['$locationProvider', '$routeProvider', configFunction]);
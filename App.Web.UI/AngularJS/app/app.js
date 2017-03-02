var App = angular.module('App', ['ngRoute', 'ui.bootstrap', 'chart.js']);

App.controller('MainController', MainController);
App.controller('GridController', GridController);
App.controller('ViewProductController', ViewProductController);

var configFunction = function ($locationProvider, $routeProvider) {

    $routeProvider.
          when('/grid', {
              templateUrl: '/SPA/Views/Grid.html',
              controller: GridController
          }).
          when('/huba/create', {
              templateUrl: '/SPA/Views/Huba.html',
              controller: HubaController
          }).
          otherwise({
              redirectTo: function () {
                  return '/grid';
              }
          });
}

App.config(['$locationProvider', '$routeProvider', configFunction]);
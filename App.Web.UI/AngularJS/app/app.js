var App = angular.module('App', ['ngRoute']);

App.controller('MainCtrl', mainCtrl);
App.controller('HomeCtrl', homeCtrl);
App.controller('TestCtrl', testCtrl);


App.config(['$locationProvider', '$routeProvider', configFunction]);
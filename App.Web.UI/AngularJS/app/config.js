var configFunction = function ($locationProvider, $routeProvider) {
    
    $locationProvider.hashPrefix('');

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
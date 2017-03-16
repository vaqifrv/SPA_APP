app.directive('bootstrapTagsinput', [function() {

  function getItemProperty(scope, property) {
    if (!property)
      return undefined;

    if (angular.isFunction(scope.$parent[property]))
      return scope.$parent[property];

    return function(item) {
      return item[property];
    };
  }

  return {
    restrict: 'EA',
    scope: {
      tags: '=myTags'
    },
    template: '<select multiple ng-model="tags" class="form-control"></select>',
    replace: false,
    link: function(scope, element, attrs) {
      $(function() {
          if (!angular.isArray(scope.tags))
            scope.tags = [];

        var select = $('select', element);
        var typeaheadSourceArray = attrs.typeaheadSource ? attrs.typeaheadSource.split('.') : null;
        var typeaheadSource = typeaheadSourceArray ?
            (typeaheadSourceArray.length > 1 ?
                scope.$parent[typeaheadSourceArray[0]][typeaheadSourceArray[1]]
                : scope.$parent[typeaheadSourceArray[0]])
            : null;

        select.tagsinput(scope.$parent[attrs.options || ''] || {
          typeahead : {
            source   : angular.isFunction(typeaheadSource) ? typeaheadSource : null
          },
          itemValue: getItemProperty(scope, attrs.itemvalue),
          itemText : getItemProperty(scope, attrs.itemtext),
          confirmKeys : getItemProperty(scope, attrs.confirmkeys) ? JSON.parse(attrs.confirmkeys) : [13],
          tagClass: "tag label label-info"
        });

        for (var i = 0; i < scope.tags.length; i++) {
            select.tagsinput('add', scope.tags[i]);
        }

        select.on('itemAdded', function(event) {
            if (scope.tags.indexOf(event.item) === -1)
                scope.tags.push(event.item);
        });

        select.on('itemRemoved', function(event) {
            var idx = scope.tags.indexOf(event.item);
          if (idx !== -1)
              scope.tags.splice(idx, 1);
        });

        // create a shallow copy of model's current state, needed to determine
        // diff when model changes
        var prev = scope.tags.slice();
        scope.$watch("model", function() {
            var added = scope.tags.filter(function (i) { return prev.indexOf(i) === -1; }),
              removed = prev.filter(function (i) { return scope.tags.indexOf(i) === -1; }),
              i;

            prev = scope.tags.slice();

          // Remove tags no longer in binded model
          for (i = 0; i < removed.length; i++) {
            select.tagsinput('remove', removed[i]);
          }

          // Refresh remaining tags
          select.tagsinput('refresh');

          // Add new items in model as tags
          for (i = 0; i < added.length; i++) {
            select.tagsinput('add', added[i]);
          }
        }, true);
      });
    }
  };
}]);

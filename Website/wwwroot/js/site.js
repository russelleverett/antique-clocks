angular.module("images", [])
    .directive("imageOverview", imageOverview);

function imageOverview() {
    var controller = ['$scope', '$http', function ($scope, $http) {
        var vm = this;

        function init() {
            $scope.items = angular.copy($scope.ngModel);
        }
        init();

        $scope.removeImage = function (id) {
            $http.delete('/images/remove/' + id).then(function (response) {
                $scope.items = angular.copy(response.data);
            });
        }

        $scope.defaultImage = function (id) {
            $http.post('/images/default/' + id).then(function (response) {
                $scope.items = angular.copy(response.data);
                alert("Image with ID: " + id + " has been made the default image.");
            });
        }
    }];

    return {
        restrict: 'E',
        templateUrl: '/templates/image-overview.html',
        scope: {
            ngModel: '=ngModel'
        },
        controller: controller
    };
};
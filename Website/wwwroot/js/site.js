angular.module("antiqueClocks", [])
    .directive("imageOverview", imageOverview)
    .service("data", dataService)
    .filter("iif", iif);

function iif() {
    return function (input, trueValue, falseValue) {
        return input ? trueValue : falseValue;
    }
}

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

function dataService($http) {
    this.get = function (route, callback) {
        $http.get(route).then(function (response) {
            if (callback !== null)
                callback(response.data);
        }, function (error) {
            //TODO: NOM NOM
        });
    };
    this.post = function (route, data, callback) {
        $http.post(route, JSON.stringify(data)).then(function (response) {
            if (callback) {
                callback(response.data);
            }
        });
    };
    this.delete = function (route, callback) {
        $http.delete(route).then(function (response) {
            if (callback)
                callback(response.data);
        });
    };
}
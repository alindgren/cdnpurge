angular.module('umbraco').controller("PurgeCDN.Dialog", purgeCdnDialogController);
function purgeCdnDialogController($scope, $routeParams, $http, contentResource) {
    var dialogOptions = $scope.dialogOptions;
    console.debug('dialogOptions', dialogOptions);
    var node = dialogOptions.currentNode;
    // var url = Umbraco.Sys.ServerVariables["articulate"]["articulatePropertyEditorsBaseUrl"] + "GetThemes"; See https://our.umbraco.org/documentation/extending/version7-assets
    $scope.busy = true;
    $http.get('/umbraco/backoffice/PurgeCdn/PurgeCdnAPI/InitDialog?nodeId=' + node.id).success(function (data) {
        console.log(data);
        $scope.PurgeCdnDialogModel = data;
        $scope.busy = false;
    }).error(function (data) {
        console.log(data);
        $scope.PurgeCdnDialogModel = { ErrorMessage: "Unhandled error - see console for details" };
        $scope.busy = false;
    });

    $scope.purgeCdn = function () {
        $scope.busy = true;
       $http.get('/umbraco/backoffice/PurgeCdn/PurgeCdnAPI/Purge?nodeId=' + node.id).success(function (data) {
            console.log(data);
            $scope.PurgeCdnDialogModel = data;
            $scope.busy = false;
        }).error(function (data) {
            console.log(data);
            $scope.PurgeCdnDialogModel = { ErrorMessage: "Unhandled error - see console for details" };
            $scope.busy = false;
        });
    }
}
'use strict';

app.controller('PerformanceDataController', ['$scope', 'backendHubProxy', '$location', 'userSerivce',
  function ($scope, backendHubProxy,$location, userSerivce) {
      $scope.isJoinSectionVisible = true;
      $scope.sessionId = roomId;
      $scope.name = '';
      $scope.points = [0, 2, 5, 8, 13, 16];
      console.log('trying to connect to service');
      //var performanceDataHub = backendHubProxy.addToRoom(roomId);
      console.log('connected to service');


      $scope.join = function () {
          userSerivce.setUserName($scope.name);
          backendHubProxy.addToRoom();
          $scope.isJoinSectionVisible = false;
      }


      $scope.clearVotes=function()
      {
          backendHubProxy.clearVotes();
      }
      $scope.showVotes = function ()
      {
          backendHubProxy.showVotes();
      }

      $scope.addMyVote= function(item) {
          backendHubProxy.addMyVote(item);
      }
  }
]);
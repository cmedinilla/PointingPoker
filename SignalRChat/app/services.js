'use strict';

app.factory('backendHubProxy', ['$rootScope', 'Hub', '$timeout', '$location', 'userSerivce', function ($rootScope, Hub, $timeout, $location, userSerivce) {

    //declaring the hub connection
    var hub = new Hub('chatHub', {

        //client side methods
        listeners: {
            'joinNewClient': function (user,scoreVisibility) {
                for (var i = 0; i < user.length; i++) {
                    var result = $.grep($rootScope.roomMembers, function (e) { return e.connectionId === user[i].Id; });
                    if (result.length === 0) {
                        $rootScope.roomMembers.push({
                            name: user[i].Name,
                            id: user[i].Id,
                            score: user[i].Score,
                            connectionId: user[i].ConnectionId,
                            hideScore: scoreVisibility
                        });
                   }
                }

                $rootScope.$apply();
            },
            'removeClient': function (id) {
                var data = $.grep($rootScope.roomMembers, function (e) {
                    return e.connectionId !== id;
                });
                $rootScope.roomMembers = data;
                $rootScope.$apply();
            },
            'clearVotes': function () {
                for (var i = 0; i < $rootScope.roomMembers.length; i++) {
                    $rootScope.roomMembers[i].score = 0;
                    $rootScope.roomMembers[i].hideScore = true;
                }
                $rootScope.$apply();
            },
            'showVotes': function () {
                for (var i = 0; i < $rootScope.roomMembers.length; i++) {
                    $rootScope.roomMembers[i].hideScore = false;
                }
                $rootScope.$apply();
            },
            'updateVotes': function (connectionId, votePoint) {
                for (var i = 0; i < $rootScope.roomMembers.length; i++) {
                    if ($rootScope.roomMembers[i].connectionId===connectionId) {
                        $rootScope.roomMembers[i].score = votePoint;
                    }
                }
                $rootScope.$apply();
            }
        },

        //server side methods
        methods: ['lock', 'addToRoom', 'ClearVotes', 'ShowVotes', 'addMyVote'],

        //query params sent on initial connection
        queryParams: {
            'roomId': userSerivce.user.roomId || '',
            'id': userSerivce.user.id
        },

        //handle connection error
        errorHandler: function (error) {
            console.error(error);
        },

        //specify a non default root
        //rootPath: '/api

        stateChanged: function (state) {
            switch (state.newState) {
                case $.signalR.connectionState.connecting:
                    //your code here
                    break;
                case $.signalR.connectionState.connected:
                    //your code here
                    break;
                case $.signalR.connectionState.reconnecting:
                    //your code here
                    break;
                case $.signalR.connectionState.disconnected:
                    //your code here
                    break;
            }
        }
    });

    var edit = function (employee) {
        hub.lock(employee.Id); //Calling a server method
    };
    var done = function (employee) {
        hub.unlock(employee.Id); //Calling a server method
    }
    var addToRoom = function () {
        hub.addToRoom(userSerivce.user.name);
    };
    var showVotes= function() {
        hub.ShowVotes();
    }
    var clearVotes= function() {
        hub.ClearVotes();
    }
    var addMyVote = function (value) {
    hub.addMyVote(value);
}
    return {
        editEmployee: edit,
        doneWithEmployee: done,
        addToRoom: addToRoom,
        clearVotes: clearVotes,
        showVotes: showVotes,
        addMyVote: addMyVote

    };
}]);


app.factory('userSerivce', ['$rootScope', function ($rootScope) {
    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    }



    var user = {
        name: '',
        id: guid(),
        roomId: roomId
    };
    var setUserName = function (name) {
        user.name = name;
    }
    return {
        user: user,
        setUserName: setUserName
    }
}]);


/*
app.factory('backendHubProxy', ['$rootScope', 'backendServerUrl',
  function ($rootScope, backendServerUrl) {

      function backendFactory(serverUrl, hubName) {
          var connection = $.hubConnection(backendServerUrl);
          var proxy = connection.createHubProxy(hubName);

          connection.start().done(function () { });

          return {
              on: function (eventName, callback) {
                  proxy.on(eventName, function (result) {
                      $rootScope.$apply(function () {
                          if (callback) {
                              callback(result);
                          }
                      });
                  });
              },
              invoke: function (methodName, callback) {
                  proxy.invoke(methodName)
                  .done(function (result) {
                      $rootScope.$apply(function () {
                          if (callback) {
                              callback(result);
                          }
                      });
                  });
              }
          };
      };

      return backendFactory;
  }]);
*/

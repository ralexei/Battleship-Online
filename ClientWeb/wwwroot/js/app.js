var ships = [];
var lobbies = [];
var cellSize = 31;
var userName = "";

function Ship(obj) {
    this.obj = obj;
    this.id = obj.Id;
    this.pos = {
        x: obj.StartPosition.X,
        y: obj.StartPosition.Y
    };
    this.size = obj.Size;
    this.vertical = obj.ShipOrientation === "Vertical";
    this.div = angular.element('<div class="ship movable" draggable="true"></div>');

    this.init = function () {
        var ship = this;
        var div = this.div;
        div.draggable({
            containment: "#player",
            start: function () {
            },
            drag: function () {
                var x = Math.round(parseFloat(div.css("left")) / (cellSize - 1));
                var y = Math.round(parseFloat(div.css("top")) / (cellSize - 1));
                if (ship.check({ x: x, y: y })) {
                    div.css("border-color", "green");
                }
                else {
                    div.css("border-color", "red");
                }
            },
            stop: function () {
                var x = Math.round(parseFloat(div.css("left")) / (cellSize - 1));
                var y = Math.round(parseFloat(div.css("top")) / (cellSize - 1));
                if (ship.check({ x: x, y: y })) {
                    ship.pos.x += x;
                    ship.pos.y += y;
                }
                ship.redraw();
            }
        }).click(function () {
            if (ship.pos.y <= 10 - ship.size)
                if (ship.pos.x <= 10 - ship.size) {
                    var nship = ship.clone();
                    nship.vertical = !ship.vertical;
                    if (nship.check({ x: 0, y: 0 })) {
                        ship.vertical = !ship.vertical;
                        ship.draw();
                    }
                }
        });
        this.redraw();
    }

    this.redraw = function () {
        $('#cell_' + this.pos.x + '_' + this.pos.y + " div.content").append(this.div);
        this.draw();
        this.div.css("border-color", "blue");
    };

    this.draw = function () {
        if (!this.vertical) {
            this.div.css('width', this.size * cellSize - (this.size - 1) * 2);
            this.div.css('height', cellSize);
        }
        else {
            this.div.css('height', this.size * cellSize - (this.size - 1) * 2);
            this.div.css('width', cellSize);
        }
        this.div.css('left', 0);
        this.div.css('top', 0);
        this.div.css('padding-right', 0);
        this.div.css('padding-bottom', 0);
    }

    this.getPosition = function () {
        var w = 1;
        var h = 1;
        if (this.vertical) {
            w = this.size;
        } else {
            h = this.size;
        }
        return {
            w: w,
            h: h,
            left: this.pos.y,
            right: this.pos.y + w,
            top: this.pos.x,
            bottom: this.pos.x + h
        };
    }

    this.check = function (pos) {
        var b = true;
        var ship = this.clone();
        ship.pos.x += pos.x;
        ship.pos.y += pos.y;
        var pos1 = ship.getPosition();
        ships.forEach(function (el) {
            if (el.id !== ship.id) {
                var pos2 = el.getPosition();
                if (!(pos1.right < pos2.left || pos1.left > pos2.right))
                    if (!(pos1.bottom < pos2.top || pos1.top > pos2.bottom)) {
                        b = false;
                        return;
                    }
            }
        });
        return b;
    }

    this.getCells = function () {
        var arr = [];
        for (var i = 0; i < this.size; i++) {
            if (this.vertical)
                arr.push({ Position: { X: this.pos.x, Y: this.pos.y + i }, Status: "Alive" });
            else
                arr.push({ Position: { X: this.pos.x + i, Y: this.pos.y }, Status: "Alive" });
        }
        return arr;
    }

    this.send = function () {
        var arr = this.getCells();
        var obj = {
            Id: this.id,
            Size: this.size,
            StartPosition: { X: this.pos.x, Y: this.pos.y },
            ShipOrientation: this.vertical ? "Vertical" : "Horizontal",
            Cells: []
        }
        return obj;
    }

    this.clone = function () {
        return new Ship(this.send());
    }
}

var app = angular.module('ngGame', [])
    .controller('BattlefieldController', function ($scope, LobbyService) {
        LobbyService.init();
        $scope.randomShips = function () {
            LobbyService.randomShips()
        };
        $scope.setName = function () {
            LobbyService.addUser($scope.name);
        }
    })

    .controller('LobbyController', function ($scope, $timeout, LobbyService) {
        $scope.lobbies = LobbyService.lobbies;
        $scope.name = "";
        $scope.stage = "Select";
        $scope.pass = "";
        $scope.lobby = "";
        $scope.pass2 = "";
        $scope.update = function () {
            $timeout(function () {
                $scope.lobbies = LobbyService.lobbies;
                if ($scope.name === "")
                    $scope.name = userName;
                if ($scope.stage === "Select")
                    $scope.update();
            });
        }
        $scope.update();
        $scope.createLobby = function () {
            $scope.isCreated = true;
            LobbyService.createLobby($scope.name, $scope.pass);
            $scope.stage = "Next";
            angular.element(".lobbies").hide();
            angular.element(".btn-lobby").hide();
            angular.element(".info").show();
        }
        $scope.passLobby = function () {
            LobbyService.sendPass($scope.lobby, $scope.pass2);
        }
        $scope.play = function (el) {
            $scope.lobby = el.lobby.Creator;
            LobbyService.play(el.lobby.Creator);
        };
    })

    .controller('UserController', function ($scope, $timeout, UserService) {
        $scope.name = "Guest";
        $scope.elo = $scope.elo;
        UserService.init($scope);
        $scope.setName = function () {
            UserService.addUser($scope.name);
        };
    })

    .controller('InitController', function ($scope) {
        $scope.hello = "Hi";
        $.connection.hub.start().done(function () {
            $.connection.lobbyHub.server.getField();
            $.connection.lobbyHub.server.connect();
            if ($.cookie("id") === undefined) {
                $("#nameModal").modal("show");
            }
            else {
                $.connection.userHub.server.hasUser($.cookie("id"));
            }
        });
    })

    .factory('UserService', function () {
        return {
            hub: $.connection.userHub,
            $scope: null,
            elo: 0,
            init: function ($scope) {
                var service = this;
                service.$scope = $scope;
                var hub = this.hub;
                hub.client.setCookie = function (id) {
                    $.cookie("id", id, { expires: 1000, path: '/' });
                };
                hub.client.setUser = function (user) {
                    $scope.name = user.Name;
                    $scope.elo = user.Elo;
                    userName = user.Name;
                };
                hub.client.noUser = function () {
                    $("#nameModal").modal("show");
                };
                hub.client.setConnectedPlayerName = function (currentUserName) {
                  userName = currentUserName;
                };
            },
            addUser: function (user) { this.hub.server.addUser(user); }
        };
    })

    .factory('LobbyService', function () {
        return {
            id: 0,
            hub: $.connection.lobbyHub,
            lobbies: [],
            play: function (Userid) {
                if ($.cookie('id'))
                    this.hub.server.play(this.id, Userid, $.cookie('id'));
            },
            init: function () {
                var service = this;
                var hub = this.hub;
                hub.client.pass = function (lobby, err) {
                    $("#lobbytitle").html(lobby.LobbyName);
                    $("#lobby").val(lobby.Creator);
                    $("#passModal").modal("show");
                }

                hub.client.log = function (str) {
                    console.log("Server:" + str);
                }

                hub.client.play = function (id) {
                    $(".room #roomId").val(id);
                    $(".room #player").val(service.id);
                    $(".room").submit();
                }

                hub.client.initField = function (field) {
                    service.clearShips();
                    ships = field.map(function f(el, i) {
                        var ship = new Ship(el);
                        ship.init();
                        return ship;
                    });
                }

                hub.client.getUserId = function (id) {
                    service.id = id;
                }

                hub.client.getMap = function () {
                    ships.forEach(function (el) {
                        el.div.draggable('disable');
                        el.div.click(function () {
                            alert("Wait for game");
                        });
                    });
                    var send = ships.map(function (el) {
                        return el.send();
                    });
                    hub.server.getMap(service.id, send);
                }

                hub.client.getLobbies = function (lobbies) {
                    service.lobbies = lobbies;
                }
            },
            createLobby(name, pass) {
                this.hub.server.addLobby(name, pass);
            },
            randomShips: function () {
                this.hub.server.getField();
            },
            clearShips: function () {
                $(".ship").remove();
            },
            sendPass: function (Userid, pass) {
                this.hub.server.pass(this.id, Userid, pass);
            }
        };
    }).directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    });

$(document).ready(function () {
    $("#input-box").keyup(function (event) {
        if (event.keyCode == 13) {
            $("#enter-btn").click();
        }
    });
});
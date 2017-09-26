var cellSize = 31;

var app = angular.module('ngGame', [])
    .controller('PlayerController', function ($scope, $timeout, GameService) {
        GameService.init();
        $scope.cells = [];
        $scope.update = function () {
            $timeout(function () {
                var ships = GameService.myMap.ships;
                if (Array.isArray(ships)) {
                    for (var i = 0; i < ships.length; i++)
                    {
                        var ship = ships[i];
                        var div = angular.element("#cell_" + ship.StartPosition.Y + "_" + ship.StartPosition.X + " .content .ship");
                        if (div.length > 0)
                        {
                            var b = true;
                            ship.Cells.forEach(function (el) {
                                b &= el.Status !== "Alive";
                            });
                            if (b) {
                                div.css("border", "2px solid red");
                            }
                        }
                        else
                        {
                            var cell = angular.element("#cell_" + ship.StartPosition.Y + "_" + ship.StartPosition.X + " .content");
                            div = angular.element('<div class="ship"></div>');
                            if (ship.ShipOrientation !== "Vertical")
                            {
                                div.css('width', ship.Size * cellSize - (ship.Size - 1) * 2);
                                div.css('height', cellSize);
                            }
                            else
                            {
                                div.css('height', ship.Size * cellSize - (ship.Size - 1) * 2);
                                div.css('width', cellSize);
                            }
                            cell.append(div);
                        }
                    }
                }
                $scope.cells = GameService.myMap.cells;
                $scope.update();
            });
        }
        $scope.update();
    })
    .controller('OpponentController', function ($scope, $timeout, GameService) {
        $scope.cells = [];
        $scope.onCellClick = function (cell)
        {
            var pos = { X: cell.col, Y: cell.$parent.row };
            if($scope.cells[pos.X][pos.Y] === 'Hidden');
                GameService.onTakeShoot(pos);
        };
        $scope.update = function () {
            $timeout(function () {
                GameService.deadShips.forEach(
                    function (ship) {
                        var div = angular.element("#cell2_" + ship.StartPosition.Y + "_" + ship.StartPosition.X + " .content .ship");
                        if (div.length === 0)
                        {
                            var cell = angular.element("#cell2_" + ship.StartPosition.Y + "_" + ship.StartPosition.X + " .content");
                            div = angular.element("<div class=\"ship\"></div>");
                            if (ship.ShipOrientation !== "Vertical") {
                                div.css('width', ship.Size * cellSize - (ship.Size - 1) * 2);
                                div.css('height', cellSize);
                            }
                            else {
                                div.css('height', ship.Size * cellSize - (ship.Size - 1) * 2);
                                div.css('width', cellSize);
                            }
                            div.css("border", "solid 2px red");
                            cell.append(div);
                        }
                    }
                );
                if (GameService.waiting)
                {
                    $(".bg_info").show();
                }
                else
                {
                    $(".bg_info").hide();
                }
                $scope.cells = GameService.opMap;
                $scope.update();
            });
        }
        $scope.update();
    })
    .controller('BackController', function ($scope) {
        $scope.home = function ()
        {
            window.location.href = "/";
        }
    })
    .factory('GameService', function () {
        return {
            hub: $.connection.gameHub,
            onTakeShoot: function (pos)
            {
                this.hub.server.takeShoot(gameId, player, pos);
            },
            id: 0,
            waiting: true,
            myMap: [],
            opMap: [],
            deadShips: [],
            init: function ()
            {
                var service = this;
                var hub = this.hub;
                var gameOvered = 0;
                hub.client.noGame = function ()
                {
                    window.location.href = "/";
                }
                hub.client.endGame = function (b, connectionId) {
                    $(".full").show();
                    if (gameOvered === 0) {
                        if (b === -1) {
                            hub.server.gameOver(service.id, connectionId);
                            $(".full .status .txt").html("Player left the game.");
                        }
                        else if (b === 0) {
                            hub.server.gameOver(connectionId, service.id);
                            $(".full .status .txt")[0].innerHTML = "You Lose.";
                        }
                        else if (b === 1) {
                            hub.server.gameOver(service.id, connectionId);
                            $(".full .status .txt")[0].innerHTML = "You Win.";
                        }
                    }
                    gameOvered = 1;
                };

                hub.client.setId = function (id) {
                    service.id = id;
                    hub.server.getInfo(gameId, id, player);
                };

                hub.client.updateInfo = function () {
                    hub.server.getInfo(gameId, service.id, player);
                };

                hub.client.getInfo = function (my, op, deadShips, b) {
                    service.myMap = my;
                    service.opMap = op;
                    service.deadShips = deadShips;
                    service.waiting = b;
                };

                $.connection.hub.start(function () {
                    hub.server.startGame(gameId, player);
                    hub.server.connect();
                    gameOvered = 0;
                });
             }
        };
    });
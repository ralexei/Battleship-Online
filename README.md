# The Battleship online game


**1). Map requirements.**

-Ships: 1 x 1, 2 x 3, 3 x 2, 4 x 1;

-No collisions between ships;

-Flat ship structure;

-Ability to randomize ship positions;

-Ability to arrange ships manually;


**2). Lobby.**

-List all players that started the game and is waiting for opponent;

-Player with higher rank* must be on top in lobby;

-Player must fill the map before start/join the game (randomized by default);

-Player must have ability to set password for game (play with friends);


**3).Game flow.**

-On game page, will be 2 maps, on the left side is map of current player (with ships), on the right side is map of opponent player(initial empty)

-Server will determine player that hit first;

-After hit, player will wait for opponent hit;

-When opponent leaves the game, current player are winner. (vice versa respectively);

-On the end of game will be calculated rank for both players.


**4).Leader board.**

-Display top (x) ranked players from the beginning.

* - Use ELO rating system

- - - -

**Technical requirements** 

The game is web application;

The application is real time*;

The application must be supported by Google Chrome & Mozilla Firefox browser;

The application will be deployed to real server.

* - using WebSockets protocol.

- - - -

**Tools & technologies**

.NET Framework 4.6.2;

ASP.NET MVC (not ASP.NET CORE) use empty asp.net project template

Angular framework + Bootstrap;

Use Bower as client package dependency manager;

Use SignalR for wss implementation;

Entity framework ORM;

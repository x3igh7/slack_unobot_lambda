# slack_unobot_lambda
a server-less slack bot to play uno using .net core

How to play:
* Integrate with slack (webhook to be provided)
* Use commands! Just type "Unobot:" followed by
  * create (create a new game for your channel if one isn't already in progress)
  * join (up to 4 players can join, minimum of 2)
  * start (starts the game when you have enough players)
  * play _card_name_ (example: "Unbobot: play r1" pr "Unobot: play wd4 r" to choose a wild color)

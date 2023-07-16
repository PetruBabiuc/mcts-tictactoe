# mcts-tictactoe
This application represents a game of Tic-Tac-Toe. It has the following features:
* It can be played only user against AI
* Adjustable
  * Board size (has to be greater than 2x2)
  * Number of simulations done by the computer before each action
* The computer chooses it's actions using one of the 2 algorithms:
  * Original MCTS (Monte Carlo Tree Search)
  * Modified MCTS (it does not _forget_ the results of the simulations preserving them)
* The player can reset or preserve the computer's experience (the tree created with the simulations) between games
* The computer can consider the draw a victory making it even harder for the player to achive the victory

The application has the following class diagram:
![Class diagram](https://github.com/PetruBabiuc/mcts-tictactoe/blob/main/diagrams/Class%20diagram.png)

The following images represent screenshots with the application:
![screenshot](https://github.com/PetruBabiuc/mcts-tictactoe/blob/main/screenShots/1.png)
![screenshot](https://github.com/PetruBabiuc/mcts-tictactoe/blob/main/screenShots/2.png)
![screenshot](https://github.com/PetruBabiuc/mcts-tictactoe/blob/main/screenShots/3.png)
![screenshot](https://github.com/PetruBabiuc/mcts-tictactoe/blob/main/screenShots/4.png)

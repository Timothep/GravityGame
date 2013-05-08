# Description

GravityGame is a very simple game, that allows you to simulate the effect gravity has on bodies, presumably planets. 
You can add & remove planets of different sizes, give them a velocity, and watch how they interact.
The game also has a text console that allows you to do things like saving and loading worlds (or rather, universes).

![Screenshot][https://dl.dropbox.com/u/60089248/zooi/screenshot.png]

# Playing instructions

Basic playing instructions are displayed once the game starts. 

Here are some additional instructions that may be of use:
* You can pause the game by pressing P.
* To save the game, open the console by pressing the tilde (~) key, enter save <name> and press enter.
* Similarly, using the load command you can load a game.
* Removing planets is a bit tricky, as I haven't really implemented a proper method for removing them (yet). Right now, you can keep track of all planets in the universe at the top left of the screen. Each GravityObject is a planet, and each planet has a number. The first planet you add has number 0 and therefore will be displayed as GravityObject0. The second planet will have number 1, the third one number 2, and so on. Once a planet gets removed, the numbers will shift to fill in the gap, so if you remove planet number 0, the planet that was previously number 1 will now be number 0. To remove a planet with a number, enter del number in the console.
* The game also has a settings file in which you can change a few options about how the game is displayed.

# Requirements

* .NET XX
* XNA

# Technical Details

The game is written in C#, using the XNA library to handle the graphics. It is called a "simulation", even though it actually isn't, as it doesn't simulate gravity as it works in our universe.

The formulas are a little bit tweaked, so that the force of gravity reduces more strongly as the distance between two objects decreases. This way it's possible for two objects not to influence each other when they're on either sides of the screen, while still being able to influence each other strongly when they're close to each other. This effect is present in the real universe, though to a lesser extent (in reality, the force gets divided by the distance squared. In the simulation, the force gets divided by the distance cubed).

As you may notice, console input is very limited. A lot of keys simply don't work. This is because XNA doesn't come with a library for text input, so I had to listen for key press events and perform a huge switch on the pressed keys to determine which characters to add to the input string. Later I found out about a better way to get keyboard input, so that method is present in my later games.
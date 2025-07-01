# Spire of Light

## About

Game designed for a university group project.

The project involved ideating, prototyping, then developing a game in Unity within 16 weeks part-time. A game design document (GDD) was created first outlining the idea, prototypes, and schedule for development. The core of the game was developed within the first half, and the final weeks were devoted to refinement with three iterations of playtesting.

The game that was created is a puzzle-platformer with elements of stealth and combat, where the player's goal is to climb a tower while avoiding detection, while also achieving optional objectives for greater challenge.

All the code, art, and animation was created without the use of the Asset Store.

## Interesting sections of code

**GameDevGroupProject/Assets/Scripts/PlayerScripts/playerBehavior.cs**  
Contains a parameterized movement curve using Unity's Input System.

**GameDevGroupProject/Assets/Scripts/RobotScripts/robotBehavior.cs**  
Contains state machine and time logic controlling the patrolling movement of the enemies.

**GameDevGroupProject/Assets/Scripts/RobotScripts/robotBehavior.cs**  
Contains a dynamic mesh-creation function used for drawing enemy detection areas.

**GameDevGroupProject/Assets/Scripts/cameraMovement.cs**  
A very simple interface for smooth-dampened and bounded camera movement.

**GameDevGroupProject/Assets/Scripts/endTracker.cs**  
Defines the logic of the endscreen UI (reached on level completion), which changes based on player achievement within the level.

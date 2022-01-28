# PMUnityTest
Unity Test that involves Tools, Gameplay and Architecture. It uses Unity2018.4.

# Tasks
## 1. Editor Tools
### (Find the tool in Tools -> Object Text Configurator)
Imagine that you need to create a tool that will help to automatically configure prefabs in
the project. The tool has to consume provided file with a predefined format and apply the
configuration to GameObjects. To accomplish this task you will have to:
- Create prefabs in the Project.
- Create the tool in an Editor Window that should support:
- Search for all Prefabs that have text Component attached(In Project and
in a given directory).
- Ability to apply the configuration from the file to selected prefabs.
- Handle misconfiguration of the GameObject.
- Ability to apply or revert changes.
- In EditorTools/ directory you will find a data.json file.
## 2. Gameplay task
### (Play it in Scene 2.Gameplay.unity)
While working on the game you were asked to create daily bonus minigame, that will
request data from a server and after successful response should present it to the user.
In Gameplay/ directory you will find art assets, a reference mockup and Server.dll. The
Wheel should spin and update Player’s balance. Player’s balance should be logged in Console.
Keep in mind that this feature will be a part of the big game, that should run on iPad 2.
## 3. Design and Architecture task
### (Play it in Scene 3.Architecture.unity)
Design and architect a simple Minesweeper game using MVP.
## 4. Feedback questions
### (Find anwsers inside Assets/4.Answers.md)
- Was the test interesting?
- What was the most challenging part of the test?
- What would you like to change in the test?
- Did you have enough time to accomplish the test?

# Assignment 3: Antymology
Source for ant model: https://www.turbosquid.com/3d-models/3d-model-ant-1339233



![Ants](Images/Ants.gif)

## Current Code
My code is currently located in 3 components (found within the components folder)
1. Agents:
    - AntScript.cs
    - QueenAntScript.cs
    - GlobalVar.cs

2. Configuration
    - WorldManager.cs

3. UI
    - NumOfNestUI.cs

### Agents
AntScript: Contains the code for the ants, has functions that allow it to move, dig, and transfer health to another ant.

QueenAntScript: Contains the code for the queen ant. Has functions similar to the ants, but can also lay a nest block.

GlobalVar: Contains global variables like the Queen's Location and a boolean determining if the ants are on their first generation. True allows ants to initalise the variables used in their rulesets, but false will allow ants to change and remember changes to their ruleset variables.

### Configuration
WorldManager: Adjusted the GenerateAnts function to spawn ants and the queen ant randomly in the map. Added a function called KillOldAntsSpawnNew that destroys all ants and calls the GenerateAnts function to spawn a new generation of ants.

### UI
NumOfNestUI: Tracks the number of nest blocks in the map and updates the UI that tells how many nest blocks were made.


## How the model works



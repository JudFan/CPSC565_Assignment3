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
Also contains variables to control the learning rate and how much health Mulch blocks give.

### Configuration
WorldManager: Adjusted the GenerateAnts function to spawn ants and the queen ant randomly in the map. Added a function called KillOldAntsSpawnNew that destroys all ants and calls the GenerateAnts function to spawn a new generation of ants.

### UI
NumOfNestUI: Tracks the number of nest blocks in the map and updates the UI that tells how many nest blocks were made.


## How the model works
Parameters: 

Located in the GlobalVar script under the Sample Scene tab (upper left of the UI). Click it, go to the Inspector tab (upper right of the UI) and the adjustible parameters are :
Health Percent Mulch Gives
Ant Learning Rate

Located in the WorldManager Script under the Sample Scene tab. Click it, go to the inspector tab, and the adjustible parameter is:
MaxAnts

The ants first congregate to their queen ant via the Global Queen's Location Variable. If they have enough health to donate to the Queen, they will do so by moving to the queen's location and trading health to the queen if she needs it. Otherwise, they will stalk her. 

Once the ants lose enough health from giving it to the queen (dependant on the Min_Health_for_Transfer parameter), they will explore and look for mulch blocks to consume to return to the queen to give her health if she needs it. The amount of health given is decided by the Charity Modifier parameter

Additionally, the Queen also has a Min_Health_For_Nest paramter that decides how much health the Queen must have so she can lay a nest block. If the threshold isn't met, the Queen will also explore for Mulch blocks.

The model has GlobalVar store the parameters Min_Health_for_Transfer, Min_Health_For_Nest, and Charity_Modifier so that future Generations can inherit it. The ants and Queen of the next generations will modify these parameters to suit their goals better.


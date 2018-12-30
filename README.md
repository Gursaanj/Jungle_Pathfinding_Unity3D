# Jungle_Pathfinding_Unity3D


In this project, I will try to make a small simple game using the random platform generation as well as the navigation mesh project package from unity's github.

In which each segment of the level is baked to be a different navigation mesh with an NPC (navigation mesh agent) patrolling the area (bound to different control points) in which they can randomly set up trap for the player.
  Every time a trap is set up, a navigation obstacle is planted randomly in the scene (some location within the level) and the navigations meshes are re-baked.
  If the player triggers the trap, the player is destroyed and the level ends.
  If the player reaches the end of the level, the level is complete and the player wins

Once all the main code is completed. The navigation meshes will be turned into unity grassy terrains (using mesh materials and textures made in Blender) and the navigation mesh obstacles are transformed into jungle like obstacles like trees and vines and walls. This is to practice implementation of materials and textures within Unity.

## Start of project November 29th 2018

- [ ] The NPC (patrolling unit) has been made and is set to patrol around control points
- [ ]  the control points are set up to be spot lights
- [ ] Everytime the NPC stops at a particular patrol point (based on random probability) it triggers the spot light to turn on
- [ ]  the spot light is set up to illuminate and changing intensity and color to indicate that it is active
- [ ] the navigation obstacles have been made (a cylinder and a wall)
- [ ] the obstacles are set to instantiate every time a light has been turned on. They are set to be placed anywhere in the level
- [ ] Different (4 types) of floor models have been made

Every new update will be commented here in a new branch and recommitted to the main branch. 

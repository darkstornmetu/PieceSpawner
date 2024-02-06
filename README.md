# PieceSpawner

Piece spawner is a level generation tool for Unity. 

* It has been mainly written to be used in grid based 2D puzzle games. It has a user-friendly interface where the user can see, edit, reset the current state of the level in the editor.

* It is possible to the create the same level with different piece prefabs with different logic or the same logic with different piece appearances.

* It is also possible to edit the look in the editor to match with the chosen style.

What can be learned: 

* Custom editor window usage with a lot of different features.

* Simple usages of Property Attributes and Property Drawers.

* Detached prefab system between logic and the visuals.

* Usages of creating a generic data containers that is easily extendable.

Note: There is no gameplay code in this project; however, it can easily be integrated either using your relative classes or through inheritance from the current classes. 

# How To

1) Tools/Piece Spawner
2) Select Default Data Sets or create your own and place them in the references
3) Select your grid sizes and increments
4) Combine the preferred piece type and the color type and select the piece that you want to modify
5) Spawn the combination of pieces you have modified into the scene and select a path to save it in the project

# Example Screenshots

### Default piece set
![Spheres_Cubes_Default](https://github.com/darkstornmetu/PieceSpawner/assets/129167887/5fda4dc1-f75f-4af2-9ba2-831dc6d6bc76)
### Default piece set with alternative icons
![Spheres_Cubes_Alternate](https://github.com/darkstornmetu/PieceSpawner/assets/129167887/12b2b35c-824f-42a8-b85e-1f77ff6c3862)
### Balloon piece set with alternative icons
![Balloons_Rockets_Alternate](https://github.com/darkstornmetu/PieceSpawner/assets/129167887/7478823c-6dbc-4d1b-9451-d693c9f9d1fb)

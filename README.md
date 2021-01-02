# voxel-game-test
A Unity project to generate voxel meshes based on world data, load/unload chunk meshes around a player, and handle block interactions.

![github-small](/editor_sc.png)

- Meshes are built based on only exposed faces for efficiency.
- Raycasting from the camera perspective (and assessment of its normal) is used to identify what block is being targeted for manipulation.
- World generation uses scaled [Perlin noise](https://en.wikipedia.org/wiki/Perlin_noise) to create consistent sloped areas.

# File breakdown
## Blocks.cs
- Stores data about block types and maintains a hash table to look up their name from ID
- Also contains a static helper method for converting normal vectors into cardinal directions for debugging

## Chunks.cs
- Contains the class used to represent a chunk object (a 16 by 16 by 128 unit of world data)
- Controls the Perlin noise generation and what blocks are generated when populating the world with data

## MeshBuilder.cs
- Builds the chunk GameObjects and creates their meshes vertex by vertex, while adding the proper UV information based on block type

## Player.cs
- Checks for key and mouse input to handle item switching and block breaking/placing
- Contains initialization code and prints debugging information

## WorldLoader.cs
- Maintains a list of currently loaded chunks
- Loads chunks near the player, and unloads those farther away
- Also containers helper methods for retrieving block data at a world level

# voxel-game-test
A Unity project to generate voxel meshes based on world data, load/unload chunk meshes around a player, and handle block interactions.

![github-small](/editor_sc.png)

- Meshes are built based on only exposed faces for efficiency.
- Raycasting from the camera perspective (and assessment of its normal) is used to identify what block is being targeted for manipulation.
- World generation uses scaled [Perlin noise](https://en.wikipedia.org/wiki/Perlin_noise) to create consistent sloped areas.

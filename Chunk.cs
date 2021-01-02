using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public const int HEIGHT = 128;
    public const int WIDTH = 16;
    public int xOffset;
    public int zOffset;

    float seed;

    public GameObject objectRef;

    public int[,,] data;

    public Chunk(int x, int z, float seed) {
        this.xOffset = x;
        this.zOffset = z;
        this.seed = seed;

        data = new int[HEIGHT, WIDTH, WIDTH];

        BuildChunkData();
    }

    public void BuildChunkData() {
        //for (int y = 0; y < 128; y++) {
            for (int x = 0; x < WIDTH; x++) {
                for (int z = 0; z < WIDTH; z++) {

                    float veryLargeScale = 0.005f;
                    float largeScale = 0.04f;
                    float fineScale = 0.25f;

                    int elevation = (int)Mathf.Clamp(
                        (5f * Mathf.PerlinNoise((seed + x + xOffset * WIDTH) * veryLargeScale, (seed + z + zOffset * WIDTH) * veryLargeScale))
                        + (30f * Mathf.PerlinNoise((seed + x + xOffset * WIDTH) * largeScale, (seed + z + zOffset * WIDTH) * largeScale))
                        + (3f * Mathf.PerlinNoise((seed + x + xOffset * WIDTH) * fineScale, (seed + z + zOffset * WIDTH) * fineScale))
                        , 0, 50) + 35;

                    Biome biome = BiomeAtElevation(elevation);

                    
                    //int grassHeight = 50;
                    //Debug.Log(grassHeight);
                    int stoneHeight = elevation - 10;//(int)(Mathf.PerlinNoise(x, z) * 20f + 50);
                    for (int y = 127; y >= 0; y--) {
                        if (elevation < 45) elevation = 45;
                        if (y < stoneHeight) {
                            data[y, x, z] = Blocks.STONE;
                        } else if (y < elevation) {
                            data[y, x, z] = Blocks.DIRT;
                        } else if (y == elevation) {
                            if (biome == Biome.Plains) {
                                data[y, x, z] = Blocks.GRASS;
                            } else if (biome == Biome.Desert) {
                                data[y, x, z] = Blocks.SAND;
                            } else if (biome == Biome.Mountain) {
                                data[y, x, z] = Blocks.STONE;
                            } else if (biome == Biome.Ocean) {
                                data[y, x, z] = Blocks.WATER;
                            }
                        }
                    }
                }
            }
        //}
    }

    public enum Biome {
        Plains,
        Desert,
        Mountain,
        Ocean
    }

    public Biome BiomeAtElevation(int elevation) {
        if (elevation < 45) return Biome.Ocean;
        if (elevation < 55) return Biome.Plains;
        if (elevation < 60) return Biome.Desert;
        return Biome.Mountain;
    }

    public void GenerateChunkMesh() {
        MeshBuilder mb = new MeshBuilder();
        objectRef = mb.GenerateMesh(data, xOffset, zOffset);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks {

    public const int AIR = 0;
    public const int DIRT = 1;
    public const int GRASS = 2;
    public const int STONE = 3;
    public const int OAK = 4;
    public const int SAND = 5;
    public const int WATER = 6;
    public const int CACTUS = 7;
    public const int MAGNETITE = 8;
    public const int BAUXITE = 9;

    private static Dictionary<int, string> blockNames = new Dictionary<int, string>();

    public static string NormalToCardinal(Vector3 normal) {
        if (normal.Equals(new Vector3(1, 0, 0))) {
            return "North";
        } else if (normal.Equals(new Vector3(-1, 0, 0))) {
            return "South";
        } else if (normal.Equals(new Vector3(0, 1, 0))) {
            return "Top";
        } else if (normal.Equals(new Vector3(0, -1, 0))) {
            return "Bottom";
        } else if (normal.Equals(new Vector3(0, 0, 1))) {
            return "West";
        } else if (normal.Equals(new Vector3(0, 0, -1))) {
            return "East";
        } else {
            return "Unknown";
        }
    }

    public static void LoadBlocks() {
        blockNames.Add(AIR, "Air");
        blockNames.Add(DIRT, "Dirt");
        blockNames.Add(GRASS, "Grass");
        blockNames.Add(STONE, "Stone");
        blockNames.Add(OAK, "Oak");
        blockNames.Add(SAND, "Sand");
        blockNames.Add(WATER, "Water");
        blockNames.Add(CACTUS, "Cactus");
        blockNames.Add(MAGNETITE, "Magnetite");
        blockNames.Add(BAUXITE, "Bauxite");
    }

    public static string GetBlockName(int block) {
        try {
            return blockNames[block];
        } catch (KeyNotFoundException e) {
            return "Unknown block";
        }
    }

}

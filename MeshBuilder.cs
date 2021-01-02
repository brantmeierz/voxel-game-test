using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder {

    private List<Vector3> vertices;
    private List<Vector2> uv;
    private List<int> triangles;

    bool renderChunkBottoms = false;

    public enum Face {
        Top,
        North,
        South,
        East,
        West,
        Bottom
    }

    public GameObject GenerateMesh(int[,,] data, int xOffset, int zOffset) {

        vertices = new List<Vector3>();
        uv = new List<Vector2>();
        triangles = new List<int>();

        int[,,] northData = WorldLoader.generated.Get(xOffset + 1, zOffset).data;
        int[,,] southData = WorldLoader.generated.Get(xOffset - 1, zOffset).data;
        int[,,] westData = WorldLoader.generated.Get(xOffset, zOffset + 1).data;
        int[,,] eastData = WorldLoader.generated.Get(xOffset, zOffset - 1).data;

        for (int y = 0; y < data.GetLength(0); y++) {
            for (int x = 0; x < data.GetLength(1); x++) {
                for (int z = 0; z < data.GetLength(2); z++) {
                    //Render non-air blocks
                    if (data[y, x, z] != 0) {
                        BuildBlocksAround(data, x, y, z, Chunk.WIDTH * xOffset, Chunk.WIDTH * zOffset, northData, southData, westData, eastData);
                    }
                }
            }
        }

        Mesh chunkMesh = new Mesh();

        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.uv = uv.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        GameObject go = new GameObject("Chunk [" + xOffset + ", " + zOffset + "]", typeof(MeshFilter), typeof(MeshRenderer));
        go.transform.localScale = new Vector3(1, 1, 1);

        go.GetComponent<MeshFilter>().mesh = chunkMesh;
        go.GetComponent<MeshRenderer>().material = Resources.Load("Material/SpriteMaterial", typeof(Material)) as Material;
        go.AddComponent<MeshCollider>();

        chunkMesh.RecalculateNormals();

        return go;
    }

    void BuildBlocksAround(int[,,] data, int x, int y, int z, int xOffset, int zOffset, int[,,] northData, int[,,] southData, int[,,] westData, int[,,] eastData) {
        //Air above
        if (y == 128 || data[y + 1, x, z] == 0) {
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z + 1));
        
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 4);

            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 4);

            AddUV(data[y, x, z], Face.Top);
        }
        //Air below
        if ((y > 0 && data[y - 1, x, z] == 0) || (renderChunkBottoms && y == 0)) {
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z + 1));
        
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 4);

            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 4);

            AddUV(data[y, x, z], Face.Bottom);
        }
        //Air north
        if ((x < Chunk.WIDTH - 1 && data[y, x + 1, z] == 0) || (x == Chunk.WIDTH - 1 && northData[y, 0, z] == 0)) {
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z + 1));

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
            
            AddUV(data[y, x, z], Face.North);
        }
        //Air south
        if ((x > 0 && data[y, x - 1, z] == 0) || (x == 0 && southData[y, 15, z] == 0)) {
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z));
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z + 1));

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);

            AddUV(data[y, x, z], Face.South);
        }
        //Air east
        if ((z > 0 && data[y, x, z - 1] == 0) || (z == 0 && eastData[y, x, 15] == 0)) {
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z));
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z));
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z));

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);

            AddUV(data[y, x, z], Face.East);
        }
        //Air west
        if ((z < Chunk.WIDTH - 1 && data[y, x, z + 1] == 0) || (z == Chunk.WIDTH - 1 && westData[y, x, 0] == 0)) {
            vertices.Add(new Vector3(xOffset + x, y, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x, y + 1, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x + 1, y + 1, zOffset + z + 1));
            vertices.Add(new Vector3(xOffset + x + 1, y, zOffset + z + 1));

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);
            
            AddUV(data[y, x, z], Face.West);
        }
    }

    public void AddUV(int block, Face face) {
        if (block == Blocks.DIRT) {
            AddUVIndex(0, 0);
        } else if (block == Blocks.GRASS) {
            if (face == Face.Bottom) {
                AddUVIndex(0, 0);
            } else {
                AddUVIndex(1, 0);
            }
        } else if (block == Blocks.STONE) {
            AddUVIndex(2, 0);
        } else if (block == Blocks.OAK) {
            if (face == Face.Top || face == Face.Bottom) {
                AddUVIndex(3, 1);
            } else {
                AddUVIndex(3, 0);
            }
        } else if (block == Blocks.SAND) {
            AddUVIndex(2, 1);
        } else if (block == Blocks.WATER) {
            AddUVIndex(1, 1);
        } else if (block == Blocks.CACTUS) {
            if (face == Face.Top) {
                AddUVIndex(0, 2);
            } else {
                AddUVIndex(0, 1);
            }
        } else if (block == Blocks.MAGNETITE) {
            AddUVIndex(1, 2);
        } else if (block == Blocks.BAUXITE) {
            AddUVIndex(2, 2);
        }
    }

    private const float SIZE = 4.0f;

    public void AddUVIndex(int x, int y) {
        float top = 1 - ((float)y / SIZE + 0.002f);
        float bottom = 1 - ((float)y / SIZE + (float)1 / SIZE - 0.002f);
        float left = (float)x / SIZE + 0.002f;
        float right = (float)x / SIZE + (float)1 / SIZE - 0.002f;

        uv.Add(new Vector2(left, top));
        uv.Add(new Vector2(left, bottom));
        uv.Add(new Vector2(right, bottom));
        uv.Add(new Vector2(right, top));
    }

}

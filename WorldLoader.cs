using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLoader : MonoBehaviour {

    int currentChunkX = 0;
    int currentChunkZ = 0;

    int renderDistance = 8;

    public static float seed;

    public Text coordinateDisplay;

    public class ChunkDict {
        private Dictionary<int, Dictionary<int, Chunk>> data;

        public ChunkDict() {
            data = new Dictionary<int, Dictionary<int, Chunk>>();
        }

        public void Add(int x, int z, Chunk chunk) {
            if (!data.ContainsKey(x)) {
                data.Add(x, new Dictionary<int, Chunk>());
            }
            if (!data[x].ContainsKey(z)) {
                data[x].Add(z, chunk);
            }
        }

        public Chunk Get(int x, int z) {
            if (Contains(x, z)) {
                return data[x][z];
            }
            return null;
        }

        public void Remove(int x, int z) {
            data[x].Remove(z);
        }

        public bool Contains(int x, int z) {
            return data.ContainsKey(x) && data[x].ContainsKey(z);
        }

        public int Count() {
            int count = 0;
            Dictionary<int, Dictionary<int, Chunk>>.KeyCollection xList = data.Keys;
            int[] xListInts = new int[data.Keys.Count];
            xList.CopyTo(xListInts, 0);
            for (int i = 0; i < xListInts.Length; i++) {
                count += data[xListInts[i]].Keys.Count;
            }
            return count;
        }

        public List<Chunk> GetAllChunks() {
            List<Chunk> list = new List<Chunk>();
            foreach (Dictionary<int, Chunk> xList in data.Values) {
                foreach (Chunk chunk in xList.Values) {
                    list.Add(chunk);
                }
            }
            return list;
        }
    }
    
    public static ChunkDict loaded;
    public static ChunkDict generated;

    void Start() {
        seed = Random.Range(1000, 10000);
        
        loaded = new ChunkDict();
        generated = new ChunkDict();

        //GenerateChunksAround(0, 0);
        LoadChunksAround(0, 0);
        //GenerateChunk(0, 0);
        //LoadChunk(0, 0);
        //LoadChunk(0, 1);
        //LoadChunk(1, 1);
        //LoadChunk(1, 0);
        //LoadChunk(-1, 0);


    }

    void GenerateChunksAround(int x, int z) {
        for (int i = -renderDistance; i <= renderDistance; i++) {
            for (int j = -renderDistance; j <= renderDistance; j++) {
                if (Vector2.Distance(new Vector2(x + i, z + j), new Vector2(x, z)) < renderDistance) {
                    GenerateChunk(x + i, z + j);
                }
            }
        }
    }

    void LoadChunksAround(int x, int z) {
        for (int i = -renderDistance; i <= renderDistance; i++) {
            for (int j = -renderDistance; j <= renderDistance; j++) {
                if (Vector2.Distance(new Vector2(x + i, z + j), new Vector2(x, z)) < renderDistance) {
                    LoadChunk(x + i, z + j);
                }
            }
        }
    }

    bool ChunkGenerated(int x, int z) {
        return generated.Contains(x, z);
    }

    bool ChunkLoaded(int x, int z) {
        return loaded.Contains(x, z);
    }

    void GenerateChunk(int x, int z) {
        if (!ChunkGenerated(x, z)) {
            generated.Add(x, z, new Chunk(x, z, seed));
        }
    }

    void LoadChunk(int x, int z) {
        if (!ChunkLoaded(x, z)) {
            GenerateChunk(x, z);
            GenerateChunk(x - 1, z);
            GenerateChunk(x + 1, z);
            GenerateChunk(x, z - 1);
            GenerateChunk(x, z + 1);
            loaded.Add(x, z, generated.Get(x, z));
            generated.Get(x, z).GenerateChunkMesh();
        }
    }

    public static void ReloadChunk(int chunkX, int chunkZ) {
        Chunk chunk = loaded.Get(chunkX, chunkZ);
        Destroy(chunk.objectRef);
        chunk.GenerateChunkMesh();
    }

    void UnloadChunksAround(int x, int z) {
        List<Chunk> chunks = loaded.GetAllChunks();
        for (int i = 0; i < chunks.Count; i++) {
            //Debug.Log(i);
            if (Vector2.Distance(new Vector2(chunks[i].xOffset, chunks[i].zOffset), new Vector2(x, z)) >= renderDistance) {
                //Debug.Log("Removing " + chunks[i].objectRef.name);
                Destroy(chunks[i].objectRef);
                loaded.Remove(chunks[i].xOffset, chunks[i].zOffset);
            }
        }
    }

    void Update() {
        int x = (int)gameObject.transform.position.x;
        int y = (int)gameObject.transform.position.y;
        int z = (int)gameObject.transform.position.z;
        int chunkX = (int)Mathf.Floor(gameObject.transform.position.x / Chunk.WIDTH);
        int chunkZ = (int)Mathf.Floor(gameObject.transform.position.z / Chunk.WIDTH);
        coordinateDisplay.text = ("Player coordinates (X:" + x + " Y:" + y + " Z:" + z + ") Chunk: (X:" + chunkX + " Z:" + chunkZ + ")");

        if (currentChunkX != chunkX || currentChunkZ != chunkZ) {
            //Update visible chunks
            //Debug.Log("New Chunk: ");
            
            currentChunkX = chunkX;
            currentChunkZ = chunkZ;
            GenerateChunksAround(currentChunkX, currentChunkZ);
            LoadChunksAround(currentChunkX, currentChunkZ);
            UnloadChunksAround(currentChunkX, currentChunkZ);

            //Debug.Log("Loaded: " + loadedChunks.Count);
            //Debug.Log("Generated: " + generatedChunks.Count);
        }
    }

    public static void SetBlockAt(int x, int y, int z, int type) {
        int chunkX = (int)Mathf.Floor((float)x / Chunk.WIDTH);
        int chunkZ = (int)Mathf.Floor((float)z / Chunk.WIDTH);
        Chunk chunk; 
        if ((chunk = generated.Get(chunkX, chunkZ)) == null) {
            chunk = new Chunk(chunkX, chunkZ, seed);
            generated.Add(chunkX, chunkZ, chunk);
        }
        int adjX = x - 16 * chunkX;
        int adjZ = z - 16 * chunkZ;
        chunk.data[y, adjX, adjZ] = type;
        ReloadChunk(chunkX, chunkZ);
        if (adjX == 0) ReloadChunk(chunkX - 1, chunkZ);
        if (adjX == 15) ReloadChunk(chunkX + 1, chunkZ);
        if (adjZ == 0) ReloadChunk(chunkX, chunkZ - 1);
        if (adjZ == 15) ReloadChunk(chunkX, chunkZ + 1);
    }

    public static int GetBlockAt(int x, int y, int z) {
        int chunkX = (int)Mathf.Floor((float)x / Chunk.WIDTH);
        int chunkZ = (int)Mathf.Floor((float)z / Chunk.WIDTH);
        Chunk chunk; 
        if ((chunk = generated.Get(chunkX, chunkZ)) == null) {
            chunk = new Chunk(chunkX, chunkZ, seed);
            generated.Add(chunkX, chunkZ, chunk);
        }
        return chunk.data[y, x - 16 * chunkX, z - 16 * chunkZ];
    }

    /*public static int GetWorldBlockData(int x, int y, int z) {
        int chunkX = (int)Mathf.Floor((float)x / Chunk.WIDTH);
        int chunkZ = (int)Mathf.Floor((float)z / Chunk.WIDTH);
        Chunk chunk; 
        if ((chunk = generated.Get(chunkX, chunkZ)) == null) {
            chunk = new Chunk(chunkX, chunkZ, seed);
            generated.Add(chunkX, chunkZ, chunk);
        }
        return chunk.data[y, x - 16 * chunkX, z - 16 * chunkZ];
        //return chunk.data[y, Mathf.Abs(x % 16), Mathf.Abs(z % 16)];
        //return chunk.data[y, x < 0 ? 15 - (-x % 16) : (x % 16), z < 0 ? 15 - (-z % 16) : (z % 16)];
        //for (int i = 0; i < loadedChunks.Count; i++) {
        //    if (loadedChunks[i].xOffset == chunkX && loadedChunks[i].zOffset == chunkZ) {
        //        return loadedChunks[i].data[y, Mathf.Abs(x) % Chunk.WIDTH, Mathf.Abs(z) % Chunk.WIDTH];
        //    }
       // }
    }*/

}

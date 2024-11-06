using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject[] groundTilePrefabs;    
    public Transform player;              
    public float spawnDistance = 50f;      
    public int initialPoolSize = 20;       

    private List<GameObject> groundTilePool;
    private float tileWidth;              
    private float nextSpawnX;            

    private void Start()
    {

        groundTilePool = new List<GameObject>();
        nextSpawnX = player.position.x;

        foreach (var groundTile in groundTilePrefabs)
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject tile = Instantiate(groundTile, transform);
                tile.SetActive(false);
                groundTilePool.Add(tile);
            }
        }


        for (int i = 0; i < initialPoolSize / 2; i++)
        {
            GameObject tile = groundTilePool[i];
            tile.transform.position = new Vector3(nextSpawnX, 0, 0);
            tile.SetActive(true);

            Tilemap tilemap = tile.GetComponent<Tilemap>();
            tileWidth = tilemap.size.x;
            nextSpawnX += tileWidth;
        }
    }

    private void Update()
    {
        if (player.position.x + spawnDistance > nextSpawnX)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        GameObject tile = groundTilePool[Random.Range(0, groundTilePool.Count)];
        while (tile.activeSelf) {
            if (tile.gameObject.transform.position.x < player.position.x - 10f) {
                tile.SetActive(false);
            }
            else
            {
                tile = groundTilePool[Random.Range(0, groundTilePool.Count)];
            }
        }

        tile.transform.position = new Vector3(nextSpawnX, 0, 0);
        tile.SetActive(true);

        Tilemap tilemap = tile.GetComponent<Tilemap>();
        tileWidth = tilemap.size.x;
        nextSpawnX += tileWidth;
    }

}

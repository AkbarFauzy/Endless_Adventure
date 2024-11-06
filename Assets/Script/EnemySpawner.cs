using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform playerTransform;
    
    private List<GameObject> enemyPool;

    public float TimeUntilEnemySpawn = 2f;
    private float _timeSinceLastSpawn;

    private void Start()
    {
        enemyPool = new List<GameObject>();

        foreach (var enemy in enemyPrefabs)
        {
            for (int i =0;i<5;i++)
            {
                var temp = Instantiate(enemy);
                temp.GetComponent<Enemy>().SetSpawner(this);
                temp.SetActive(false);
                enemyPool.Add(temp);
            }
        }
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (enemyPool.Count > 0 && _timeSinceLastSpawn > TimeUntilEnemySpawn)
        {
            var enemy = enemyPool[Random.Range(0, enemyPool.Count)];
            enemy.transform.position = new Vector3(playerTransform.position.x + 50f, 0, 0);

            enemy.SetActive(true);
            enemyPool.Remove(enemy);
            _timeSinceLastSpawn = 0f;
            TimeUntilEnemySpawn = Random.Range(2f, 5f);
        }
    }

    public void AddEnemyToPool(Enemy enemy)
    {
        enemyPool.Add(enemy.gameObject);
        enemy.gameObject.SetActive(false);
    }

}

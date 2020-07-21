using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private GameObject spawner;
    private Vector2 spawnerLocation;
    private Timer fireTimer;

    private int enemyHealth = 3000;
    
    GameObject[] enemySpawns = new GameObject[3];

    Vector2[] enemySpawnLocations = new Vector2[3];

    private int enemyCount = 500;

    // Start is called before the first frame update
    public void Start()
    {
        enemySpawns = GameObject.FindGameObjectsWithTag("Respawn");
        
        spawnerLocation = spawner.transform.position;

        for (int i = 0; i < enemySpawns.Length; i++)
        {
            enemySpawnLocations[i] = enemySpawns[i].transform.position;
        }
        
        fireTimer = gameObject.AddComponent<Timer>();
        rb2d = GetComponent<Rigidbody2D>();
        fireTimer.Duration = 0.1f;
        fireTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer.Finished)
        {
            spawnerLocation = spawner.transform.position;
            Fire();
            fireTimer.Run();
        }
        
        if (enemyCount == 0)
        {
            SceneManager.LoadScene("EndScene1");
        }
    }

    private void FixedUpdate()
    {
        StartMoving();
    }

    void Fire()
    {
        GameObject bullet = ObjectPool.GetBullet();
        if(bullet!=null)
        {
            bullet.transform.position = spawnerLocation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().ownerIsPlayer = false;
            bullet.GetComponent<Bullet>().StartMoving();
        }
    }
    
    private void LaunchEnemyTroops()
    {
        GameObject enemy = ObjectPool.GetEnemy();
        enemy.transform.position = enemySpawnLocations[Random.Range(0, 2)];
        if(!enemy.activeInHierarchy)
            enemy.SetActive(true);
    }
    
    private void StartMoving()
    {
        transform.Translate(Vector3.left * Time.fixedDeltaTime * -1);

        if (transform.position.x > Screen.width / 2)
        {
            LaunchEnemyTroops();
        }
        
    }
    
    public void StopMoving()
    {
        rb2d.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("bullet")) return;
        if (other.gameObject.GetComponent<Bullet>().ownerIsPlayer)
        {
            ObjectPool.ReturnBulletToPool(other.gameObject);

            if (enemyHealth <= 0)
            {
                enemyCount--;
                enemyHealth = 0;
                ObjectPool.ReturnEnemyToPool(gameObject);
                LaunchEnemyTroops();
            }
            else
            {
                enemyHealth -= Random.Range(5,10);
            }
        }
        else
        {
            other.gameObject.GetComponent<Bullet>().transform.position *= -1;
        }
    }

    private void OnBecameInvisible()
    {
        ObjectPool.ReturnEnemyToPool(gameObject);
        LaunchEnemyTroops();
    }
}

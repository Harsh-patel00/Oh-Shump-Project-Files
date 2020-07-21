using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
	private static GameObject bulletPrefab;
	private static List<GameObject> bulletPool;

	private static GameObject enemyPrefab;
	private static List<GameObject> enemyPool;
	
	public static void Initialize()
	{
		// Create a bullet pool
		bulletPrefab = Resources.Load<GameObject>("Bullet");
		bulletPool = new List<GameObject>(25);
		for (int i = 0; i < bulletPool.Capacity; i++)
		{
			bulletPool.Add(GetNewBullet());
		}
		
		// Create an enemy pool
		enemyPrefab = Resources.Load<GameObject>("Enemy");
		enemyPool = new List<GameObject>(4);
		for (int j = 0; j < enemyPool.Capacity; j++)
		{
			enemyPool.Add(GetNewEnemy());
		}
	}

	public static GameObject GetBullet()
	{
		if (bulletPool.Count > 0)
		{
			GameObject bullet = bulletPool[bulletPool.Count - 1];
			bulletPool.RemoveAt(bulletPool.Count - 1);
			return bullet;
		}
		else
		{
			bulletPool.Capacity++;
			return GetNewBullet();
		}
	}

	public static void ReturnBulletToPool(GameObject bullet)
	{
		bullet.SetActive(false);
		bullet.GetComponent<Bullet>().StopMoving();
		bulletPool.Add(bullet);
	}

	static GameObject GetNewBullet()
	{
		GameObject bullets = GameObject.Instantiate(bulletPrefab);
		bullets.GetComponent<Bullet>().Initialize();
		bullets.SetActive(false);
		//GameObject.DontDestroyOnLoad(bullets);
		return bullets;
	}
	
	public static GameObject GetEnemy()
	{
		if (enemyPool.Count > 0)
		{
			GameObject enemy = enemyPool[enemyPool.Count - 1];
			enemyPool.RemoveAt(enemyPool.Count - 1);
			return enemy;
		}
		else
		{
			enemyPool.Capacity++;
			return GetNewEnemy();
		}
	}

	public static void ReturnEnemyToPool(GameObject enemy)
	{
		enemy.SetActive(false);
		enemy.GetComponent<Enemy>().StopMoving();
		enemyPool.Add(enemy);
	}

	static GameObject GetNewEnemy()
	{
		GameObject enemies = GameObject.Instantiate(enemyPrefab);
		enemies.GetComponent<Enemy>().Start();
		enemies.SetActive(false);
		return enemies;
	}
	
	public static void DestoryAllBullets()
	{
		foreach(var bullet in bulletPool)
			GameObject.Destroy(bullet);
	}
}

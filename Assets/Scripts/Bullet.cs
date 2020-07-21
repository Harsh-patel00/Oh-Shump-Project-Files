using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public bool ownerIsPlayer;
    
    public void Initialize()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void StartMoving()
    {
        if(ownerIsPlayer)
        {
            rb2d.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
        }
        else
        {
            rb2d.AddForce( new Vector2(-5f, Random.Range(0,5)) , ForceMode2D.Impulse);
        }
    }

    public void StopMoving()
    {
        rb2d.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ObjectPool.ReturnBulletToPool(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        ObjectPool.ReturnBulletToPool(gameObject);
    }
}

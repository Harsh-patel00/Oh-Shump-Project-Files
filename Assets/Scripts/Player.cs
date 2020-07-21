using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject spawner;

    private Vector2 spawnLocation;
    
    private Rigidbody2D rb2d;

    private int playerHealth = 2000;

    private Text playerHealthText;

    private Vector2 moveDirection;
    
    // Start is called before the first frame update
    public void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerHealthText = FindObjectOfType<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(0f, Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
        {
            spawnLocation = spawner.transform.position;
            Fire();
        }
        
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    void MovePlayer(Vector2 moveDirection)
    {
        rb2d.MovePosition((Vector2)transform.position + (moveDirection * moveSpeed * Time.fixedDeltaTime));
    }
    void Fire()
    {
        GameObject bullet = ObjectPool.GetBullet();
        bullet.transform.position = spawnLocation;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().ownerIsPlayer = true;
        bullet.GetComponent<Bullet>().StartMoving();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("bullet"))
        {
            if (!other.gameObject.GetComponent<Bullet>().ownerIsPlayer)
            {
                ObjectPool.ReturnBulletToPool(other.gameObject);
                playerHealth = (playerHealth <= 0) ? 0 : playerHealth - 15;
                playerHealthText.text = "Player Health: " + playerHealth;
            }
            else
            {
                other.gameObject.GetComponent<Bullet>().transform.position *= -1;
            }
        }
    }
}

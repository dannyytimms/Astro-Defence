using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float horizontalSpeed = 0.5f;
    public float verticalSpeed = 0.5f;

    public float leftBounds = -2.0f, rightBounds = 2.0f;
    public float minVerticalTargetHeight = 1.0f; //how high we want the enemy to sit on the screen
    public float maxVerticalTargetHeight = 2.5f; //how high we want the enemy to sit on the screen

    public Transform bulletSpawnTransform;

    public bool weaponsActive = true;
    public float fireRate = 0.5f;


    private bool isAlive = false;
    private float elapsedTime = 0.0f;
    private bool moveLeft = true; //we will start moving to the left first.

    private void Start()
    {
        HandleSpawn();
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
            elapsedTime += Time.deltaTime;

        //if (elapsedTime >= bulletLifetime)
        //{
        //    EndLife();
        //}
        //while (transform.position.y > verticalHeight)
        //{
        //    HandleVerticalMovement();
        //}

    }

    private void HandleSpawn()
    {
        isAlive = true;

        StartCoroutine(HandleWeaponsSystem());

        StartCoroutine(HandleVerticalMovement());
    }

    private void HandleDeath()
    {
        isAlive = false;
        gameObject.SetActive(false);
        elapsedTime = 0.0f; //we need to remember to reset the timer whilst using object pooling.
    }

    private float GenerateEnemyYTarget()
    {
        Random.seed = System.Environment.TickCount;
        return Random.Range(minVerticalTargetHeight, maxVerticalTargetHeight);
    }

    private float GetEnemyXPos()
    {
        return transform.position.x;
    }

    private IEnumerator HandleVerticalMovement()
    {
        while (transform.position.y >= GenerateEnemyYTarget())
        {
            gameObject.transform.Translate(-Vector3.up * (verticalSpeed * Time.deltaTime));
            yield return null;            
        }
        StartCoroutine(HandleHorizontalMovement());
    }

    private IEnumerator HandleHorizontalMovement()
    {
        while (isAlive)
        {
            if(moveLeft)
                gameObject.transform.Translate(-Vector3.right * (horizontalSpeed * Time.deltaTime));
            else
                gameObject.transform.Translate(Vector3.right * (horizontalSpeed * Time.deltaTime));
            if (GetEnemyXPos() >= rightBounds)
                moveLeft = true;
            else if (GetEnemyXPos() <= leftBounds)
                moveLeft = false;

            yield return null;
        }
    }

    private IEnumerator HandleWeaponsSystem()
    {
        while (weaponsActive)
        {
            yield return new WaitForSeconds(fireRate);
            FireBullet();
        }
    }

    private void FireBullet()
    {

    }

}

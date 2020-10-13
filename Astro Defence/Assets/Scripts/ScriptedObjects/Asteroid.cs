using System;
using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float asteroidLifetime = 2.0f;

    [Space(10)]
    public Vector3 startPos, endPos;
    [Space(10)]

    private Animator animator;
    private BoxCollider2D col;
    private Rigidbody2D rb;

    private float elapsedTime = 0.0f;

    bool canFollowPath = true;
    bool isDestructable; //don't want to destroy this object if it isn't even visible!
    bool isAlive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnBecameVisible()
    {
        isDestructable = true;
    }

    private void OnBecameInvisible()
    {
        isDestructable = false;
    }

    void OnEnable()
    {
        Tuple<Vector3, Vector3> positions = Obstacle_Manager.SharedInstance.GetAsteroidStartEndPositions();
        transform.position = positions.Item1;
        startPos = transform.position;// we don't necessarily need to set this, but we can use this value to debug in the editor if something doesn't look right.
        endPos = positions.Item2;

        canFollowPath = true;
        isAlive = true;
    }

    void Update()
    {
        if (!isAlive) //if the object is dead or inactive we don't want to make these calculations
            return;

        HandleAsteroidLifetime();
    }

    private void HandleAsteroidLifetime()
    {
        if (gameObject.activeInHierarchy)
            elapsedTime += Time.deltaTime;

        if (elapsedTime >= asteroidLifetime)
        {
            EndLife();
        }

        if (canFollowPath && rb.velocity.magnitude <= 2.5f)
        {
            rb.AddForce(endPos * 2.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle") //this gives our obstacles a cool bounce effect!
        {
            canFollowPath = false;
            return;
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            if (!isDestructable)
                return;

            EndLife(true);
        }
        else
        {
            EndLife(false);
        }
    }

    public void EndLife(bool destroyedByBullet = false)
    {
        animator.SetTrigger("Start_Explosion");
        Audio_Manager.SharedInstance.PlaySoundEffect("Explosion");
        col.enabled = false;

        if (destroyedByBullet)                    
            GUI_Manager.SharedInstance.AddScore();        

        StartCoroutine(EndLifeSequence());
    }

    private IEnumerator EndLifeSequence()
    {
        yield return new WaitForSecondsRealtime(2); //give the explosion time to finish

        col.enabled = true;//we need to remember to reset the object whilst using object pooling.
        gameObject.SetActive(false);
        elapsedTime = 0.0f;
        isAlive = false;
    }
}

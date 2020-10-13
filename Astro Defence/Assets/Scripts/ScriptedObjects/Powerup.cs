using System.Collections;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float powerupLifetime = 5.0f;

    [Space(10)]
    public Vector3 startPos, endPos;
    [Space(10)]

    public PowerupType powerupType;

    private Animator animator;
    private BoxCollider2D col;

    private float elapsedTime = 0.0f;

    bool canFollowPath = true;
    bool isDestructable = false; //don't want to destroy this if it isn't visible!
    bool isAlive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
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
        System.Tuple<Vector2, Vector2> positions = Obstacle_Manager.SharedInstance.GetPowerupSpawnPosition();
        transform.position = positions.Item1;
        startPos = transform.position;// we don't necessarily need these, but we can use this value to debug in the editor if something doesn't look right.
        endPos = positions.Item2;

        transform.rotation = new Quaternion(0, 0, 0, 0);

        canFollowPath = true;
        isAlive = true;
    }

    void Update()
    {
        if (!isAlive) //if the object is dead or inactive we don't want to make these calculations
            return;

        HandlePowerupLifetime();
    }

    private void HandlePowerupLifetime()
    {
        if (gameObject.activeInHierarchy)
            elapsedTime += Time.deltaTime;

        if (elapsedTime >= powerupLifetime)
        {
            EndLife();
        }

        if (canFollowPath)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, movementSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDestructable)
            return;

        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Player")
        {
            if (powerupType == PowerupType.Spray)
                PlayerWeaponSystem.SharedInstance.ApplySpray();
            else if (powerupType == PowerupType.Shield)
                PlayerBehaviour.SharedInstance.ApplyImmunity();

            canFollowPath = false;
            EndLife(collision.gameObject.tag == "Bullet" ? true : false);
        }
    }

    public void EndLife(bool destroyedByBullet = false)
    {
        animator.SetTrigger("Start_Explosion");
        Audio_Manager.SharedInstance.PlaySoundEffect("Powerup");
        col.enabled = false;

        if (destroyedByBullet)
            GUI_Manager.SharedInstance.AddScore(10);

        StartCoroutine(EndLifeCoroutine());
    }

    private IEnumerator EndLifeCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f); //give the explosion time to finish

        col.enabled = true;//we need to remember to reset the object whilst using object pooling.
        gameObject.SetActive(false);
        elapsedTime = 0.0f;
        isAlive = false;
    }
}

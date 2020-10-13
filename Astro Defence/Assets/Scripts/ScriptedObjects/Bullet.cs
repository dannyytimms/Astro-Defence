using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float bulletLifetime = 2.0f;

    private float elapsedTime = 0.0f;

    void Update()
    {
        if(gameObject.activeInHierarchy)
            elapsedTime += Time.deltaTime;

        if(elapsedTime >= bulletLifetime)
        {
            EndLife();
        }

        gameObject.transform.Translate(Vector3.up * (movementSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D()
    {
        EndLife();
    }

    private void EndLife()
    {
        gameObject.SetActive(false);
        elapsedTime = 0.0f; //we need to remember to reset the bullet timer whilst using object pooling.
    }
}

using System.Collections;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    public static PlayerWeaponSystem SharedInstance;

    [Tooltip("How often the bullets will fire (in seconds)")]
    public float fireRate = 0.5f;

    [Tooltip("Should the guns fire on start?")]
    public bool weaponsActive = true;

    [Tooltip("Is the player using the spray powerup?")]
    public bool isSpraying = false;
    [Tooltip("How long should the spray powerup last for?")]
    public float sprayTime = 5.0f;

    private Transform spawnLocation; // We need this transform so the bullets fire from the front of the ship rather than the center

    private void Awake()
    {
        SharedInstance = this;
        spawnLocation = GetComponentInChildren<Transform>();
    }

    private void Start()
    {
        StartCoroutine(FireCannons());
    }

    private IEnumerator FireCannons()
    {
        while (weaponsActive)
        {
            yield return new WaitForSeconds(fireRate);

            if (PlayerBehaviour.SharedInstance.isDead) //we don't want to keep firing the cannons if the player is dead...
            {
                weaponsActive = false;
                yield return null;
            }

            if (isSpraying)
                FireMultiple();
            else
                FireRound();
        }
    }

    private void FireRound()
    {
        if (!weaponsActive)
            return;

        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("PF_Gunshot");

        if(bullet != null)
        {
            bullet.transform.position = spawnLocation.position;
            bullet.transform.eulerAngles = new Vector3(0, 0, 0);
            bullet.SetActive(true);
        }
    }

    private void FireMultiple()
    {
        if (!weaponsActive)
            return;

        for(int i = 0; i < 3; i++)
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("PF_Gunshot");

            if (bullet != null)
            {
                bullet.transform.position = spawnLocation.position;

                switch (i)
                {
                    case 0:
                        bullet.transform.eulerAngles = new Vector3(0, 0, -35);
                        break;
                    case 1:
                        bullet.transform.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case 2:
                        bullet.transform.eulerAngles = new Vector3(0, 0, 35);
                        break;
                }
                bullet.SetActive(true);
            }
        }
    }    

    public void ApplySpray()
    {
        StartCoroutine(ApplySprayEffects());
    }

    private IEnumerator ApplySprayEffects()
    {
        isSpraying = true;

        float elapsedTime = 0.0f;

        while (elapsedTime <= sprayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isSpraying = false;
    }
}

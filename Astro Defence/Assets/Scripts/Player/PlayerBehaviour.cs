using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    public SpriteRenderer shieldRenderer;

    public static PlayerBehaviour SharedInstance;

    private bool isFlashing;

    public bool isDead = false;
    public bool isImmune;
    public float immunityTime = 5.0f;

    private void Awake()
    {
        SharedInstance = this;
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isImmune) //We cannot die
            return;

        if(collision.gameObject.tag == "Obstacle")
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GUI_Manager.SharedInstance.RemoveLife();
        if (GUI_Manager.SharedInstance.lives > 0)
        {
            ResetPlayerPosition();
            FlashPlayer();
        }
        else
        {
            isDead = true;
            Leaderboard_Manager.SharedInstance.AddToLeaderboard(GUI_Manager.SharedInstance.GetScore());
            ObjectPooler.SharedInstance.ResetSceneObjects();
            DestroyPlayerObj();
        }
    }

    private void DestroyPlayerObj()
    {
        playerRenderer.enabled = false;
        DisplayDeathMessage();
    }

    private void DisplayDeathMessage()
    {
        GUI_Manager.SharedInstance.DisplayMenu("Death");
    }

    private void FlashPlayer()
    {
        if (isFlashing)
            return;

        StartCoroutine(FlashPlayerSequence());
    }

    private IEnumerator FlashPlayerSequence()
    {
        isFlashing = true;

        int i = 0;
        while (i < 10)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            i++;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        isFlashing = false;
    }

    private void FlashShield()
    {
        if (isFlashing)
            return;

        StartCoroutine(FlashShieldSequence());
    }

    private IEnumerator FlashShieldSequence()
    {
        isFlashing = true;

        int i = 0;
        while (i < 10)
        {
            shieldRenderer.enabled = !shieldRenderer.enabled;
            i++;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        shieldRenderer.enabled = false;

        isFlashing = false;
    }

    private void ResetPlayerPosition()
    {
        transform.position = new Vector3(0, -4, 0);
    }

    public void ApplyImmunity()
    {
        StartCoroutine(ApplyImmunityEffects());
    }

    private IEnumerator ApplyImmunityEffects()
    {
        isImmune = true;

        float elapsedTime = 0.0f;
        shieldRenderer.enabled = true;

        while(elapsedTime <= immunityTime)
        {
            elapsedTime += Time.deltaTime;

            if(immunityTime - elapsedTime <= 2)
            {
                FlashShield();
            }

            yield return new WaitForEndOfFrame();
        }

        isImmune = false;
    }


}

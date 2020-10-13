using System.Collections;
using UnityEngine;

public enum ObstacleSidePortion { Left, Right, Top } //the respective half of the side the obstacle is spawned at
public enum PowerupType { Spray, Shield } //what does the powerup do? By the way, we class powerups as obstacles. 

public class Obstacle_Manager : MonoBehaviour
{
    public static Obstacle_Manager SharedInstance;

    public bool shouldObstaclesSpawn = true;
    public bool shouldPowerupsSpawn = true;

    [Space(10)]
    public float asteroidSpawnTime = 1.0f;

    [Space(10)]
    public float offScreenOffset = 1.0f; /*we don't just want the object to spawn on the edge of the screen, 
    we need it to spawn outside the screen with a maximum distance away from the bounds of the screen so it's not travelling forever.*/

    [Tooltip("We will get a time somewhere between the min and the max so the player isnt always using powerups!")]
    public float powerupMinSpawnTime = 30.0f;
    public float powerupMaxSpawnTime = 60.0f;

    private ObstacleSidePortion obstacleStartingSide;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnObstacle());
        StartCoroutine(SpawnPowerup());
    }

    private IEnumerator SpawnObstacle()
    {
        while (shouldObstaclesSpawn)
        {
            yield return new WaitForSeconds(asteroidSpawnTime);
            GameObject asteroid = ObjectPooler.SharedInstance.GetPooledObject("PF_Asteroid");

            if (asteroid != null)
            {
                asteroid.SetActive(true);
            }
        }
    }

    private IEnumerator SpawnPowerup()
    {
        while (shouldPowerupsSpawn)
        {
            Random.seed = System.Environment.TickCount;
            yield return new WaitForSeconds(Random.Range(powerupMinSpawnTime, powerupMaxSpawnTime));

            if (!PlayerBehaviour.SharedInstance.isImmune) //Only spawn additional powerups if the player is not immune anymore.
            {
                GameObject powerup = GetRandomPowerup();

                if (powerup != null)
                {
                    powerup.SetActive(true);
                }
            }
        }
    }

    private GameObject GetRandomPowerup()
    {
        Random.seed = System.Environment.TickCount;
        int random = Random.Range(1, 3);

        if(random == 1)
            return ObjectPooler.SharedInstance.GetPooledObject("PF_ShieldPowerup");
        else
            return ObjectPooler.SharedInstance.GetPooledObject("PF_SprayPowerup");
    }

    public System.Tuple<Vector2, Vector2> GetPowerupSpawnPosition()
    {
        Vector2 startPos = new Vector2(Random.Range(-2, 2), //Having some issues with getting the correct canvas bounds, implementing this work around in the interest of time.
            GUI_Manager.SharedInstance.TopLeftCorner.y + offScreenOffset);

        Vector2 endPos = new Vector2(startPos.x, -startPos.y); //fall vertically

        return System.Tuple.Create(startPos, endPos);
    }

    public System.Tuple<Vector3, Vector3> GetAsteroidStartEndPositions()
    {
        return System.Tuple.Create(GetAsteroidStartPos(), GetAsteroidEndPos());
    }

    private Vector3 GetAsteroidStartPos()
    {
        Random.seed = System.Environment.TickCount;
        int sideOfScreen = Random.Range(1, 4);

        switch (sideOfScreen)
        {
            case 1: //Left
                obstacleStartingSide = ObstacleSidePortion.Left;
                return new Vector3(GUI_Manager.SharedInstance.BottomLeftCorner.x - offScreenOffset, Random.Range(GUI_Manager.SharedInstance.BottomLeftCorner.y,
                    GUI_Manager.SharedInstance.TopLeftCorner.y), 0);
            case 2: //Right
                obstacleStartingSide = ObstacleSidePortion.Right;
                return new Vector3(GUI_Manager.SharedInstance.TopRightCorner.x + offScreenOffset, Random.Range(GUI_Manager.SharedInstance.TopRightCorner.y,
                    GUI_Manager.SharedInstance.BottomRightCorner.y), 0);
            case 3: //Top
                obstacleStartingSide = ObstacleSidePortion.Top;
                return new Vector3(Random.Range(GUI_Manager.SharedInstance.TopLeftCorner.x, GUI_Manager.SharedInstance.TopRightCorner.x),
                    GUI_Manager.SharedInstance.TopLeftCorner.y + offScreenOffset, 0);
        }
        return Vector3.zero;
    }

    private Vector3 GetAsteroidEndPos()
    {
        Random.seed = System.Environment.TickCount;

        switch (obstacleStartingSide)
        {
            case ObstacleSidePortion.Left:
                return new Vector3(GUI_Manager.SharedInstance.BottomRightCorner.x + offScreenOffset, Random.Range(GUI_Manager.SharedInstance.TopRightCorner.y,
                    GUI_Manager.SharedInstance.BottomRightCorner.y), 0);
            case ObstacleSidePortion.Right:
                return new Vector3(GUI_Manager.SharedInstance.BottomLeftCorner.x - offScreenOffset, Random.Range(GUI_Manager.SharedInstance.BottomLeftCorner.y,
                    GUI_Manager.SharedInstance.TopLeftCorner.y), 0);
            case ObstacleSidePortion.Top:
                return new Vector3(Random.Range(GUI_Manager.SharedInstance.BottomLeftCorner.x, GUI_Manager.SharedInstance.BottomRightCorner.x),
                    GUI_Manager.SharedInstance.BottomLeftCorner.y - offScreenOffset, 0);
        }
        return Vector3.zero;
    }
}

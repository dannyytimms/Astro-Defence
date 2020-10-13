using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    [Tooltip("How fast we want the player to move to the new position. (On DEVICE)")]
    public float swipeMovementSpeed = 0.01f;

    [Tooltip("How fast we want the player to move to the new position. (On DESKTOP)")]
    public float desktopMovementSpeed = 5.0f;

    [Tooltip("The min and max bounds of movement so our player doesn't leave the screen.")]
    public float playerBoundsLeft = -2;
    public float playerBoundsRight = 2;

    private GameObject playerObject;
    private Vector3 firstTouchPos;
    private Vector2 lastTouchPos;
    private float minDragDistance;

    private void Awake()
    {
        if (playerObject == null) playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) Debug.Log("Couldn't get playerObject");
    }

    void Start()
    {
        minDragDistance = Screen.width * 15 / 100;// 15% width of the screen considering we are only swiping left and right
    }

    private float PlayerXPos()
    {
        return playerObject.transform.position.x;
    }

    private float PlayerYPos()
    {
        return playerObject.transform.position.y;
    }

    void Update()
    {      
        if (Input.touchCount > 0)
            HandleTouchInput();
        if (Input.anyKey)
            HandleEditorInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1) //single touch
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                firstTouchPos = touch.position;
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position
            {
                lastTouchPos = touch.position;

                if (Mathf.Abs(lastTouchPos.x - firstTouchPos.x) > minDragDistance || Mathf.Abs(lastTouchPos.y - firstTouchPos.y) > minDragDistance)
                {
                    if (Mathf.Abs(lastTouchPos.x - firstTouchPos.x) > Mathf.Abs(lastTouchPos.y - firstTouchPos.y))
                    {
                        if (lastTouchPos.x > firstTouchPos.x)
                        {   //Right swipe
                            if (PlayerXPos() <= playerBoundsRight)
                            {
                                playerObject.transform.Translate(new Vector2(touch.deltaPosition.x, 0) * (Time.deltaTime * swipeMovementSpeed));
                                if (PlayerXPos() > playerBoundsRight)
                                    playerObject.transform.position = new Vector3(playerBoundsRight - 0.1f, PlayerYPos(), 0);
                            }
                        }
                        else
                        {   //Left swipe
                            if (PlayerXPos() >= playerBoundsLeft)
                            {
                                playerObject.transform.Translate(new Vector2(touch.deltaPosition.x, 0) * (Time.deltaTime * swipeMovementSpeed));
                                if (PlayerXPos() < playerBoundsLeft)
                                    playerObject.transform.position = new Vector3(playerBoundsLeft + 0.1f, PlayerYPos(), 0);
                            }
                        }
                    }
                }
            }
        }
    }

    void HandleEditorInput() // note: this won't work correctly if a device is plugged in with Unity Remote open.
    {
        if (Input.GetKey(KeyCode.LeftArrow) && PlayerXPos() > playerBoundsLeft)
        {
            playerObject.transform.Translate(-Vector3.right * (Time.deltaTime * desktopMovementSpeed));
        }
        if (Input.GetKey(KeyCode.RightArrow) && PlayerXPos() < playerBoundsRight)
        {
            playerObject.transform.Translate(Vector3.right * (Time.deltaTime * desktopMovementSpeed));
        }
    } 
}

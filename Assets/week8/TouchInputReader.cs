using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputReader : MonoBehaviour
{
    private InputTime inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [SerializeField] private float minSwipDistane;

    [SerializeField] private Playermovement Playermovement;

    [SerializeField] private Transform swipeTrailOBJ;
    [SerializeField] private TrailRenderer swipeTrail;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        inputActions = new InputTime();
        if (mainCamera != null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Start()
    {
        if (swipeTrail != null)
        {
            swipeTrail.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Touch.PrimaryContact.started += OnTouchStarted;
        inputActions.Touch.PrimaryContact.canceled += OnTouchEnded;
    }

    private void OnDisable()
    {
        inputActions.Touch.PrimaryContact.started -= OnTouchStarted;
        inputActions.Touch.PrimaryContact.canceled -= OnTouchEnded;

        inputActions.Disable();
    }
    
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        startTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
        //Debug.Log("Start Position: " + startTouchPosition);
        if (swipeTrail != null)
        {
            swipeTrail.Clear();

        }

        if (swipeTrailOBJ != null)
        {
            Vector3 startWorld = ScreenToWorldPoint(startTouchPosition);
            swipeTrailOBJ.position = startWorld;
            swipeTrailOBJ.gameObject.SetActive(true);   
        }
    }
    

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        endTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
        //Debug.Log("End Position: " + endTouchPosition);
        if (swipeTrailOBJ != null)
        {
            swipeTrailOBJ.gameObject.SetActive(false);
        }
        DetectSwipt();
    }

    private void DetectSwipt()
    {
        Vector2 swipDelta = endTouchPosition - startTouchPosition;
        if (swipDelta.magnitude < minSwipDistane)
        {
            Debug.Log("Swip tooo short"); Playermovement.SwipeStop();
            return;
        }
        if(Mathf.Abs(swipDelta.x) > Mathf.Abs(swipDelta.y))
        {
            if (swipDelta.x > 0)
            {
                Debug.Log("Swip right"); Playermovement.SwipeMoveRight();
            }
            else
            {
                Debug.Log("Swip left"); Playermovement.SwipeMoveLeft();
            }
        }
        else
        {
            if (swipDelta.y > 0)
            {
                Debug.Log("Swip up"); Playermovement.SwipeJump();
            }
            else
            {
                Debug.Log("Swip down");
            }
        }
    }

    private Vector3 ScreenToWorldPoint(Vector2 screenPosition)
    {
        Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, 10f);
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0f;
        return worldPoint;
    }

    private void Update()
    {
        bool isTouching = inputActions.Touch.PrimaryContact.IsPressed();

        if (isTouching && swipeTrailOBJ != null && swipeTrailOBJ.gameObject.activeSelf)
        {
            Vector2 currentTouchPosition = inputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
            Vector3 currentWorld = ScreenToWorldPoint(currentTouchPosition);

            swipeTrailOBJ.position = currentWorld;
        }
    }
}

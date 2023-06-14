using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    public static SwipeControl Instance;

    private void Awake()
    {
        Instance = (Instance == null) ? this : Instance;
    }

    private float magnitudeSwipe = 25.0f;
    private bool isSwipe;
    private Vector2 currentTouchPosition;
    private Vector2 previousTouchPosition;
    private Vector2 directionTouchVector;

    private void Update()
    {
        UpdateSwipe();
    }

    void test()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentTouchPosition = previousTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            directionTouchVector = currentTouchPosition - previousTouchPosition;
            previousTouchPosition = currentTouchPosition;

            Vector3 direction = new Vector3(directionTouchVector.x, 0.0f, directionTouchVector.y);
            Quaternion targetQuaternion = Quaternion.LookRotation(direction);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, Time.deltaTime * 5.0f);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            currentTouchPosition = previousTouchPosition = Vector3.zero;
        }
    }

    private void UpdateSwipe()
    {
        bool touchBegan = false;
        bool touchMoved = false;
        bool touchEnded = false;

   

#if UNITY_EDITOR
        touchBegan = Input.GetMouseButtonDown(0);
        touchMoved = Input.GetMouseButton(0);
        touchEnded = Input.GetMouseButtonUp(0);
#elif UNITY_IOS
        if(Input.touchCount > 0)
        {
            touchBegan = Input.touches[0].phase == TouchPhase.Began;
            touchMoved = Input.touches[0].phase == TouchPhase.Moved;
            touchEnded = Input.touches[0].phase == TouchPhase.Ended;
        }
#endif

        if (touchBegan)
        {
            currentTouchPosition = previousTouchPosition = Input.mousePosition;
        }
        else if (touchMoved && isSwipe == false)
        {
            currentTouchPosition = Input.mousePosition;
            directionTouchVector = currentTouchPosition - previousTouchPosition;
            previousTouchPosition = currentTouchPosition;

            if (directionTouchVector.magnitude >= magnitudeSwipe)
            {
                isSwipe = true;

                float x = directionTouchVector.x;
                float y = directionTouchVector.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                    {
                        SwipeLeft();
                    }
                    else
                    {
                        SwipeRight();
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        SwipeDown();
                    }
                    else
                    {
                        SwipeUp();
                    }
                }
            }
        }
        else if (touchEnded)
        {
            currentTouchPosition = previousTouchPosition = directionTouchVector = Vector2.zero;
            isSwipe = false;
        }
    }

    private void SwipeLeft()
    {
       // Debug.Log("swipe left");
    }

    private void SwipeRight()
    {
    //    Debug.Log("swipe right");
    }

    private void SwipeUp()
    {
      //  Debug.Log("swipe up");
    }

    private void SwipeDown()
    {
       // Debug.Log("swipe down");
    }
}

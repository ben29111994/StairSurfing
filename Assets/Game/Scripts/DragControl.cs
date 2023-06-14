using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragControl : MonoBehaviour
{
    public float lerpSpeed;
    public float ratio;
    private Vector2 currentTouchPosition;
    private Vector2 previousTouchPosition;
    private Vector2 directionTouchVector;

    public void UpdateSwipe(Transform target)
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
        else if (touchMoved)
        {
            currentTouchPosition = Input.mousePosition;
            directionTouchVector = currentTouchPosition - previousTouchPosition;
            previousTouchPosition = currentTouchPosition;         
        }
        else if (touchEnded)
        {
            currentTouchPosition = previousTouchPosition = directionTouchVector = Vector2.zero;
        }

        Vector3 temp = transform.position;
        temp.x += directionTouchVector.x * ratio;
        temp.x = Mathf.Clamp(temp.x, -3.0f, 3.0f);
        transform.position = temp;

        Vector3 tarPos = target.position;
        tarPos.x = Mathf.Lerp(tarPos.x, temp.x, Time.deltaTime * lerpSpeed);
        target.position = tarPos;
    }
}

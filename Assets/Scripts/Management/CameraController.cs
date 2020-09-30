using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// @author Dominik Kawka
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private static CameraController s_instance = null;

    private Camera m_camera = null;

    private bool m_updatingMovement = true;
    private bool m_updatingArrowsMovement = true;
    private bool m_updatingTouchMovement = true;

    Vector2 StartPosition;
    Vector2 DragStartPosition;
    Vector2 DragNewPosition;
    Vector2 Finger0Position;
    float DistanceBetweenFingers;
    bool isZooming;


    // PC Camera Controll
    private Vector3 m_speed = new Vector3(0.2f, 0,  0.2f);
    private float m_timeElapsed = 0f;
    private float MOVE_DELTA_TIME = 0.016f;


    private void Start()
    {
        if (s_instance != null)
            Debug.Log("Only one instance is allowed!");
        s_instance = this;

        m_camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (m_updatingMovement)
        {
            m_timeElapsed += Time.deltaTime;
            if (m_updatingArrowsMovement && m_timeElapsed > MOVE_DELTA_TIME)
            {
                Vector3 direction = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
                direction.Scale(m_speed);
                transform.position = direction + transform.position;

                m_timeElapsed = 0f;
            }


            if (m_updatingTouchMovement)
            {
                if (Input.touchCount == 0 && isZooming)
                {
                    isZooming = false;
                }

                if (Input.touchCount == 1)
                {
                    if (!isZooming)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            Vector2 NewPosition = GetInputWorldPositionInversedY();
                            Vector2 PositionDifference = NewPosition - StartPosition;
                            this.transform.Translate(-PositionDifference);

                        }
                        StartPosition = GetInputWorldPositionInversedY();
                    }
                }
                else if (Input.touchCount == 2)
                {
                    if (Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        isZooming = true;

                        DragNewPosition = GetTouchWorldPosition(1);
                        Vector2 PositionDifference = DragNewPosition - DragStartPosition;

                        if (Vector2.Distance(DragNewPosition, Finger0Position) < DistanceBetweenFingers)
                            this.GetComponent<Camera>().orthographicSize += (PositionDifference.magnitude);

                        if (Vector2.Distance(DragNewPosition, Finger0Position) >= DistanceBetweenFingers)
                            this.GetComponent<Camera>().orthographicSize -= (PositionDifference.magnitude);

                        DistanceBetweenFingers = Vector2.Distance(DragNewPosition, Finger0Position);
                    }
                    DragStartPosition = GetTouchWorldPosition(1);
                    Finger0Position = GetTouchWorldPosition(0);
                }
            }
           
        }
        
    }


    public Vector3 GetInputWorldPositionInversedY()
    {
        Vector3 temp = Input.mousePosition;
        temp =  m_camera.ScreenToWorldPoint(Input.mousePosition);
        temp.y = temp.z;
        temp.z = 1;
        return temp;
    }

    public Vector3 GetInputWorldPosition()
    {
        Vector3 temp = Input.mousePosition;
        temp = m_camera.ScreenToWorldPoint(Input.mousePosition);
        return temp;
    }



    public Vector3 GetTouchWorldPosition(int fingerIndex)
    {
        return m_camera.ScreenToWorldPoint(Input.GetTouch(fingerIndex).position);
    }


    public bool OverallMovement
    {
        get => m_updatingMovement;
        set
        {
            m_updatingMovement = value;
        }
    }

    public bool ArrowsMovement
    {
        get => m_updatingArrowsMovement;
        set
        {
            m_updatingMovement = value;
        }
    }

    public bool TouchMovement
    {
        get => m_updatingTouchMovement;
        set
        {
            m_updatingTouchMovement = value;
        }
    }

    public static void SSetOverallMovement(bool val)
    {
        if (s_instance != null)
        {
            s_instance.OverallMovement = val;
        }
    }
}

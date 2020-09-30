using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// [Singleton]
/// Defines a way input events are processed.
/// @author Rivenort
/// </summary>
public class M_InputManager : UT_IClearable
{
    private static M_InputManager s_instance = null;
    private static readonly object s_lock = new object();

    private GameObject m_currentTrackedModel = null;
    private GameObject m_currentDraggedObject = null;
    private DraggableObject m_currentDraggedObjectComp = null;
    private GameObject m_draggingCandidate = null;

    private float m_leftButtDownTimeElapsed = 0f;
    private bool m_leftButtonDown = false;
    private const float CLICK_TIME = 0.2f;

    //private Vector3 m_camPosOnBegan = Vector3.zero;

    /* when m_processing is equal to false, world input events are not processed.
     It is caused by UI elements that are currently receiving the input events. */
    /* The events are processed when freezeTime is bigger than specified value.
     The reasoning is that world input events shouldn't be processed RIGHT after 
     the UI events. */
    private bool m_processing = true;
    private float m_freezeTimeElapsed = 0f;


    private M_InputManager() { }

    public static M_InputManager Instance
    {
        get 
        {
            lock (s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_InputManager();
                return s_instance;
            }
        }
    }

    public void Clear()
    {
        m_currentTrackedModel = null;
        m_currentDraggedObject = null;
        m_currentDraggedObjectComp = null;
        m_draggingCandidate = null;
        m_leftButtonDown = false;
        m_processing = true;
        m_freezeTimeElapsed = 0f;
    }


    public void FixedUpdate()
    {
 
    }

    public void Update()
    {
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:
                m_currentTrackedModel = M_GameHelper.SGetObjectAtScreenPoint(Input.mousePosition);
                break;
            case DeviceType.Handheld:
                if (Input.touchCount > 0)
                    m_currentTrackedModel = M_GameHelper.SGetObjectAtScreenPoint(Input.GetTouch(0).position);
                break;
        }

        if (m_processing)
            m_freezeTimeElapsed += Time.deltaTime;

        if (m_freezeTimeElapsed > CLICK_TIME)
        switch (SystemInfo.deviceType)
        {
            case DeviceType.Desktop:

                if (m_leftButtonDown)
                {
                    m_leftButtDownTimeElapsed += Time.deltaTime;
                }


                if (Input.GetMouseButtonDown(0)) // On Start
                {
                    m_leftButtDownTimeElapsed = 0f;
                    m_leftButtonDown = true;
                    m_draggingCandidate = m_currentTrackedModel;

                    // Examine if object should be clicked. if not then skip to dragging examination.
                    if (m_currentTrackedModel != null)
                        {
                            ClickableObject clickable = m_currentTrackedModel.GetComponent<ClickableObject>();
                            DraggableObject draggable = m_currentTrackedModel.GetComponent<DraggableObject>();
                            if (clickable != null && clickable.disabled && draggable != null && draggable.draggingEnabled)
                            {
                                m_leftButtDownTimeElapsed = CLICK_TIME * 2;
                                CameraController.SSetOverallMovement(false);
                                m_currentDraggedObject = m_currentTrackedModel.gameObject.transform.parent.gameObject;
                            }
                        } 
                    //
                } else if (Input.GetMouseButtonUp(0) && m_leftButtDownTimeElapsed < CLICK_TIME) // OnClick
                {
                    if (m_currentTrackedModel != null)
                    {
                        ClickableObject comp = m_currentTrackedModel.transform.parent.gameObject.GetComponent<ClickableObject>();
                        if (comp != null)
                        {
                            comp.OnClick();
                        }
                    }
                    m_leftButtonDown = false;
                    m_leftButtDownTimeElapsed = 0f;
                    m_currentTrackedModel = null;
                    m_draggingCandidate = null;

                } else if (Input.GetMouseButtonUp(0) && m_leftButtDownTimeElapsed > CLICK_TIME) // End of dragging
                {
                    m_leftButtonDown = false;
                    m_leftButtDownTimeElapsed = 0f;
                    m_currentDraggedObject = null;
                }


                    // Examine for dragging
                    if (m_currentDraggedObject == null && m_leftButtDownTimeElapsed > CLICK_TIME)
                    {
                        // get a parent object 
                        if (m_draggingCandidate != null)
                        {
                            m_currentDraggedObject = m_draggingCandidate.transform.parent.gameObject;
                            m_currentDraggedObjectComp = m_currentDraggedObject.GetComponent<DraggableObject>();
                            if (!(m_currentDraggedObjectComp != null && m_currentDraggedObjectComp.draggingEnabled))
                                m_currentDraggedObject = null;
                            m_draggingCandidate = null;
                        }

                    }
                    else if (m_currentDraggedObject != null) // Dragging
                    {
                        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        if (m_currentDraggedObjectComp.keepHighYDepthValue)
                            temp.y = M_MapManager.SGetHighYDepthValue(temp.z);
                        else
                            temp.y = M_MapManager.SGetYDepthValue(temp.z);
                        m_currentDraggedObject.transform.position = temp;
                    }
                    break;
            // ------- Handheld --------
            case DeviceType.Handheld:

                    StringBuilder stringBuilder = new StringBuilder();

                    if (m_leftButtonDown)
                    {
                        m_leftButtDownTimeElapsed += Time.deltaTime;
                    }


                    // Examine for dragging
                    if (m_currentDraggedObject == null && m_leftButtDownTimeElapsed > CLICK_TIME)
                    {
                        // get a parent object 
                        if (m_draggingCandidate != null)
                        {
                            m_currentDraggedObject = m_draggingCandidate.transform.parent.gameObject;
                            m_currentDraggedObjectComp = m_currentDraggedObject.GetComponent<DraggableObject>();
                            if (!(m_currentDraggedObjectComp != null && m_currentDraggedObjectComp.draggingEnabled))
                                m_currentDraggedObject = null;
                            else
                                CameraController.SSetOverallMovement(false);
                            m_draggingCandidate = null;
                        }

                    }



                    if (m_processing)
                        m_freezeTimeElapsed += Time.deltaTime;

                    if (m_freezeTimeElapsed > CLICK_TIME)
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                    stringBuilder.AppendLine("Began: " + touch.position);

                                    m_leftButtDownTimeElapsed = 0f;
                                    m_leftButtonDown = true;
                                    m_draggingCandidate = m_currentTrackedModel;

                                    // Examine if object should be clicked. if not then skip to dragging examination.
                                    if (m_currentTrackedModel != null)
                                    {
                                        ClickableObject clickable = m_currentTrackedModel.GetComponent<ClickableObject>();
                                        DraggableObject draggable = m_currentTrackedModel.GetComponent<DraggableObject>();
                                        if (clickable != null && clickable.disabled && draggable != null && draggable.draggingEnabled)
                                        {
                                            m_leftButtDownTimeElapsed = CLICK_TIME * 2;
                                            CameraController.SSetOverallMovement(false);
                                            m_currentDraggedObject = m_currentTrackedModel.gameObject.transform.parent.gameObject;
                                        }
                                    }
                                    //

                                    break;
                            case TouchPhase.Moved:
                                    stringBuilder.AppendLine("Moved: " + touch.position);

                                    if (m_currentDraggedObject != null) // Dragging
                                {
                                    Vector3 temp = Camera.main.ScreenToWorldPoint(touch.position);
                                    if (m_currentDraggedObjectComp.keepHighYDepthValue)
                                        temp.y = M_MapManager.SGetHighYDepthValue(temp.z);
                                    else
                                        temp.y = M_MapManager.SGetYDepthValue(temp.z);
                                    m_currentDraggedObject.transform.position = temp;
                                }

                                break;
                            case TouchPhase.Ended:
                                stringBuilder.AppendLine("Ended: " + touch.position);

                                if (m_leftButtDownTimeElapsed < CLICK_TIME) // OnClick
                                {
                                    if (m_currentTrackedModel != null)
                                    {
                                        ClickableObject comp = m_currentTrackedModel.transform.parent.gameObject.GetComponent<ClickableObject>();
                                        if (comp != null)
                                        {
                                            comp.OnClick();
                                        }
                                    }
                                    m_leftButtonDown = false;
                                    m_leftButtDownTimeElapsed = 0f;
                                    m_currentTrackedModel = null;
                                    m_draggingCandidate = null;
                                } else // End of dragging
                                {
                                    m_leftButtonDown = false;
                                    m_leftButtDownTimeElapsed = 0f;
                                    m_currentDraggedObject = null;
                                    CameraController.SSetOverallMovement(true);
                                }
                                break;
                        }
                    } else
                        {
                            stringBuilder.AppendLine("No touch");
                        }

                    stringBuilder.AppendLine("CurrentDraggedObject: " + (m_currentDraggedObject == null ? "NULL" : m_currentDraggedObject.name));
                    stringBuilder.AppendLine("CurrentTrackedModel: " + (m_currentTrackedModel == null ? "NULL" : m_currentTrackedModel.name));
                    stringBuilder.AppendLine("DraggingCandidate " + (m_draggingCandidate == null ? "NULL" : m_draggingCandidate.name));
                    M_GameHelper.debugText = stringBuilder.ToString();
                    break;
        }
    }

    /// <summary> Sets weather input events are supposed to be processed. </summary>
    public static void SSetProcessingEvents(bool val)
    {
        if (s_instance != null)
        {
            if (val)
                s_instance.m_freezeTimeElapsed = 0f;
            s_instance.m_processing = val;
        }
    }
}

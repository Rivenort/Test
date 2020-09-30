using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// @author Dominik
/// </summary>
[RequireComponent(typeof(ClickableObject))]
[RequireComponent(typeof(DraggableObject))]
public class BuildingProd : MonoBehaviour, IBuildingProduction
{
    private bool m_pauseBuildings = false;

    protected SpriteRenderer m_spriteRenderer;

    protected DAT_BuildingProd data;

    [Space(10)]
    [Header("GFX")]
    public Sprite spriteOnMoving;
    public Sprite spriteWhenBuilding;
    public Sprite spriteWhenReady;

    [Space(10)]
    [Header("UI")]
    public Slider buildingProgressSlider;
    public GameObject buildButton;
    public Color colorWhenMoving;

    [Space(10)]
    [Header("Physics")]
    [SerializeField] BuildingPhysics m_buildingPhysics;

    [Space(10)]
    [Header("GameLogic")]
    [SerializeField] protected Guid m_id;
    [SerializeField] protected int m_moodOnThisBuilding;


    private Guid m_assignedSettler;

    private float m_timeElapsed;
    private Action<IBuilding> m_actionOnBuildingFinished = null;

    public enum State
    {
        ON_MOVING,
        BUILDING,
        READY
    }
    public State state;

    protected void Start()
    {
        if (m_id == Guid.Empty) // we dont want to create new id if it was already set (e.g. by serialization)
            m_id = Guid.NewGuid();

        GetComponent<ClickableObject>().Func = OnClick;

        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SetState(state);

        if (m_assignedSettler == null)
            m_assignedSettler = Guid.Empty;
    }


    protected void Update()
    {
        if (!m_pauseBuildings)
            switch (state)
            {
                case State.BUILDING:
                    // wait until building is finished
                    m_timeElapsed += Time.deltaTime;
                    buildingProgressSlider.value = m_timeElapsed / data.buildingTime;

                    if (m_timeElapsed >= data.buildingTime)
                    {
                        SetState(State.READY);
                        m_actionOnBuildingFinished(this);
                    }
                    break;
                case State.READY:

                    break;
            }
    }



    public void SetState(State state)
    {
        if (m_spriteRenderer == null)
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        switch (state)
        {
            case State.ON_MOVING:
                GetComponent<DraggableObject>().draggingEnabled = true;
                GetComponent<ClickableObject>().disabled = true;
                buildingProgressSlider.gameObject.SetActive(false);
                m_spriteRenderer.color = colorWhenMoving;
                m_spriteRenderer.sprite = spriteOnMoving;
                this.state = state;
                break;
            case State.BUILDING:
                GetComponent<DraggableObject>().draggingEnabled = false;
                GetComponent<ClickableObject>().disabled = true;
                buildingProgressSlider.gameObject.SetActive(true);
                m_spriteRenderer.color = Color.white;
                m_spriteRenderer.sprite = spriteWhenBuilding;
                this.state = state;
                break;
            case State.READY:
                GetComponent<DraggableObject>().draggingEnabled = false;
                GetComponent<ClickableObject>().disabled = false;
                buildingProgressSlider.gameObject.SetActive(false);
                m_spriteRenderer.color = Color.white;
                m_spriteRenderer.sprite = spriteWhenReady;
                this.state = state;
                break;
        }
    }


    private void OnClick()
    {
        if (state == State.READY)
            UI_ScreenBuilding.SShowStats(this);
    }

    /*
    public void InitArea(int w, int h)
    {
        m_area = new bool[w * h];
    }

    public int MiddleTile()
    {
        int i = m_width / 2;
        int j = m_height / 2;
        return i * m_width + j;
    }*/


    public Guid GetId()
    {
        return m_id;
    }

    public void SetId(Guid id)
    {
        m_id = id;
    }

    public string GetName()
    {
        return data.name;
    }

    public string GetDesc()
    {
        return data.desc;
    }

    public string GetPrefab()
    {
        return data.prefabName;
    }

    public int GetBuildTime()
    {
        return data.buildingTime;
    }

    public int GetCostFood()
    {
        return data.costFood;
    }

    public int GetCostResearch()
    {
        return data.costResearch;
    }

    public float GetCostFactor()
    {
        return data.costFactor;
    }

    public bool IsAvailableOnStart()
    {
        return data.availableOnStart;
    }

    public Sprite GetIcon()
    {
        return spriteWhenReady;
    }

    public void Pause(bool val)
    {
        m_pauseBuildings = val;
    }

    public int GetProducedFood()
    {
        return data.producedFood;
    }

    public int GetProducedResearch()
    {
        return data.producedResearch;
    }

    public DAT_Ware.Type GetProducedWareType()
    {
        return data.producedWare;
    }

    public Guid GetWorker()
    {
        return m_assignedSettler;
    }

    public Guid SetWorker(Guid newWorker)
    {
        Guid temp = m_assignedSettler;
        m_assignedSettler = newWorker;
        return temp;
    }

    public BuildingPhysics GetPhysics()
    {
        return m_buildingPhysics;
    }

    public void SetData(DAT_BuildingProd data)
    {
        this.data = data;
    }

    public GameObject GetBuildButton()
    {
        return buildButton;
    }

    public void StartBuilding()
    {
        SetState(State.BUILDING);
    }

    public DAT_Building GetData()
    {
        return data;
    }

    public void SetActionOnBuildingFinished(Action<IBuilding> func)
    {
        m_actionOnBuildingFinished = func;
    }

    public void SetTimeElapsed(float timeElapsed)
    {
        m_timeElapsed = timeElapsed;
    }

    public State GetState()
    {
        return state;
    }

    public float GetTimeElapsed()
    {
        return m_timeElapsed;
    }

    public void TriggerDay(int day)
    {
        if (m_assignedSettler != Guid.Empty)
            m_moodOnThisBuilding = M_SettlersManager.SGetMoodFavBuilding(m_assignedSettler);
    }

    public void TriggerMonth(int month)
    {

    }

    public void TriggerYear(int year)
    {

    }

    public int GetMood()
    {
        return m_moodOnThisBuilding;
    }

    public bool IsWorking()
    {
        if (m_assignedSettler == Guid.Empty) return false;
        return true;
    }
}

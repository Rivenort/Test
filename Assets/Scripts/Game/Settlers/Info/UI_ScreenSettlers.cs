using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ScreenSettlers : MonoBehaviour
{
    private static UI_ScreenSettlers s_instance = null;

    public GameObject settlerScreen;
    public GameObject elemSettlerRow;
    public GameObject container;
    public TMP_Text setlersCount;


    private void Start()
    {
        if (s_instance != null)
            Debug.LogWarning("Instance has been already created!");
        s_instance = this;
    }

    private void OnEnable()
    {

    }

    public void ShowSettlers(List<ISettler> settlers)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            GameObject.Destroy(container.transform.GetChild(i).gameObject);
        }

        setlersCount.text = settlers.Count.ToString();

        RectTransform containerTransform = container.GetComponent<RectTransform>();
        containerTransform.sizeDelta = new Vector2(containerTransform.rect.width, elemSettlerRow.GetComponent<RectTransform>().rect.height * settlers.Count);

        for (int i = 0; i < settlers.Count; i++)
        {
            GameObject temp = Instantiate(elemSettlerRow);
            temp.transform.SetParent(container.transform);
            temp.GetComponent<UI_ElemSettler>().Setup(settlers[i], settlerScreen);
            RectTransform rectTransform = temp.GetComponent<RectTransform>();
            float yOffset = container.GetComponent<RectTransform>().rect.height / 2 - rectTransform.rect.height / 2;
            rectTransform.localPosition = new Vector3(0, yOffset - rectTransform.rect.height * i, 0);

        }

        M_ScreenManager.SChangeScreenPersistentBack(this.gameObject);
    }

    public static void SShowSettlers(List<ISettler> settlers)
    {
        if (s_instance == null) return;
        s_instance.ShowSettlers(settlers);
    }

    public static void SShowSettlers(List<Guid> settlers)
    {
        if (s_instance == null) return;
        s_instance.ShowSettlers(M_SettlersManager.SGetSettlers(settlers));
    }

    public static void SShowSettlers()
    {
        if (s_instance == null) return;
        s_instance.ShowSettlers(M_SettlersManager.SGetSettlers());
    }

}

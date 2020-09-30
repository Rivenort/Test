using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ElemSettler : MonoBehaviour
{
    private static int s_counter = 0;

    [Header("Background")]
    public Color colorRowA;
    public Color colorRowB;

    private static List<ISettler> s_employees = new List<ISettler>();

    [Space(10)]
    [Header("Interface")]
    public TMP_Text name;
    public Button infoButton;
    public Image settlerPortrait;
    public Slider moodSlider;

    private ISettler m_settler;
    private GameObject m_settlerScreen;

    public void Setup(ISettler settler, GameObject settlerScreen)
    {
        if (s_counter % 2 == 0)
            gameObject.GetComponent<Image>().color = colorRowA;
        else
            gameObject.GetComponent<Image>().color = colorRowB;

        m_settler = settler;
        m_settlerScreen = settlerScreen;
        name.text = settler.GetName();
        moodSlider.value = settler.GetMood();

        settlerPortrait.sprite = M_SettlersManager.SGetPortrait(settler.GetPortraitId());
        infoButton.onClick.AddListener(OnClickInfo);

        s_counter++;
    }

    public void OnClickInfo()
    {
        UI_ScreenSettler.SShowSettler(m_settler);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort    
/// </summary>
public class GUIUtils
{

    public static void AddElementsToScrollContainer(GameObject container, List<GameObject> objects)
    {

        int i = 0;
        RectTransform containerTransform = container.GetComponent<RectTransform>();
        foreach (GameObject o in objects)
        {
            o.transform.SetParent(container.transform);
            RectTransform objRectTransform = o.GetComponent<RectTransform>();

            objRectTransform.sizeDelta = new Vector2(containerTransform.rect.width, objRectTransform.rect.height);

            float yOffset = containerTransform.rect.height / 2 - objRectTransform.rect.height / 2;
            objRectTransform.localPosition = new Vector3(0, yOffset - objRectTransform.sizeDelta.y * i, 0);

            containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, containerTransform.sizeDelta.y + objRectTransform.sizeDelta.y);


            i++;
        }

        containerTransform.localPosition = new Vector3(0, -containerTransform.sizeDelta.y / 2, 0);
    }

    public static void AddElementsToCleanYScrollContainer(GameObject container, List<GameObject> objects)
    {

        foreach (Transform child in container.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        int i = 0;
        // reset (y) height
        RectTransform containerTransform = container.GetComponent<RectTransform>();
        containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, 0);

        foreach (GameObject o in objects)
        {
            o.transform.SetParent(container.transform);
            RectTransform objRectTransform = o.GetComponent<RectTransform>();

            objRectTransform.sizeDelta = new Vector2(containerTransform.rect.width, objRectTransform.rect.height);

            float yOffset = containerTransform.rect.height / 2 - objRectTransform.rect.height / 2;
            objRectTransform.localPosition = new Vector3(0, yOffset - objRectTransform.sizeDelta.y * i, 0);

            containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, containerTransform.sizeDelta.y + objRectTransform.sizeDelta.y);


            i++;
        }

        containerTransform.localPosition = new Vector3(0, -containerTransform.sizeDelta.y / 2, 0);
    }
}

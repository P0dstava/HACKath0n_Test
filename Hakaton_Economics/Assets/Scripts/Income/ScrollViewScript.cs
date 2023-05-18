using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScript : MonoBehaviour
{
    RectTransform rectTrans;

    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Debug.Log(rectTrans.sizeDelta.y + " x-" +rectTrans.sizeDelta.x);
    }
}

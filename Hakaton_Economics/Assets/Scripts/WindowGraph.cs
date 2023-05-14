using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField]private Sprite circleSprite;
    private RectTransform graphContainer;
    float xPosition = 0f, yPosition = 0f, yMaximum = 0f, xMaximum = 0f, graphHeight, graphWidth; 

    private void Awake(){
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valList =  new List<int>(){0, 0, 0, 0, 0, 0, 0, 0, 15, 100};
        for(int i = 0; i < valList.Count; i++){
            if(valList[i] > yMaximum)
                yMaximum = valList[i] + 10f;
        }
        ShowGraph(valList);
    }

    private void ShowGraph(List<int> valueList){
        graphHeight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;

        xMaximum = valueList.Count * graphWidth;

        GameObject lastCircleGameObject = null;
        for(int i = 0; i < valueList.Count; i++){
            xPosition = 50 + i * 50;
            yPosition = 10 +(valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if(lastCircleGameObject != null){
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB){
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);


        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);
        float angles = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPosA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, angles);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    public static WindowGraph instance;

    void Awake(){
        if(instance != null){
            Debug.LogWarning("Instance freak up!");
            return;
        }
        instance = this;
    }
    
    [SerializeField]private Sprite circleSprite;
    private RectTransform graphContainer;
    float xPosition = 0f, yPosition = 0f, yMaximum = 0f, xMaximum = 0f, graphHeight, graphWidth; 
    public List<float> valList;

    void Start(){
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        valList = new List<float>();
        ShowGraph(valList);
    }

    public void ShowGraph(List<float> valueList){
        yMaximum = 0;
        for(int i = 0; i < valueList.Count; i++){
            if(valueList[i] > yMaximum){
                yMaximum = valueList[i] + 10f;
            }
        }
        graphHeight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;

        xMaximum = valueList.Count * graphWidth;

        Debug.Log(graphHeight + "   " + yMaximum);
        GameObject lastCircleGameObject = null;
        for(int i = 0; i < valueList.Count; i++){
            xPosition = 50 + i * 50;
            yPosition = 10 +(1/Mathf.Log((valueList[i] /graphHeight)));
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

    public void DeleteGraphContainerDots(){
        DeleteAllChildren(graphContainer);
    }

    void DeleteAllChildren(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = childCount - 1; i >= 0; i--){
            Transform child = parent.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}

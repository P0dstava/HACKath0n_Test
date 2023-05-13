using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField]private Sprite circleSprite;
    private RectTransform graphContainer;
    float xPosition = 0, yPosition = 0, yMaximum = 100f, graphHeight; 

    private void Awake(){
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valList =  new List<int>(){5, 65, 85, 45, 79, 34, 5, 0, 100, 12};
        ShowGraph(valList);
    }

    private void CreateCircle(Vector2 anchoredPosition){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void ShowGraph(List<int> valueList){
        graphHeight = graphContainer.sizeDelta.y;
        for(int i = 0; i < valueList.Count; i++){
            xPosition = i * 50f;
            yPosition = (valueList[i] / yMaximum) * graphHeight;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
    }
}

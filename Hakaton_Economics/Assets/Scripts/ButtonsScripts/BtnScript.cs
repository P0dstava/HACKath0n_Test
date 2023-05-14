using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnScript : MonoBehaviour
{
    [SerializeField]Button m_Button;
    [SerializeField]List<GameObject> objsToActivate = new List<GameObject>(), objsToDisable = new List<GameObject>();

    void Awake(){
        Button btn = m_Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick(){
        if(objsToActivate.Count != 0){
            for(int i = 0; i < objsToActivate.Count; i++){
                objsToActivate[i].SetActive(true);
            }
        }

        if(objsToDisable.Count != 0){
            for(int i = 0; i < objsToDisable.Count; i++){
                objsToDisable[i].SetActive(false);
            }
        }
    }
}

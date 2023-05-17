using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditTransactionManager : MonoBehaviour
{
    public static EditTransactionManager instance;

    void Awake(){
        if(instance != null){
            Debug.LogWarning("Instance freak up");
            return;
        }
        instance = this;
    }

    public TransactionScriptableObject tsObj;
    [SerializeField]GameObject editManager, transactionMenu, bottomPanel;

    public void EditTransaction(TransactionScriptableObject transSO){
        tsObj = transSO;
        SetActiveEditor(true);
    }

    void SetActiveEditor(bool state){
        editManager.SetActive(state);
        transactionMenu.SetActive(!state);
        bottomPanel.SetActive(!state);
    }
}

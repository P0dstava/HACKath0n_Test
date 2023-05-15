using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransPrefabScript : MonoBehaviour
{
    TransactionManager transactionManager;
    public TransactionScriptableObject curTransaction;
    [SerializeField]Button m_Button;

    void Awake(){
        Button btn = m_Button.GetComponent<Button>();
        btn.onClick.AddListener(DeleteTransaction);
        transactionManager = TransactionManager.instance;
    }

    void DeleteTransaction(){
        GameObject.Destroy(gameObject);
        transactionManager.RemoveTransaction(curTransaction);
        GameObject.DestroyImmediate(curTransaction, true);
    }
}

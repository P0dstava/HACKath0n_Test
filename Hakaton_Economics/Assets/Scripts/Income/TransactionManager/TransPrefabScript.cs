using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TransPrefabScript : MonoBehaviour
{
    TransactionManager transactionManager;
    EditTransactionManager editTransactionManager;
    public TransactionScriptableObject curTransaction;
    [SerializeField]Button d_Button, e_Button;

    void Awake(){
        Button deleteBtn = d_Button.GetComponent<Button>();
        Button editBtn = e_Button.GetComponent<Button>();
        deleteBtn.onClick.AddListener(DeleteTransaction);
        editBtn.onClick.AddListener(EditTransaction);

        editTransactionManager = EditTransactionManager.instance;
        transactionManager = TransactionManager.instance;
    }

    private void Update()
    {
        transform.localScale = Vector3.one;
    }

    void EditTransaction(){
        editTransactionManager.EditTransaction(curTransaction);
    }

    void DeleteTransaction(){
        string filePath = Application.persistentDataPath + "/" + curTransaction.tagOfTrans +".json";
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        transactionManager.RemoveTransaction(curTransaction);
        GameObject.DestroyImmediate(curTransaction, true);
        GameObject.Destroy(gameObject);
    }
}

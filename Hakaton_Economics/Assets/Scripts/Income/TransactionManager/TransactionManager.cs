using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TransactionManager : MonoBehaviour
{
    public static TransactionManager instance;
    
    [SerializeField]Button p_Button, m_Button;

    [SerializeField]TransactionScriptableObject cash;
    [SerializeField]TextMeshProUGUI cashText;
    [HideInInspector]public List<TransactionScriptableObject> transactions;
    public string typeOfSorting = "date";

    void Start(){
        //cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }
    
    void Awake(){
        if(instance != null){
            Debug.LogWarning("Instance freak up!");
            return;
        }
        instance = this;

        Button p_btn = p_Button.GetComponent<Button>();
        Button m_btn = m_Button.GetComponent<Button>();
        p_btn.onClick.AddListener(ShowIncome);
        m_btn.onClick.AddListener(ShowSpendings);
    }

    // Update is called once per frame
    void Update(){
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    void ShowIncome(){
        for(int i = 0; i < transactions.Count; i++){
            if(transactions[i].summOfTrans > 0.00f)
                Debug.Log(transactions[i].nameOfTrans);
        }
    }

    void ShowSpendings(){
        for(int i = 0; i < transactions.Count; i++){
            if(transactions[i].summOfTrans < 0.00f)
                Debug.Log(transactions[i].nameOfTrans);
        }
    }

    public void AddTransaction(TransactionScriptableObject m_Transaction){
        transactions.Add(m_Transaction);
        ChangeCashAmount(m_Transaction);
    }

    void ChangeCashAmount(TransactionScriptableObject m_Transaction){
        cash.summOfTrans += m_Transaction.summOfTrans;
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    void SortTransactions(string typeOfSorting){
        switch(typeOfSorting){
            case "date":
            transactions = transactions.OrderBy(transactions => transactions.year).ThenBy(TransactionScriptableObject => TransactionScriptableObject.month).ThenBy(TransactionScriptableObject => TransactionScriptableObject.day).ToList();
            break;
        }
    }
}

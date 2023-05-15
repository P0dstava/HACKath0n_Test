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
    [HideInInspector]public List<TransactionScriptableObject> transactions, m_TransactionsList, p_TransactionsList;
    [SerializeField]GameObject transPrefab, transListObj;
    float prefabOffsetY = 166f;

    public string typeOfSorting = "date";
    
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
        
        m_TransactionsList = new List<TransactionScriptableObject>();
        p_TransactionsList = new List<TransactionScriptableObject>();
    }

    void Start(){
        //cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
        //prefabOffset = transPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }


    // Update is called once per frame
    void Update(){
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    void ShowIncome(){
        if(p_TransactionsList.Count > 0){
            for(int i = 0; i < transactions.Count; i++){
                Debug.Log(p_TransactionsList[i].nameOfTrans);
                GenerateList(p_TransactionsList);
            }
        }
    }

    void ShowSpendings(){
        if(m_TransactionsList.Count > 0){
            for(int i = 0; i < transactions.Count; i++){
                Debug.Log(m_TransactionsList[i].nameOfTrans);
                GenerateList(m_TransactionsList);
            }
        }
    }

    //Function called from outside to add a new transaction to a list
    public void AddTransaction(TransactionScriptableObject m_Transaction){
        transactions.Add(m_Transaction);
        AssignTransaction(m_Transaction);
        ChangeCashAmount(m_Transaction);
    }

    //Adding or substracting a new transaction summ from overall amount of money
    void ChangeCashAmount(TransactionScriptableObject m_Transaction){
        cash.summOfTrans += m_Transaction.summOfTrans;
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    //Assigning trnsaction to "m_TransactionsList" if it is negative and to "p_TransactionsList" if it is positive
    void AssignTransaction(TransactionScriptableObject m_Transaction){
        if(m_Transaction.summOfTrans > 0f){
            p_TransactionsList.Add(m_Transaction);
        }

        else if(m_Transaction.summOfTrans < 0f){
            m_TransactionsList.Add(m_Transaction);
        }
    }

    //Generating a list of prefabs
    void GenerateList(List<TransactionScriptableObject> transList){
        DeleteAllChildren(transListObj.transform);
        for(int i = 0; i < transList.Count; i++){
            GameObject newTransPrefab = Instantiate(transPrefab);
            newTransPrefab.transform.SetParent(transListObj.transform);
            newTransPrefab.transform.position = new Vector2(transListObj.transform.position.x, transListObj.transform.position.y - prefabOffsetY * i);
            /*newTransPrefab.Find("DateText").text = transList[i].nameOfTrans;
            newTransPrefab.Find(DateText).text = transList[i].nameOfTrans;*/
        }
    }

    //Wiping all previous prefabs from transListObj
    void DeleteAllChildren(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    void SortTransactions(string typeOfSorting){
        switch(typeOfSorting){
            case "date":
            transactions = transactions.OrderBy(transactions => transactions.year).ThenBy(TransactionScriptableObject => TransactionScriptableObject.month).ThenBy(TransactionScriptableObject => TransactionScriptableObject.day).ToList();
            break;
        }
    }
}

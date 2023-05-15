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

    GameObject prevTransPrefab;
    Transform prevTransPrefabTrans;
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

        prevTransPrefab = new GameObject();
        prevTransPrefabTrans = prevTransPrefab.GetComponent<Transform>();
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
        /*if(p_TransactionsList.Count > 0){
            for(int i = 0; i < transactions.Count; i++){
                Debug.Log(p_TransactionsList[i].nameOfTrans);
            }
        }*/
        GenerateList(p_TransactionsList);
    }

    void ShowSpendings(){
        /*if(m_TransactionsList.Count > 0){
            for(int i = 0; i < transactions.Count; i++){
                Debug.Log(m_TransactionsList[i].nameOfTrans);
            }
        }*/
        GenerateList(m_TransactionsList);
    }

    //Function called from outside to add a new transaction to a list
    public void AddTransaction(TransactionScriptableObject m_Transaction){
        transactions.Add(m_Transaction);
        AssignTransaction(m_Transaction);
        ChangeCashAmount(m_Transaction);
    }
    
    //Function called from outside to add a new transaction to a list
    public void RemoveTransaction(TransactionScriptableObject m_Transaction){
        transactions.Remove(m_Transaction);

        if(m_Transaction.summOfTrans > 0f){
            p_TransactionsList.Remove(m_Transaction);
            GenerateList(p_TransactionsList);
        }

        else if(m_Transaction.summOfTrans < 0f){
            m_TransactionsList.Remove(m_Transaction);
            GenerateList(m_TransactionsList);
        }

        m_Transaction.summOfTrans = -m_Transaction.summOfTrans;
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
        if(transList.Count != 0){
            for(int i = 0; i < transList.Count; i++){
                GameObject newTransPrefab = Instantiate(transPrefab);
                Transform newTransPrefabTrans = newTransPrefab.GetComponent<Transform>();
                newTransPrefabTrans.GetComponent<TransPrefabScript>().curTransaction = transList[i];

                TextMeshProUGUI newTransPrefabName = newTransPrefabTrans.Find("TransName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI newTransPrefabSumm = newTransPrefabTrans.Find("TransSumm").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI newTransPrefabDate = newTransPrefabTrans.Find("TransDate").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI newTransPrefabTime = newTransPrefabTrans.Find("TransTime").GetComponent<TextMeshProUGUI>();

                TextMeshProUGUI prevTransPrefabDate = newTransPrefabTrans.Find("TransDate").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI prevTransPrefabTime = newTransPrefabTrans.Find("TransTime").GetComponent<TextMeshProUGUI>();
            
                newTransPrefabTrans.SetParent(transListObj.transform);

                newTransPrefabName.text = transList[i].nameOfTrans;
                newTransPrefabSumm.text = transList[i].summOfTrans.ToString("0.00");
                newTransPrefabTime.text = transList[i].hour.ToString()+":"+transList[i].minute.ToString();

                if(prevTransPrefabTrans != newTransPrefabTrans){
                    string dateText = transList[i].year.ToString()+"."+transList[i].month.ToString()+"."+transList[i].day.ToString();

                    //if(newTransPrefabDate.text == prevTransPrefabDate.text){
                        newTransPrefabDate.text = dateText;
                        //prevTransPrefabDate.text = "";
                    //}
                    newTransPrefabTrans.position = new Vector2(transListObj.transform.position.x, transListObj.transform.position.y /*- prefabOffsetY*/);
                    prevTransPrefabTrans.position = new Vector2(transListObj.transform.position.x, transListObj.transform.position.y - prefabOffsetY * (transList.Count - i));

                    prevTransPrefabTrans = newTransPrefabTrans;
                }


                /*else if(prevTransPrefabTrans == newTransPrefabTrans){
                    Debug.Log("фівфівфівф New: " + newTransPrefabTrans.position.y + " Prev:" + prevTransPrefabTrans.position.y);
                    string dateText = transList[i].year.ToString()+"."+transList[i].month.ToString()+"."+transList[i].day.ToString();
                    prevTransPrefabDate.text = dateText;
                    newTransPrefabDate.text = dateText;
                    newTransPrefabTrans.position = new Vector2(transListObj.transform.position.x, transListObj.transform.position.y - prefabOffsetY);
                }*/
                /*newTransPrefab.Find(DateText).text = transList[i].nameOfTrans;*/
            }
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
        
        prevTransPrefab = new GameObject();
        prevTransPrefabTrans = prevTransPrefab.GetComponent<Transform>();
        prevTransPrefabTrans.SetParent(transListObj.transform);
    }

    void SortTransactions(string typeOfSorting){
        switch(typeOfSorting){
            case "date":
            transactions = transactions.OrderBy(transactions => transactions.year).ThenBy(TransactionScriptableObject => TransactionScriptableObject.month).ThenBy(TransactionScriptableObject => TransactionScriptableObject.day).ToList();
            break;
        }
    }
}

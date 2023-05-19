using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.IO;

public class TransactionManager : MonoBehaviour
{
    public static TransactionManager instance;
    
    [SerializeField]Button p_Button, m_Button, d_Button;
    bool dateOrder = true; //true - from lowest to biggest, false - vise versa
    int prevList = 1;

    [SerializeField]TransactionScriptableObject cash;
    [SerializeField]TextMeshProUGUI cashText;
    [HideInInspector]public List<TransactionScriptableObject> transactions, m_TransactionsList, p_TransactionsList;
    [SerializeField]GameObject transPrefab, transListObj;

    GameObject prevTransPrefab;
    Transform prevTransPrefabTrans;
    float prefabOffsetY = 166f;
    //WindowGraph windowGraph;
    
    void Awake(){
        if(instance != null){
            Debug.LogWarning("Instance freak up!");
            return;
        }
        instance = this;

        Button p_btn = p_Button.GetComponent<Button>();
        Button m_btn = m_Button.GetComponent<Button>();
        Button d_btn = d_Button.GetComponent<Button>();
        p_btn.onClick.AddListener(ShowIncome);
        m_btn.onClick.AddListener(ShowSpendings);
        d_btn.onClick.AddListener(ChangeDateOrder);
        
        m_TransactionsList = new List<TransactionScriptableObject>();
        p_TransactionsList = new List<TransactionScriptableObject>();

        //windowGraph = WindowGraph.instance;

        prevTransPrefab = new GameObject();
        prevTransPrefabTrans = prevTransPrefab.GetComponent<Transform>();

        LoadScriptableObjects();

        for(int i = 0; i < transactions.Count; i++){
            AssignTransaction(transactions[i]);
        }

        GenerateListForDate();
    }

    void Update(){
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    void ShowIncome(){
        GenerateList(p_TransactionsList);
        prevList = 1;
    }

    void ShowSpendings(){
        GenerateList(m_TransactionsList);
        prevList = -1;
    }

    void ChangeDateOrder(){
        dateOrder = !dateOrder;
        GenerateListForDate();
    }

    public void GenerateListForDate(){
        if(prevList == 1){
            GenerateList(p_TransactionsList);
        } 
        else if(prevList == -1){
            GenerateList(m_TransactionsList);
        }
    }

    //Function called from outside to add a new transaction to a list
    public void AddTransaction(TransactionScriptableObject m_Transaction){
        transactions.Add(m_Transaction);
        AssignTransaction(m_Transaction);
        ChangeCashAmount(m_Transaction.summOfTrans);
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

        //m_Transaction.summOfTrans = -m_Transaction.summOfTrans;
        ChangeCashAmount(-m_Transaction.summOfTrans);
    }

    //Adding or substracting a new transaction summ from overall amount of money
    void ChangeCashAmount(float m_Transaction){
        cash.summOfTrans += m_Transaction;
        cashText.text = cash.summOfTrans.ToString("0.00") + "₴";
    }

    //Assigning trnsaction to "m_TransactionsList" if it is negative and to "p_TransactionsList" if it is positive
    void AssignTransaction(TransactionScriptableObject m_Transaction){
        if(m_Transaction.summOfTrans > 0f){
            p_TransactionsList.Add(m_Transaction);
            prevList = 1;
        }

        else if(m_Transaction.summOfTrans < 0f){
            m_TransactionsList.Add(m_Transaction);
            prevList = -1;
        }
    }

    //Generating a list of prefabs
    void GenerateList(List<TransactionScriptableObject> transList){
        DeleteAllChildren(transListObj.transform);
        if(transList.Count != 0){
            DateSort(transList);
            List<float> transNumbers = new List<float>();
            //windowGraph.DeleteGraphContainerDots();

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

                transNumbers.Add(transList[i].summOfTrans);
                /*else if(prevTransPrefabTrans == newTransPrefabTrans){
                    Debug.Log("фівфівфівф New: " + newTransPrefabTrans.position.y + " Prev:" + prevTransPrefabTrans.position.y);
                    string dateText = transList[i].year.ToString()+"."+transList[i].month.ToString()+"."+transList[i].day.ToString();
                    prevTransPrefabDate.text = dateText;
                    newTransPrefabDate.text = dateText;
                    newTransPrefabTrans.position = new Vector2(transListObj.transform.position.x, transListObj.transform.position.y - prefabOffsetY);
                }*/
                /*newTransPrefab.Find(DateText).text = transList[i].nameOfTrans;*/
            }
            //windowGraph.ShowGraph(transNumbers);
        }
    }

    void DateSort(List<TransactionScriptableObject> transList){
        TransactionScriptableObject temp = ScriptableObject.CreateInstance<TransactionScriptableObject>();
        //Year sorting
        for(int i = 0; i < transList.Count - 1; i++){
            for(int j = 0; j < transList.Count - i - 1; j++){
                if (transList[j].year > transList[j + 1].year){
                    // Swap elements
                    temp = transList[j];
                    transList[j] = transList[j + 1];
                    transList[j + 1] = temp;
                }
            }
        }
        
        //Month sorting
        for(int i = 0; i < transList.Count - 1; i++){
            for(int j = 0; j < transList.Count - i - 1; j++){
                if (transList[j].month > transList[j + 1].month && transList[j].year == transList[j + 1].year){
                    // Swap elements
                    temp = transList[j];
                    transList[j] = transList[j + 1];
                    transList[j + 1] = temp;
                }
            }
        }

        //Day sorting
        for(int i = 0; i < transList.Count - 1; i++){
            for(int j = 0; j < transList.Count - i - 1; j++){
                if (transList[j].day > transList[j + 1].day && transList[j].year == transList[j + 1].year && transList[j].month == transList[j + 1].month){
                    // Swap elements
                    temp = transList[j];
                    transList[j] = transList[j + 1];
                    transList[j + 1] = temp;
                }
            }
        }

        if(!dateOrder){
            transList.Reverse();
        }
    }

    //Wiping all previous prefabs from transListObj
    void DeleteAllChildren(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = childCount - 1; i >= 0; i--){
            Transform child = parent.GetChild(i);
            Destroy(child.gameObject);
        }
        
        prevTransPrefab = new GameObject();
        prevTransPrefabTrans = prevTransPrefab.GetComponent<Transform>();
        prevTransPrefabTrans.SetParent(transListObj.transform);
    }

    private void LoadScriptableObjects()
    {
        string dataPath = Application.persistentDataPath + "/";
        // Get all JSON files in the directory
        string[] jsonFiles = Directory.GetFiles(dataPath, "*.json");

        // Process the list of JSON files
        foreach (string filePath in jsonFiles)
        {
            // Read the JSON file contents
            string json = File.ReadAllText(filePath);

            // Convert JSON to ScriptableObject
            TransactionScriptableObject myData = ScriptableObject.CreateInstance<TransactionScriptableObject>();
            JsonUtility.FromJsonOverwrite(json, myData);

            // Add the ScriptableObject to the list
            transactions.Add(myData);
        }
        /*string folderPath = "Assets/Scripts/Income/ScriptableObjects/";
        string[] guids = AssetDatabase.FindAssets("t:TransactionScriptableObject", new[] { folderPath });

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            TransactionScriptableObject scrObject = AssetDatabase.LoadAssetAtPath<TransactionScriptableObject>(assetPath);

            if (scrObject != null)
            {
                transactions.Add(scrObject);
            }
        }*/
    }
}

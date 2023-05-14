using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Globalization;

public class CreateTransactoinBtn : MonoBehaviour
{
    [SerializeField] TMP_InputField nameField, amountOfMoneyField;
    [Header("Date")][SerializeField] TMP_InputField dayField; 
    [SerializeField]TMP_InputField monthField, yearField;
    [Header("Time")][SerializeField] TMP_InputField hourField; 
    [SerializeField]TMP_InputField minuteField;

    [SerializeField]Button m_Button;
    [HideInInspector]public TransactionScriptableObject trnsSO;

    TransactionManager transManager;

    void Awake(){
        Button btn = m_Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void Start(){
        transManager = TransactionManager.instance;
    }

    void OnClick(){
        trnsSO = ScriptableObject.CreateInstance<TransactionScriptableObject>();

        FillTheTransaction();

        AssetDatabase.CreateAsset(trnsSO, "Assets/Scripts/Income/ScriptableObjects/"+trnsSO.nameOfTrans+".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void FillTheTransaction(){
        trnsSO.nameOfTrans = nameField.text;
        trnsSO.summOfTrans = float.Parse(amountOfMoneyField.text, CultureInfo.InvariantCulture);
        TransactionTime();

        transManager.AddTransaction(trnsSO);
    }

    void TransactionTime(){
        trnsSO.year = int.Parse(yearField.text);
        trnsSO.month = int.Parse(monthField.text);
        trnsSO.day = int.Parse(dayField.text);
        trnsSO.hour = int.Parse(hourField.text);
        trnsSO.minute = int.Parse(minuteField.text);
    }
}

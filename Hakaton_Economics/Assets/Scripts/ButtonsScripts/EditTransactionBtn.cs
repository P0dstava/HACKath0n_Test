using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Globalization;

public class EditTransactionBtn : MonoBehaviour
{
    [SerializeField]TMP_InputField nameField, amountOfMoneyField;
    [Header("Date")][SerializeField] TMP_InputField dayField; 
    [SerializeField]TMP_InputField monthField, yearField;
    [Header("Time")][SerializeField] TMP_InputField hourField; 
    [SerializeField]TMP_InputField minuteField;

    [SerializeField]Button m_Button;
    [HideInInspector]public TransactionScriptableObject trnsSO;
    [SerializeField]TransactionScriptableObject curCash;

    [Header("Errors")][SerializeField] GameObject summFailError; 
    [SerializeField] GameObject summDecimalFailError, timeFailError, dateFailError;

    TransactionManager transManager;
    EditTransactionManager editManager;

    void Awake(){
        Button btn = m_Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);

        editManager = EditTransactionManager.instance;
    }

    void Start(){
        transManager = TransactionManager.instance;
    }

    void OnClick(){
        FillTheTransaction();
    }

    void FillTheTransaction(){
        trnsSO = editManager.tsObj;

        transManager.RemoveTransaction(trnsSO);
        
        if(nameField.text != "")
            trnsSO.nameOfTrans = nameField.text;
        
        CheckForNumber();

        TransactionTime();

        transManager.AddTransaction(trnsSO);

        transManager.GenerateListForDate();
    }

    void TransactionTime(){
        if(yearField.text != ""){ 
            if(yearField.text.Length == 4)
                trnsSO.year = int.Parse(yearField.text);
            else
                dateFailError.SetActive(true);
        }
        if(monthField.text != ""){  
            if(monthField.text.Length <= 2 && monthField.text.Length > 0)
                trnsSO.month = int.Parse(monthField.text);
            else
                dateFailError.SetActive(true);
        }
        if(dayField.text != ""){  
            if(dayField.text.Length <= 2 && dayField.text.Length > 0)
                trnsSO.month = int.Parse(dayField.text);
            else
                dateFailError.SetActive(true);
        }
        if(hourField.text != ""){  
            if(hourField.text.Length <= 2 && hourField.text.Length > 0)
                trnsSO.month = int.Parse(hourField.text);
            else
                timeFailError.SetActive(true);
        }
        if(minuteField.text != ""){  
            if(minuteField.text.Length <= 2 && minuteField.text.Length > 0)
                trnsSO.month = int.Parse(minuteField.text);
            else
                timeFailError.SetActive(true);
        }
    }

    bool CheckIfMoreThanTwoAfterDot(float inputSumm){
        bool hasMoreThanTwoDecimalPlaces = false;
        if(Mathf.Abs(inputSumm - Mathf.Round(inputSumm)) > float.Epsilon){
            string stringValue = inputSumm.ToString(System.Globalization.CultureInfo.InvariantCulture);

            int decimalPlaces = stringValue.Length - stringValue.IndexOf('.') - 1;

            hasMoreThanTwoDecimalPlaces = decimalPlaces > 2;
        }   
        return hasMoreThanTwoDecimalPlaces;
    }

    void CheckForNumber(){
        bool foundComma = false;

        foreach (char c in amountOfMoneyField.text){
            if (c == ','){
                foundComma = true;
                break;
            }
        }

        if(amountOfMoneyField.text != ""){
            if(!foundComma){
                float inputSumm = float.Parse(amountOfMoneyField.text, CultureInfo.InvariantCulture);
                bool moreThanTwoNumbers = CheckIfMoreThanTwoAfterDot(Mathf.Abs(inputSumm));

                if(moreThanTwoNumbers == false){
                    if(inputSumm > 0f){
                        trnsSO.summOfTrans = inputSumm;
                    }
                    else if(inputSumm < 0f && (curCash.summOfTrans + inputSumm) > 0f){
                        trnsSO.summOfTrans = inputSumm;
                    }
                    else{
                        summFailError.SetActive(true);
                    }
                }
                else{
                    summDecimalFailError.SetActive(true);
                }
            }
            else{
                summDecimalFailError.SetActive(true);
            }
        }
    }
}

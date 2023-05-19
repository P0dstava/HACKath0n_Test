using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class CreateTransactoinBtn : MonoBehaviour
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

    const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    string generatedString = "", fileName = "";
    List<string> previousNames = new List<string>();

    bool createble = true;

    void Awake(){
        Button btn = m_Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void Start(){
        transManager = TransactionManager.instance;
    }

    void OnClick(){
        TurnOffErrors();

        trnsSO = ScriptableObject.CreateInstance<TransactionScriptableObject>();

        FillTheTransaction();
        if(createble){
            fileName = GenerateRandomString(8);
            RegenerateIfMatch(fileName);

            SaveScriptableObjectAsJSON(trnsSO, fileName);
            /*AssetDatabase.CreateAsset(trnsSO, "Assets/Scripts/Income/ScriptableObjects/"+fileName+".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
        }
    }

    void SaveScriptableObjectAsJSON(TransactionScriptableObject transaction, string fileName)
    {

        // Serialize the instantiated ScriptableObject to JSON
        string json = JsonUtility.ToJson(transaction);
        // Save the JSON string to a file with the custom name
        transaction.tagOfTrans = fileName;
        string filePath = Application.persistentDataPath + "/" + fileName +".json";
        System.IO.File.WriteAllText(filePath, json);
    }

    string GenerateRandomString(int length)
    {
        string randomString = "";
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(0, characters.Length);
            randomString += characters[randomIndex];
        }


        return randomString;
    }

    void RegenerateIfMatch(string targetString)
    {
        if(previousNames.Contains(targetString) || generatedString == targetString)
        {
            generatedString = GenerateRandomString(8);
            RegenerateIfMatch(generatedString);
        }
    }

    void FillTheTransaction(){
        if(nameField.text != "")
            trnsSO.nameOfTrans = nameField.text;
        else
            trnsSO.nameOfTrans = "Транзакція";

        createble = true;
        
        //Checking amount of money
        bool foundComma = false;

        foreach (char c in amountOfMoneyField.text){
            if (c == ','){
                foundComma = true;
                break;
            }
        }

        if(!foundComma){
            float inputSumm = float.Parse(amountOfMoneyField.text, CultureInfo.InvariantCulture);
            bool moreThanTwoNumbers = CheckIfMoreThanTwoAfterDot(Mathf.Abs(inputSumm));

            Debug.Log(moreThanTwoNumbers);

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
                createble = false;
            }
        }
        else{
            summDecimalFailError.SetActive(true);
            createble = false;
        }
        //Checking time
        TransactionTime();

        if(createble){
            transManager.AddTransaction(trnsSO);

            transManager.GenerateListForDate();
        }
    }

    void TransactionTime(){
        if(yearField.text.Length == 4 && (monthField.text.Length <= 2 && monthField.text.Length > 0) && (dayField.text.Length <= 2 && dayField.text.Length > 0)){
            trnsSO.year = int.Parse(yearField.text);
            trnsSO.month = int.Parse(monthField.text);
            trnsSO.day = int.Parse(dayField.text);
        }
        else{
            dateFailError.SetActive(true);
            createble = false;
        }

        if((hourField.text.Length <= 2 && hourField.text.Length > 0) && (minuteField.text.Length <= 2 && minuteField.text.Length > 0)){
            trnsSO.hour = int.Parse(hourField.text);
            trnsSO.minute = int.Parse(minuteField.text);
        }
        else{
            timeFailError.SetActive(true);
            createble = false;
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

    void TurnOffErrors(){
        timeFailError.SetActive(false);
        dateFailError.SetActive(false);
        summFailError.SetActive(false);
        summDecimalFailError.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Transaction", menuName = "Transactions/New Transaction")]
public class TransactionScriptableObject : ScriptableObject
{
    [Header("Name of Transaction")]
    public string nameOfTrans = "testName";
    public string tagOfTrans = "";
    [Header("Summ of Transaction")]
    public float summOfTrans = 0f;
    [Header("Date and Time of Transaction")]
    public int year;
    public int month, day, hour, minute;

    void Awake(){
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transaction", menuName = "Transactions/New Transaction")]
public class TransactionScriptableObject : ScriptableObject
{
    public string nameOfTrans = "testName", tagOFTrans = "";
    public float summOfTrans = 0f;
    public int year, month, day, hour, minute;

    void Awake(){
        Debug.Log(year+"/"+month+"/"+day+" "+hour+":"+minute);
    }
}

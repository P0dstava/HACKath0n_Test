using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class CreditsManager : MonoBehaviour
{
    public static CreditsManager instance;

    [SerializeField] Credit creditPrefab;
    [SerializeField] Transform content;
    [SerializeField] TMP_Text sumText;

    private int sum; 

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Instance freak up!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        sumText.text = sum.ToString();
    }

    private void OnEnable()
    {
        Credit.OnDelete -= DeleteCredit;
        Credit.OnDelete += DeleteCredit;
    }

    public void CreateCredit(string name, int monthlyPay, int months)
    {
        Instantiate(creditPrefab, content).Init(name, monthlyPay, months);
        sum += monthlyPay;
        sumText.text = sum.ToString();
    }

    private void DeleteCredit(int pay)
    {
        sum -= pay;
        sumText.text = sum.ToString();
    }
}
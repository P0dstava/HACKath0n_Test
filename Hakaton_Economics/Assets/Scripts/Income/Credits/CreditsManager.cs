using System;
using System.Linq;
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
    private List<Credit> _credits;

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
        sumText.text = sum.ToString("0.00") + "₴";
        _credits = new List<Credit>();
    }

    private void OnEnable()
    {
        Credit.OnDelete -= DeleteCredit;
        Credit.OnDelete += DeleteCredit;
    }

    public void CreateCredit(string name, int monthlyPay, int months)
    {
        Credit credit = Instantiate(creditPrefab, content);
        credit.Init(name, monthlyPay, months);
        _credits.Add(credit);
        sum += monthlyPay;
        sumText.text = sum.ToString("0.00") + "₴";
    }

    private void DeleteCredit(Credit credit)
    {
        sum -= credit._pay;
        sumText.text = sum.ToString("0.00") + "₴";
        _credits.Remove(credit);
    }

    public void SortByPay()
    {
        _credits = _credits.OrderBy(obj => obj._pay).Reverse().ToList();
        for (int i = 0; i < _credits.Count; i++)
            _credits[i].transform.SetSiblingIndex(i);
    }

    public void SortByMonths()
    {
        _credits = _credits.OrderBy(obj => obj._months).Reverse().ToList();
        for (int i = 0; i < _credits.Count; i++)
            _credits[i].transform.SetSiblingIndex(i);
    }
}
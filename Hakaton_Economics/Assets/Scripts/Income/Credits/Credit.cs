using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credit : MonoBehaviour
{
    public static event Action<Credit> OnDelete;

    [SerializeField] private TMP_Text _name, _payment, _month;

    public int _pay;
    public int _months;

    public void Init(string name, int payment, int months)
    {
        _name.text = name;
        _payment.text = payment.ToString() + "₴\n/month";
        _month.text = months.ToString() + " months left";

        _pay = payment;
        _months = months;
    }

    public void Delete()
    {
        OnDelete?.Invoke(this);
        Destroy(gameObject);
    }
}
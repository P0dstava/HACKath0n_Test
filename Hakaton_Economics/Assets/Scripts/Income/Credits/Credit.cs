using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credit : MonoBehaviour
{
    public static event Action<int> OnDelete;

    [SerializeField] private TMP_Text _name, _payment, _month;

    private int _pay;

    public void Init(string name, int payment, int month)
    {
        _name.text = name;
        _payment.text = payment.ToString() + "₴/month";
        _month.text = month.ToString() + " months left";

        _pay = payment;
    }

    public void Delete()
    {
        OnDelete?.Invoke(_pay);
        Destroy(gameObject);
    }
}
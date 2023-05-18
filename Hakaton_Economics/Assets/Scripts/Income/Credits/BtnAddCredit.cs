using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class BtnAddCredit : MonoBehaviour
{
    private UnityAction<string, int, int> Add;
    [SerializeField] private TMP_InputField _name, _money, _rate, _months;
    [SerializeField] private CreditsManager _manager;
    [SerializeField] private Clear _clear;

    public void Create()
    {
        bool flag1 = int.TryParse(_months.text, out int months);
        bool flag2 = int.TryParse(_money.text, out int money);
        bool flag3 = float.TryParse(_rate.text, out float rate);

        if (!flag1 || !flag2 || !flag3)
        {
            _clear.ClearAll();
            return;
        }

        rate /= 100;

        float total = money + (money * rate);

        float monthlyPay = total / months;
        //float monthlyPay = money * (rate * Mathf.Pow(1 + rate, months)) / (Mathf.Pow(1 + rate, months) - 1);

        _manager.CreateCredit(_name.text, (int)monthlyPay, months);

        _clear.ClearAll();
    }
}
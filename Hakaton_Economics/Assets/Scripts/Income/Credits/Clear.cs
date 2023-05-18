using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clear : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] _inputs;
    public void ClearAll()
    {
        foreach (var item in _inputs)
            item.text = string.Empty;
    }
}
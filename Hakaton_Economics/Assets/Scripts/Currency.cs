using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class Currency : MonoBehaviour
{
    private const string URL = "https://api.apilayer.com/fixer/latest?symbols=USD%2CEUR%2CPLN&base=UAH";

    [SerializeField] private TMP_Text _usd;
    [SerializeField] private TMP_Text _eur;
    [SerializeField] private TMP_Text _pln;

    private void Start()
    {
        //disabled for API calls economy 

        //StartCoroutine(Get());
    }

    private IEnumerator Get()
    {
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
        //request.Timeout = -1;
        //request.Headers.Add("apikey", "Nz86dSl8Gv3wte5654XvMbP9ixH3lcja");
        //Stream stream = request.GetRequestStream();
        //yield return stream;
        //StreamReader reader = new StreamReader(stream);
        //JsonUtility.FromJson<string>(reader.ReadToEnd());
        //Debug.Log(JsonUtility.FromJson<string>(reader.ReadToEnd()));

        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            request.SetRequestHeader("apikey", "Nz86dSl8Gv3wte5654XvMbP9ixH3lcja");
            Debug.Log(1);
            yield return request.SendWebRequest();
            Debug.Log(2);
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(request.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(3);
                    SetCurrency(JsonConvert.DeserializeObject<CurrencyData>(request.downloadHandler.text));
                    break;
                default:
                    Debug.Log(0);
                    break;
            }
        }
    }

    private void SetCurrency(CurrencyData currency)
    {
        _usd.text = currency.rates.USD.ToString("f2");
        _eur.text = currency.rates.EUR.ToString("f2");
        _usd.text = currency.rates.PLN.ToString("f2");
    }
}

class CurrencyData
{
    public string _base { get; set; }
    public string date { get; set; }
    public Rates rates { get; set; }
    public bool success { get; set; }
    public int timestamp { get; set; }
}

public class Rates
{
    public double EUR { get; set; }
    public double PLN { get; set; }
    public double USD { get; set; }
}
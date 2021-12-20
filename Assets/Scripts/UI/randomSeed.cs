using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class randomSeed : MonoBehaviour
{
    private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public void SetRandomSeed()
    {
        string text = "";
        for (int i = 0; i < 15; i++)
        {
            int value = Random.Range(0, 3);
            switch (value)
            {
                case 0:
                    text += chars[Random.Range(0, 26)];
                    break;
                case 1:
                    text += chars.ToLower()[Random.Range(0, 26)];
                    break;
                case 2:
                    text += Random.Range(0, 10);
                    break;
                default:
                    text += "ERROR";
                    break;
            }
        }
        GetComponent<TMP_InputField>().text = text;
    }
}

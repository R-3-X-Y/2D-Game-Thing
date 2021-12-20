using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sliderOutput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI outputText;
    void Update()
    {
        outputText.text = GetComponent<Slider>().value.ToString();
    }
}

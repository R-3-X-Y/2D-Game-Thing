using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldCreationData : MonoBehaviour
{
    public string seed;
    public int width, height;
    [SerializeField] private TextMeshProUGUI seedText;
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;

    public static WorldCreationData instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void GetData()
    {
        seed = seedText.text;
        width = Mathf.RoundToInt(widthSlider.value);
        height = Mathf.RoundToInt(heightSlider.value);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        text.text = "High Score: " + PlayerPrefs.GetFloat("HighScore");
    }
}

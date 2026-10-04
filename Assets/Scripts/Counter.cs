using System;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public void SetScore(int value)
    {
        scoreText.text = $"Score: {value}";
    }
}

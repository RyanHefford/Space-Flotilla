using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScore : MonoBehaviour
{
    public Text scoreText;

    private void Update()
    {
        scoreText.text = "Current Score: " + Score.score;
    }
}

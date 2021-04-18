using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int score;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    private void Update()
    {
        scoreText.text = "Score: " + score;
    }

    public void updateScore(int _score)
    {
        score += _score;
    }

}

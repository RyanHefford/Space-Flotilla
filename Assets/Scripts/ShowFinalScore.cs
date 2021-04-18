using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFinalScore : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "SCORE: " + Score.score;
    }
}

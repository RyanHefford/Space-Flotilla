using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("PlayerScore").GetComponent<TextMeshProUGUI>().text = "SCORE: " + Score.score;
    }

    public void ReturnToMenu()
    {
        Score.score = 0;
        SceneManager.LoadScene(0);
    }
}

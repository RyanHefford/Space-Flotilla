using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        EditorSceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

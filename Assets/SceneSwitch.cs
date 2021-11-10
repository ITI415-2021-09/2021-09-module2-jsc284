using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("__Prospector_Scene");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("GameScene");
    }
}
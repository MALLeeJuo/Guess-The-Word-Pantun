using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ChangeScene : MonoBehaviour
{
    // Load scene 
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Delete PlayerPref
    public void DeleteSaves()
    {
        Debug.Log("Cleared All Playerprefs");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}

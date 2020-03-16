using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour {

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C) && SceneManager.GetActiveScene().name == "scene1")
            SceneManager.LoadScene("scene2");
        if (Input.GetKeyDown(KeyCode.C) && SceneManager.GetActiveScene().name == "scene2")
            SceneManager.LoadScene("scene1");
    }
}

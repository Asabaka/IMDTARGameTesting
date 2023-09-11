using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BttnEvs : MonoBehaviour
{
    public void ChangeSceneBtt(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}

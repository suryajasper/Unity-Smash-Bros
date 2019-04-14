using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backtoMenu : MonoBehaviour
{
    public void GotoMenu()
    {
        SceneManager.LoadScene(0);
    }
}

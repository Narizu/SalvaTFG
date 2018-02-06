using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public void OpenScene0 ()
    {

        print("LOAD SCENE 0: MENU");
        SceneManager.LoadScene(0);

    }

    public void OpenScene1 ()
    {

        print("LOAD SCENE 1: GAME");
        SceneManager.LoadScene(1);

    }

    public void OpenScene2 ()
    {

        print("LOAD SCENE 2: OPTIONS");
        SceneManager.LoadScene(2);

    }

    public void ExitGame ()
    {

        print("GAME OVER");
        Application.Quit();

    }

}

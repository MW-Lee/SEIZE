using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_in : MonoBehaviour
{

    void Awake()
    {
    }
    public void StartButton()
    {
        Invoke("startgame", .2f);
    }
    void startgame()
    {
        Application.LoadLevel("Main");
    }
}

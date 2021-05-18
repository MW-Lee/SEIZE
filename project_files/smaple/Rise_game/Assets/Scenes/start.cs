using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour {

    void Awake()
    {  
    }
    public void StartButton()
    {
        Invoke("startgame",.2f);
    }
    void startgame()
    {
        Application.LoadLevel("Captur");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DisplayControl;
public class DeveloperKeys : ClientConsole
{
    private Transform consoleBox;
    private void Start()
    {
        consoleBox = this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return))
        {
            callToClientDisplay("Client receives this message", consoleBox);
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return))
        {
            callToClientDisplay("Client receives this message", consoleBox);
        }
    }
}

using UnityEngine;

public class Intro : MonoBehaviour
{

    void OnGUI()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        
        int h = 75;
        float w = (h * 16.0f) / 9;
        Rect r = new Rect((screenWidth - w) / 2, (screenHeight - h) / 2, w, h);
        if (GUI.Button(r, "Start"))
            Application.LoadLevel(1);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Cursor.visible = false;

        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 1;
    }



}

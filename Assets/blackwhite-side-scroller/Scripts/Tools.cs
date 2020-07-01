using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public bool CursorVisible;
    public int LimitFrameRate;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Cursor.visible = CursorVisible;

        if (LimitFrameRate != 0)
        {
            Application.targetFrameRate = LimitFrameRate;
            QualitySettings.vSyncCount = 1;
        }
    }



}

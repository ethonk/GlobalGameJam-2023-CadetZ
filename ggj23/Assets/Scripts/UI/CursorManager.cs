using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursors")]
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorAiming;

    private void Start()
    {
        CursorToNormal();
    }

    public void CursorToNormal()
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void CursorToAim()
    {
        Cursor.SetCursor(cursorAiming, Vector2.zero, CursorMode.ForceSoftware);
    }
}

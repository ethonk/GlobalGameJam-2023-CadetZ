using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursors")]
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorNormalCd;
    [SerializeField] private Texture2D cursorAiming;

    // manager instance
    private static CursorManager _instance;
    public static CursorManager Instance { get { return _instance; }}
    
    private void Start()
    {
        CursorToNormal();
    }
    
    /// <summary>
    /// Ensures that the Cursor Manager only ever exists once.
    /// </summary>
    private void Awake()
    {
        // set the instance
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public void CursorToNormal()
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void CursorToNormalCd()
    {
        Cursor.SetCursor(cursorNormalCd, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    public void CursorToAim()
    {
        Cursor.SetCursor(cursorAiming, Vector2.zero, CursorMode.ForceSoftware);
    }

}

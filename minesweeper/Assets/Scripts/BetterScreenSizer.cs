using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetterScreenSizer : MonoBehaviour
{
    public Vector2 aspectRatio = new Vector2(700, 660);
    public bool debug = false;
    public RectTransform minefield;
    public Vector2Int padding;
    public CanvasScaler canvasScaler;
    public CanvasScaler menuScaler;
    private Vector2Int lastResolution;
    private bool setting;
    private bool forceResize;

    private void Start()
    {
        lastResolution = new Vector2Int(Screen.width, Screen.height);
        Game.GetInstance().OnInit += ForceResize;
    }

    public void ForceResize()
    {
        forceResize = true;
    }

    private void Update()
    {
        var resolution = new Vector2Int(Screen.width, Screen.height);
        if (debug)
            Debug.Log("Screen: " + resolution);

        //#if !UNITY_EDITOR
        if (!setting)
        {
            aspectRatio = new Vector2(minefield.sizeDelta.x + padding.x, minefield.sizeDelta.y + padding.y);
            if (resolution.x != lastResolution.x)
            {
                float h = resolution.x * (aspectRatio.y / aspectRatio.x);
                if (h > 900)
                    h /= 2;

                StartCoroutine(SetResolution(resolution.x, (int)h / 2));
            }
            else if (resolution.y != lastResolution.y || forceResize)
            {
                float w = resolution.y * (aspectRatio.x / aspectRatio.y);
                if (w > 900)
                    w /= 2;

                StartCoroutine(SetResolution((int) w / 2, resolution.y));
            }
        }
        //#endif
    }
    IEnumerator SetResolution(int w, int h)
    {
        setting = true;
        forceResize = false;
        Vector2 scale = new Vector2(minefield.sizeDelta.x + padding.x, minefield.sizeDelta.y + padding.y);
        canvasScaler.referenceResolution = scale;
        menuScaler.referenceResolution = scale;
        yield return new WaitForSeconds(.5f);
        lastResolution = new Vector2Int(w, h);
        setting = false;
    }
}

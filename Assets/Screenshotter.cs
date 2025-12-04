using System;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeScreenshot();
        }
    }

    private static void TakeScreenshot()
    {
        var screenshotName = $"/Users/jacobedwards/Desktop/screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        ScreenCapture.CaptureScreenshot(screenshotName);
        Debug.Log($"Screenshot taken: {screenshotName}");
    }
}
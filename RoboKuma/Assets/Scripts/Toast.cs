using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Toast : MonoBehaviour
{
    private string toastString;
    private AndroidJavaObject currentActivity;

    public void showToastOnUiThread(string toastString)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            this.toastString = toastString;

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
        }
    }

    public void showToast()
    {
        Debug.Log("Running on UI thread");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }
}


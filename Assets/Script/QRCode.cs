using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.SearchService;
using UnityEngine;
using ZXing;

public class QRCode : MonoBehaviour
{
    // Start is called before the first frame update
    private WebCamTexture myCam;
    private BarcodeReader reader = new BarcodeReader();
    private Result res;
    private bool flag = true;
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log("Camera " + i + ": " + devices[i].name);
        }
        StartCoroutine(open_Camera());
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (myCam !=  null)
        {
            if (myCam.isPlaying == true) 
            {
                GUI.DrawTexture(new Rect(0, 0, 600, 800), myCam);
                if(flag == true)
                {
                    StartCoroutine(scan());
                }
            }
        }
    }

    private IEnumerator open_Camera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            myCam = new WebCamTexture(WebCamTexture.devices[0].name, 600, 800, 30);
            myCam.Play();
        }
    }

    private IEnumerator scan()
    {
        flag = false;
        Texture2D t2D = new Texture2D(600, 800);
        yield return new WaitForEndOfFrame();
        t2D.ReadPixels(new Rect(0, 0, 600, 800), 0, 0, false);
        t2D.Apply();

        res = reader.Decode(t2D.GetPixels32(), t2D.width, t2D.height);

        if (res != null)
        {
            Debug.Log(res.Text);
        }

        flag = true;

    }

    void OnDisable()
    {
        myCam.Stop();
    }
}


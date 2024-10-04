using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamSetup : MonoBehaviour
{
    WebCamTexture webcamTexture;
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice my_device = new WebCamDevice();
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
            my_device = devices[1];
        }
        webcamTexture = new WebCamTexture(my_device.name, 1920, 1080);
        rawImage.texture = webcamTexture;
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

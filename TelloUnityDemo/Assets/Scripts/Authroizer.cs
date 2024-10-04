using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authroizer : MonoBehaviour
{
   
    GameObject suspendedObject;
    // Start is called before the first frame update
    void Start()
    {
        suspendedObject = new GameObject();

        #if UNITY_WEBPLAYER || UNITY_FLASH
            yield Application.RequestUserAuthorization(UserAuthroization.WebCam)
        #endif 

        suspendedObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

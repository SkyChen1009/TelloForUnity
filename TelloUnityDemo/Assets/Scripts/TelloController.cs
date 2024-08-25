using System.Collections;
using UnityEngine;
using TelloLib;
using UnityEngine.UI;

public class TelloController : SingletonMonoBehaviour<TelloController>
{
    private static bool isLoaded = false;

    public TelloVideoTexture telloVideoTexture;
    public float lx = 0f;
    public float ly = 0f;
    public float rx = 0f;
    public float ry = 0f;

    public DronePositionTracker positionTracker;

    //// UI representing movement
    //public Image actionImage;
    //public Sprite goUpSprite;
    //public Sprite goDownSprite;
    //public Sprite tiltLeftSprite;
    //public Sprite tiltRightSprite;
    //public Sprite goForwardSprite;
    //public Sprite goBackwardSprite;
    //public Sprite goLeftSprite;
    //public Sprite goRightSprite;
    //public Sprite takeoffSprite;
    //public Sprite landSprite;
    //public Sprite stillSprite;

    //private Coroutine resetSpriteCoroutine;

    // FlipType is used for the various flips supported by the Tello.
    public enum FlipType
    {

        // FlipFront flips forward.
        FlipFront = 0,

        // FlipLeft flips left.
        FlipLeft = 1,

        // FlipBack flips backwards.
        FlipBack = 2,

        // FlipRight flips to the right.
        FlipRight = 3,

        // FlipForwardLeft flips forwards and to the left.
        FlipForwardLeft = 4,

        // FlipBackLeft flips backwards and to the left.
        FlipBackLeft = 5,

        // FlipBackRight flips backwards and to the right.
        FlipBackRight = 6,

        // FlipForwardRight flips forewards and to the right.
        FlipForwardRight = 7,
    };

    // VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
    public enum VideoBitRate
    {
        // VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
        VideoBitRateAuto = 0,

        // VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
        VideoBitRate1M = 1,

        // VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
        VideoBitRate15M = 2,

        // VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
        VideoBitRate2M = 3,

        // VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
        VideoBitRate3M = 4,

        // VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
        VideoBitRate4M = 5,

    };

    //private GameObject Slate;
    //private UnityEngine.UI.Image SlateImage;

    override protected void Awake()
    {
        if (!isLoaded)
        {
            DontDestroyOnLoad(this.gameObject);
            isLoaded = true;
        }
        base.Awake();

        Tello.onConnection += Tello_onConnection;
        Tello.onVideoData += Tello_onVideoData;

        // 找到 Slate 物件和 ContentQuad 元件
        //Slate = GameObject.Find("Slate");
        //SlateImage = Slate.GetComponent<UnityEngine.UI.Image>();

        //if (telloVideoTexture == null)
        //    telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

        //positionTracker = new DronePositionTracker();
    }

    //private void OnEnable()
    //{
    //    if (telloVideoTexture == null)
    //        telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
    //}

    //public GameObject telloVideoPrefab;
    //public GameObject telloVideoInstance;

    private void Start()
    {
        //---------------------------------------------
        //if (telloVideoTexture == null)
        //    telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

        //if (telloVideoPrefab != null)
        //{
        //    telloVideoInstance = Instantiate(telloVideoPrefab);
        //}

        Tello.startConnecting();
    }

    void OnApplicationQuit()
    {
        Tello.stopConnecting();
    }

    ////Sprites 延長顯示時間
    //void ResetSpriteAfterDelay()
    //{
    //    if (resetSpriteCoroutine != null)
    //    {
    //        StopCoroutine(resetSpriteCoroutine);
    //    }
    //    resetSpriteCoroutine = StartCoroutine(ResetSpriteCoroutine());
    //}

    ////Sprites 每個動作顯示兩秒後切回
    //IEnumerator ResetSpriteCoroutine()
    //{
    //    yield return new WaitForSeconds(3f);
    //    actionImage.sprite = stillSprite;
    //}
    public void Takeoff() //起飛
    {
        Debug.Log("Take off");
        Tello.takeOff();
        //actionImage.sprite = takeoffSprite;
        //ResetSpriteAfterDelay();
    }

    public void Land() //降落
    {
        Debug.Log("Land");
        Tello.land();
        //actionImage.sprite = landSprite;
        //ResetSpriteAfterDelay();
    }

    //public Text droneDistanceText;
    //public Text droneAltitudeText;

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.T)) {
         Tello.takeOff();
        } else if (Input.GetKeyDown(KeyCode.L)) {
         Tello.land();
        }

        float lx = 0f;
        float ly = 0f;
        float rx = 0f;
        float ry = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) {
         ry = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
         ry = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
         rx = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
         rx = -1;
        }
        if (Input.GetKey(KeyCode.W)) {
         ly = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
         ly = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
         lx = 1;
        }
        if (Input.GetKey(KeyCode.A)) {
         lx = -1;
        }*/
        Debug.Log($"lx:{lx}, ly: {ly}, rx: {rx}, ry: {ry}");
        //SetAxisValues(lx, ly, rx, ry);
    }

    //private float droneDistance = 0f;
    //private float droneAltitude = 0f;

    public void OnMoveUpPress() //往上
    {
        Debug.Log("up");
        ly = 0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, 0.5f, 0f, 0f);
        //actionImage.sprite = goUpSprite;
    }

    public void OnMoveUpRelease()
    {
        ly = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnMoveDownPress() //往下
    {
        Debug.Log("down");
        ly = -0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, -0.5f, 0f, 0f);
        //actionImage.sprite = goDownSprite;
    }

    public void OnMoveDownRelease()
    {
        ly = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnTurnLeftPress() //左旋轉
    {
        Debug.Log("turn left");
        lx = -1f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(-1f, 0f, 0f, 0f);
        //.sprite = tiltLeftSprite;
    }

    public void OnTurnLeftRelease()
    {
        lx = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnTurnRightPress() //右旋轉
    {
        Debug.Log("turn right");
        lx = 1f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(1f, 0f, 0f, 0f);
        //actionImage.sprite = tiltRightSprite;
    }

    public void OnTurnRightRelease()
    {
        lx = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnMoveForwardPress() //往前
    {
        Debug.Log("forward");
        ry = 0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, 0f, 0f, 0.5f);
        //actionImage.sprite = goForwardSprite;
    }

    public void OnMoveForwardRelease()
    {
        ry = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnMoveLeftPress()       //往左飛
    {
        Debug.Log("move left");
        rx = -0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, 0f, -0.5f, 0f);
        //actionImage.sprite = goLeftSprite;
    }

    public void OnMoveLeftRelease()
    {
        rx = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnMoveRightPress()      //往右飛
    {
        Debug.Log("move right");
        rx = 0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, 0f, 0.5f, 0f);
        //actionImage.sprite = goRightSprite;
    }

    public void OnMoveRightRelease()
    {
        rx = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    public void OnMoveBackwardPress() //往後
    {
        Debug.Log("backward");
        ry = -0.5f;
        //Debug.Log($"Press lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        SetAxisValues(0f, 0f, 0f, -0.5f);
        //actionImage.sprite = goBackwardSprite;
    }

    public void OnMoveBackwardRelease()
    {
        ry = 0f;
        //Debug.Log($"Release lx:{lx}, ly:{ly}, rx:{rx}, ry:{ry}");
        ClearAxisValues();
        //actionImage.sprite = stillSprite;
    }

    private void SetAxisValues(float lx, float ly, float rx, float ry)
    {
        Debug.Log($"Tello: lx: {Tello.controllerState.lx}, ly: {Tello.controllerState.ly}, rx: {Tello.controllerState.rx}, ry: {Tello.controllerState.ry}");
        Tello.controllerState.setAxis(lx, ly, rx, ry);
        //positionTracker.UpdatePosition(lx, ly, rx, ry);
    }

    public void ClearAxisValues()
    {
        Debug.Log("Clearing axis values");
        Tello.controllerState.setAxis(0f, 0f, 0f, 0f);
    }

    public class DronePositionTracker
    {
        /*
        public float accumulatedLX { get; private set; }
        public float accumulatedLY { get; private set; }
        public float accumulatedRX { get; private set; }
        public float accumulatedRY { get; private set; }

        public void UpdatePosition(float lx, float ly, float rx, float ry)
        {
            accumulatedLX += lx;
            accumulatedLY += ly;
            accumulatedRX += rx;
            accumulatedRY += ry;
        }

        /*public void ResetPosition()
        {
            accumulatedLX = 0f;
            accumulatedLY = 0f;
            accumulatedRX = 0f;
            accumulatedRY = 0f;
        }*/
    }


    //public void UpdateSlateTexture(Texture2D texture)
    //{
    //    // 更新 Slate 的 Image 屬性
    //    SlateImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    //}

    private void Tello_onConnection(Tello.ConnectionState newState)
    {
        if (newState == Tello.ConnectionState.Connected)
        {
            Tello.queryAttAngle();
            Tello.setMaxHeight(50);

            Tello.setPicVidMode(1); // 0: picture, 1: video
            Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
            //Tello.setEV(0);
            Tello.requestIframe();
        }
    }

    private void Tello_onVideoData(byte[] data)
    {
        // Debug.Log("Tello_onVideoData: " + data.Length);
        // 將影像資料傳遞給 TelloVideoTexture 以更新影像
        if (telloVideoTexture != null)
        {
            telloVideoTexture.PutVideoData(data);

            //if (telloVideoInstance != null)
            //{
            //    telloVideoInstance.GetComponent<RawImage>().texture = telloVideoTexture.VideoTexture;
            //}
        }

    }
}
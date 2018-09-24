using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;

public class Ball : MonoBehaviour
{
    private float ballPos, startPos;

    public GameObject _fireBed;
    public GameObject[] _powerObj;
    public static int _shieldTime = 0;
    public PhysicsMaterial2D[] ballMat;
    public bool startd;
    public static int _shieldNo = 0;
    public static int _magNo = 0;
    public static int _bounceNo = 0;
    public static bool isPaused = false;
    public Text resume;
    public Text quit;
    public Camera _camera;
    public static bool isJumpd = false;
    public static float t;
    public AudioSource audSrc;
    public AudioClip src;
    private FollowPath fp;
    public static bool p ;
    private string _tenSeconds = "CgkIuKz91ZEWEAIQCQ";
    private string _thirtySeconds = "CgkIuKz91ZEWEAIQAQ";
    private string _sixtySeconds = "CgkIuKz91ZEWEAIQCg";
    private string _survivor = "CgkIuKz91ZEWEAIQAw";
    private string _loneWolf = "CgkIuKz91ZEWEAIQBA";
    private string _fifteenMin = "CgkIuKz91ZEWEAIQBw";
    private string _magLover = "CgkIuKz91ZEWEAIQBQ";
    private string _shieldLover = "CgkIuKz91ZEWEAIQCw";
    private string _bounce = "CgkIuKz91ZEWEAIQBg";
    private string _twentyMins = "CgkIuKz91ZEWEAIQDA";
    private string _thirtyMins = "CgkIuKz91ZEWEAIQDQ";
    private float _fbedx, _fbedy, _fbedz;
    // Use this for initialization
    void Start()
    {
       
        _fbedx = _fireBed.transform.position.x;
        _fbedy = _fireBed.transform.position.y;
        _fbedz = _fireBed.transform.position.z;
        p = true;
        fp = GetComponent<FollowPath>();
        t = 0f;
        print("x = " + resume.transform.position.x + ", Y = " + resume.transform.position.x);
        //resume.transform.position = new Vector3 (resume.transform.position.x / 17.1567f, resume.transform.position.y / 17.1567f, -1.4f);
        startPos = this.transform.position.x - 2.9f;
        //print("Start"+startPos);
         
    }
    void OnLevelWasLoaded()
    {
        ObstacleSpawn.force = 1.0f;
        Powers.Reset();
    }

    // Update is called once per frame
    void Update()

    {
        
      

        startd = BackgroundScroll.startd;

        if (!isPaused)
        {
            if (startd)
            {
                if(p)
                {
                    p = false;
                    InvokeRepeating("SpawnPower", 5.0f, 5.0f);
                   
                }
               
                t += Time.deltaTime;
                Score.score = (int)t;
               
                if (!isJumpd)
                {

                    //this.GetComponent<Rigidbody2D> ().gravityScale = 1;

                    if (Input.touchSupported)
                    {
                        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            ballPos = (Input.GetTouch(0).position.x / Screen.width * 7.5f) + (startPos);
                            this.GetComponent<Rigidbody2D>().velocity = new Vector2(ballPos, 7f);
                            audSrc.clip = src;
                            audSrc.Play();
                            isJumpd = true;
                        }
                    }
                    else {
                        if (Input.GetMouseButtonDown(0))
                        {
                            ballPos = (Input.mousePosition.x / Screen.width * 7.5f) + (startPos);
                            print(ballPos);
                            this.GetComponent<Rigidbody2D>().velocity = new Vector2(ballPos, 7f);
                            audSrc.clip = src;
                            audSrc.Play();
                            isJumpd = true;
                        }
                    }

                }
                if ((int)t - _shieldTime == 5 && Powers.ShieldActivated)
                {
                    _fireBed.transform.position = new Vector3(_fbedx, _fbedy, _fbedz);
                    Powers.ShieldActivated = false;
                }
                if (Powers.ShieldActivated)
                {
                    _fireBed.transform.position = new Vector3(_fbedx, -10, _fbedz);
                }
                else if (Powers.MagnetActivated)
                {
                    //ballMat.bounciness = 0;
                    this.gameObject.GetComponent<CircleCollider2D>().sharedMaterial = ballMat[0];
                }

                else if (Powers.BounceActivated)
                    this.gameObject.GetComponent<CircleCollider2D>().sharedMaterial = ballMat[2];
            




                if (!Powers.MagnetActivated && !Powers.BounceActivated)
                    this.gameObject.GetComponent<CircleCollider2D>().sharedMaterial = ballMat[1];
           

            }


            if (Input.GetKey(KeyCode.P))
            {
                FollowPath.speed = 0f;
                CancelInvoke();
                ObstacleClone._activate = true;
                //ObstacleClone.spawn = false;
                BackgroundScroll.startd = false;
                resume.transform.position = new Vector3(-1.25f, 1.75f, -1.4f);
                this.GetComponent<Rigidbody2D>().isKinematic = true;
                //Time.timeScale = 0;
                resume.transform.position = new Vector3(4f / 61.88889f, 143f / 61.88889f, -1.4f);
                quit.transform.position = new Vector3(4f / 61.88889f, -100f / 61.88889f, -1.4f);
                isPaused = true;



            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                FollowPath.speed = 0f;
                ObstacleClone._activate = true;
                CancelInvoke();
                //ObstacleClone.spawn = false;
                BackgroundScroll.startd = false;
                resume.transform.position = new Vector3(-1.25f, 1.75f, -1.4f);
                this.GetComponent<Rigidbody2D>().isKinematic = true;
                //Time.timeScale = 0;
                resume.transform.position = new Vector3(4f / 61.88889f, 143f / 61.88889f, -1.4f);
                quit.transform.position = new Vector3(4f / 61.88889f, -100f / 61.88889f, -1.4f);
                isPaused = true;

            }
           


          
        }
        /*
        if (Social.localUser.authenticated)
        {
            
            Social.ReportProgress(testAchievement, 30.0, OnUnlockAC);
        }
        */
        if (Social.localUser.authenticated)
        {
            if ((int)t == 30)
            {
               // Debug.Log("Achived");

                Social.ReportProgress(_thirtySeconds,100.0, OnUnlockAC);
            }
            if((int)t == 3600 )
                Social.ReportProgress(_tenSeconds, 100.0, OnUnlockAC);
            if ((int)t == 60)
                Social.ReportProgress(_sixtySeconds, 100.0, OnUnlockAC);
            if((int)t == 120)
                Social.ReportProgress(_survivor, 100.0, OnUnlockAC);
            if ((int)t == 300)
                Social.ReportProgress(_loneWolf, 100.0, OnUnlockAC);
            if ((int)t == 900)
                Social.ReportProgress(_fifteenMin, 100.0, OnUnlockAC);
            if((int)t == 1800)
                Social.ReportProgress(_thirtyMins, 100.0, OnUnlockAC);
            if ((int)t == 1200)
                Social.ReportProgress(_twentyMins, 100.0, OnUnlockAC);
            if (_shieldNo >= 50)
                Social.ReportProgress(_shieldLover, 100.0, OnUnlockAC);
            if (_magNo >= 50)
                Social.ReportProgress(_magLover, 100.0, OnUnlockAC);
            if (_bounceNo >= 50)
                Social.ReportProgress(_bounce, 100.0, OnUnlockAC);
        }
    }

    public void OnUnlockAC(bool result)
    {
        Debug.Log("GPGUI: OnUnlockAC " + result);
    }

    public void SpawnPower()
    {
       
            Debug.Log("Power"+Time.deltaTime);
            GameObject spawn = _powerObj[Random.Range(0, 3)];
            //spawn.GetComponent<Rigidbody2D>().isKinematic = false;
        GameObject ins =  (GameObject)Instantiate(spawn, new Vector3(Random.Range(-2.23f, 1.98f), 8.4f, transform.position.z), Quaternion.identity);
        ins.GetComponent<Rigidbody2D>().isKinematic = false;

        
    }
   
}

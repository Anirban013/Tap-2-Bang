using UnityEngine;
using System.Collections;

public class ObstacleClone : MonoBehaviour {

    // Use this for initialization
    private ObstacleCloneParameters _parameters;
    private GameObject[] platforms;
    private float[] wCords;
    private float[] rCords;
    private float[] gCords;
   
    private float x;
    public static bool _activate;
    public static bool spawn;
    void Start () {

        _activate = false;
        spawn = true;
        _parameters = GetComponent<ObstacleCloneParameters>();
        platforms = _parameters.platforms;
        wCords = _parameters.wCords;
        rCords = _parameters.rCords;
        gCords = _parameters.gCords;

    }
	
	// Update is called once per frame
	void Update () {
        if (BackgroundScroll.startd) { 
      if (spawn)
        {
                Debug.Log("Invokeee");
            InvokeRepeating("Spawn", 1.2f, 1.4f);
               
            spawn = false;
        }
           
        }
        if (_activate)
        {
            Debug.Log("Cancellllllll");
            ActivateSpawning();
        }
    }
    private void Spawn()
    {
        int i = Random.Range(0, 4);
        print("index " + i);
        GameObject clone = platforms[i];
        print("Green " + gCords[0]);
        print(this.gameObject.tag);
        if (clone.gameObject.tag == "RedPlatform")
        {
            x = rCords[Random.Range(0, 4)];
            print("X " + rCords);
           /* CircleCollider2D ccl1 = clone.gameObject.AddComponent<CircleCollider2D>();
            CircleCollider2D ccl2 = clone.gameObject.AddComponent<CircleCollider2D>();
            ccl1.offset = new Vector2(3.36f, 0f);
            ccl1.radius = 1.01f;
            ccl2.offset = new Vector2(-3.36f, 0f);
            ccl2.radius = 1.01f;*/
        }
        else if (clone.gameObject.tag == "LongGreenPlatform")
        {

            x = gCords[Random.Range(0, 2)];

            print("X " + gCords);

           /* CircleCollider2D ccl1 = clone.gameObject.AddComponent<CircleCollider2D>();
            CircleCollider2D ccl2 = clone.gameObject.AddComponent<CircleCollider2D>();
            ccl1.offset = new Vector2(3.59f, 0f);
            ccl1.radius = 0.77f;
            ccl2.offset = new Vector2(-3.59f, 0f);
            ccl2.radius = 0.77f;*/
        }
        else if (clone.gameObject.tag == "Platform")
        {
            x = wCords[Random.Range(0, 3)];
            print("X " + wCords);
        }
        else if (clone.gameObject.tag == "Moving Platform")
            x = 0f;


        Instantiate(clone, new Vector3(x, 6.5f, 0f), Quaternion.identity);

    }

    void ActivateSpawning()
    {
        Debug.Log("Cancellled");
            CancelInvoke();

    }
   
}

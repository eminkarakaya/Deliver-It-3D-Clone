using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
[RequireComponent(typeof(Rigidbody))]
public class Motorcycle : MonoBehaviour
{
    [SerializeField] float maxPitch, minPitch;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip accelerationSound, idleSound, crashSound;
    public bool isFinish;
    private PathPlacer pathPlacer;
    private Rigidbody rb;
    [SerializeField] private Driver driver;
    [SerializeField] private Transform driverSitPlace;
    [SerializeField] private List<Transform> path;
    [SerializeField] private GameObject holder;
    [SerializeField] private Transform currTransform;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Vector3 dir;
    [SerializeField] private int currIndex;
    [SerializeField] private float speed;
    [SerializeField] private float accelerationSpeed, decelerationSpeed;
    [SerializeField] private bool isAccident;
    [SerializeField] private float restartTime;
    [SerializeField] private int restartMinusIndex;
    [SerializeField] private float crushPower;
    private AudioSource _accelClip;

    public static Motorcycle instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = .5f;
        _accelClip = SetUpEngineAudioSource(accelerationSound);
        driver = GetComponentInChildren<Driver>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        path.Clear();
        for (int i = 0; i < holder.transform.childCount; i++)
        {
            path.Add(holder.transform.GetChild(i));
        }
        currTransform = path[0];
        CloseRagdoll(false);
        
        //Physics.IgnoreCollision(GetComponent<Collider>(), driver.collider);

    }
    private void Update()
    {
        Move();
        var pitch = Mathf.Clamp(_accelClip.pitch, 1, 5);
        pitch = speed / 4;
        _accelClip.volume = 1 / (pitch*2);
        if (pitch < 1)
            pitch = 1;
        _accelClip.pitch = pitch;
        if(isAccident)
        {
            _accelClip.volume = 0;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Barrier")
        {
            if (isAccident)
                return;
            Accident();
            StartCoroutine(ResetPos());
        }
    }
    public void CloseRagdoll(bool ac)
    {
        Rigidbody[] rigidbodies = driver.transform.GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = driver.transform.GetComponentsInChildren<Collider>();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !ac;
            colliders[i].enabled = ac;
        }
        if (ac)
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].AddForce(GetCrushDir());
            }
        }
    }
    private void Accident()
    {
        audioSource.PlayOneShot(crashSound);
        CloseRagdoll(true);
        Collect.instance.FallGifts(0);
        //driver.rb.isKinematic = false;
        driver.animator.enabled = false;
        driver.transform.parent.SetParent(null);
        isAccident = true;
    }
    private IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(restartTime);
        //driver.rb.isKinematic = true;
        //GetComponent<Rigidbody>().isKinematic = true;
        var _currindex = this.currIndex;
        for (int i = 0; i < restartMinusIndex; i++)
        {
            if(_currindex > 0)
            {
                _currindex--;
            }
        }
        this.currIndex = _currindex;
        driver.transform.parent.SetParent(this.transform);
        transform.position = path[currIndex].position;
        transform.LookAt(path[currIndex + 1].position);
        driver.transform.position = driverSitPlace.position;
        isAccident = false;
        speed = 0;
        driver.animator.enabled = true;
    }
   
    public void Move()
    {
        if (!isAccident && !isFinish)
        {
            //if(Input.GetKeyDown(KeyCode.Space))
            //{
            //    audioSource.PlayOneShot(accelerationSound);
            //}
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    audioSource.Stop();
            //}
            if (Input.GetKey(KeyCode.Space))
            {
                if (speed < maxSpeed)
                {
                    speed += Time.deltaTime * accelerationSpeed;
                    Collect.instance.collectablePlaceRotation -= Time.deltaTime * decelerationSpeed;
                }
                else
                {
                    //audioSource.PlayOneShot(idleSound);
                    if (Collect.instance.collectablePlaceRotation > 0)
                        Collect.instance.collectablePlaceRotation += Time.deltaTime * decelerationSpeed;
                }
            }
            else
            {
                if (speed > 0)
                {
                    speed -= Time.deltaTime * decelerationSpeed;
                    Collect.instance.collectablePlaceRotation += Time.deltaTime * decelerationSpeed;
                }
                if (speed < 0)
                    speed = 0;
            }
            if (Vector3.Distance(transform.position, path[currIndex + 1].position) < 0.2f)
            {
                if(currIndex >= path.Count -2)
                {
                    SwitchToOfficeRoad();
                    return;
                }
                currIndex++;
            }
            transform.LookAt(path[currIndex + 1]);
            transform.Translate(Dir(currIndex,path) * speed * Time.deltaTime, Space.World);
        }
    }
    Vector3 Dir(int currIndex , List<Transform> path)
    {
        //if (path.Count == currIndex)
        //{
        //    EndOfRoad();
        //}
        var next = path[currIndex + 1].position;
        dir = (path[currIndex + 1].position - transform.position).normalized; 
        return (path[currIndex + 1].position - transform.position).normalized;
    }
    public Vector3 GetCrushDir()
    {
        return Dir(currIndex,path).normalized * crushPower * speed;
    }
    public IEnumerator Drive(int _index, List<Transform> roads)
    {
        Collect.instance.ConvertMoney();
        var _currIndex = 0;
        while (true)
        {
            yield return null;
            if (Vector3.Distance(transform.position, roads[_currIndex + 1].position) < 0.2f)
            {
                if(_currIndex == _index)
                {
                    StartCoroutine(GetComponent<EndRoad>().EarnMoney());
                    break;
                }
                _currIndex++;
            }
            if (speed < maxSpeed)
            {
                speed += Time.deltaTime * accelerationSpeed;
            }
            transform.LookAt(roads[_currIndex+1]);
            transform.Translate(Dir(_currIndex,roads) * speed * Time.deltaTime, Space.World);
        }
    }
    //void EndOfRoad()
    //{
        
    //    isFinish = true;
    //    while(true)
    //    {
    //        if (speed < maxSpeed)
    //        {
    //            speed += Time.deltaTime * accelerationSpeed;
    //        }
    //        transform.LookAt(path[currIndex + 1]);
    //        transform.Translate(Dir(currIndex) * speed * Time.deltaTime, Space.World);
    //    }
    //}
    public void SwitchToOfficeRoad()
    {
        GetComponent<EndRoad>().enabled = true;
        isFinish = true;
    }

    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        //source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = 500;
        source.dopplerLevel = 0;
        return source;
    }
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}

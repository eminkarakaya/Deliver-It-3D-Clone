using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
[RequireComponent(typeof(Rigidbody))]
public class Motorcycle : MonoBehaviour
{
    private PathPlacer pathPlacer;
    private Rigidbody rb;
    [SerializeField] private Driver driver;
    [SerializeField] private Transform driverSitPlace;
    [SerializeField] private List<Transform> path;
    [SerializeField] private GameObject holder;
    [SerializeField] private Transform currTransform;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float power;
    [SerializeField] private float torque;
    [SerializeField] private Vector3 dir;
    [SerializeField] private int currIndex;
    [SerializeField] private float speed;
    [SerializeField] private float accelerationSpeed, decelerationSpeed;
    [SerializeField] private bool isAccident;
    [SerializeField] private float restartTime;
    [SerializeField] private int restartMinusIndex;
    [SerializeField] private float crushPower;

    public static Motorcycle instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Barrier")
        {
            Debug.Log("enter");
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
        if (!isAccident)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (speed < maxSpeed)
                {
                    speed += Time.deltaTime * accelerationSpeed;
                }
            }
            else
            {
                if (speed > 0)
                {
                    speed -= Time.deltaTime * decelerationSpeed;
                }
                if (speed < 0)
                    speed = 0;
            }
            if (Vector3.Distance(transform.position, path[currIndex + 1].position) < 0.2f)
            {
                currIndex++;
            }
            transform.LookAt(path[currIndex + 1]);
            transform.Translate(Dir(currIndex) * speed * Time.deltaTime, Space.World);
        }
    }
    Vector3 Dir(int currIndex)
    {
        if (path.Count == currIndex)
            return Vector3.zero;
        var next = path[currIndex + 1].position;
        dir = (path[currIndex + 1].position - transform.position).normalized; 
        return (path[currIndex + 1].position - transform.position).normalized;
    }
    public Vector3 GetCrushDir()
    {
        return Dir(currIndex).normalized * crushPower * speed;
    }
}

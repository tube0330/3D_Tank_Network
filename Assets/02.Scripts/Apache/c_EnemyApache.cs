using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class c_EnemyApache : MonoBehaviour
{
    [Header("Patrol")]
    public List<Transform> patrolList = new List<Transform>();
    public GameObject patrolPoint;
    private Transform tr = null;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 15f;
    bool isSearch = true;
    int wayPointCount = 0;
    [SerializeField] Transform firePos1;
    [SerializeField] Transform firePos2;
    [SerializeField] GameObject A_bullet;
    [SerializeField] LeaserBeam[] leaserBeams;
    public GameObject expEffect;
    float curDelay = 0f;
    float maxDelay = 3f;

    void Start()
    {
        tr = transform;
        curDelay = maxDelay;
    }

    // Update is called once per frame
    void Update()
    {
        WayPointMove();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }


    void WayPointMove()
    {
        Vector3 PointDist = Vector3.zero;
        float dist = 0f;

        if (wayPointCount == 0)
        {
            PointDist = patrolList[0].position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(PointDist), Time.deltaTime * rotSpeed);
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(tr.position, patrolList[0].position);

            if (dist <= 5.5f)
                wayPointCount = 1;
        }

        else if (wayPointCount == 1)
        {
            PointDist = patrolList[1].position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(PointDist), Time.deltaTime * rotSpeed);
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(tr.position, patrolList[1].position);

            if (dist <= 5.5f)
                wayPointCount = 2;
        }

        else if (wayPointCount == 2)
        {
            PointDist = patrolList[2].position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(PointDist), Time.deltaTime * rotSpeed);
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(tr.position, patrolList[2].position);

            if (dist <= 5.5f)
                wayPointCount = 3;
        }

        else if (wayPointCount == 3)
        {
            PointDist = patrolList[3].position - tr.position;
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(PointDist), Time.deltaTime * rotSpeed);
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            dist = Vector3.Distance(tr.position, patrolList[3].position);

            if (dist <= 5.5f)
                wayPointCount = 0;
        }
    }

    void Attack()
    {
        Debug.Log("Attack called");
        FireRay();
    }


    void FireRay()
    {
        Debug.Log("FireRay called");
        curDelay -= Time.deltaTime;

        if (curDelay > 0)
            return;

        // Reset delay
        curDelay = maxDelay;

        // Perform raycasting and attacking
        Ray ray = new Ray(firePos1.position, firePos1.forward * 100f);
        Ray ray1 = new Ray(firePos2.position, firePos2.forward * 100f);
        RaycastHit hit;

        bool hitDetected = Physics.Raycast(ray, out hit, 100f) || Physics.Raycast(ray1, out hit,  100f);

        if (hitDetected)
        {
            leaserBeams[0].FireRay();
            leaserBeams[1].FireRay();
            ShowEffect(hit);
        }
        else
        {
            // Optionally show an effect if no hit is detected
            GameObject hiteff1 = Instantiate(expEffect, tr.InverseTransformPoint(ray.GetPoint(200f)), Quaternion.identity);
            Destroy(hiteff1, 2.0f);
        }
    }


    void ShowEffect(RaycastHit hit)
    {
        Vector3 hitPos = hit.point;
        Vector3 _normal = (firePos1.position - hitPos).normalized;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject hitEff = Instantiate(expEffect, hitPos, rot);
        Destroy(hitEff, 1.0f);

    }
}

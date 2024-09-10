using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_c : MonoBehaviour
{
    static GameManager_c instance;
    public static GameManager_c Instance
    {
        get { return instance; }
        set
        {
            if (instance == null)
                instance = value;
            else if (instance != value)
                Destroy(value.gameObject);
        }
    }

    [SerializeField] List<Transform> spawnList;
    [SerializeField] GameObject apachePrefab;

    void Start()
    {
        Instance = this;
        apachePrefab = Resources.Load<GameObject>("Apache");
    }

    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCount : MonoBehaviour
{
    public static CornerCount _instance;

    public static CornerCount Instance { get { return _instance; } }

    public int countHit = 0;
    public Transform[] overlordSpawn = new Transform[4];
    //public GameObject overlordPrefab;
    public FlockAgent overlordPrefab;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}

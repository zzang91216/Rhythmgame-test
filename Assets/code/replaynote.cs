using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replaynote : MonoBehaviour
{
    public int lane;
    public float delay;
    public GameObject noteef;
    Vector3 pos; 
    // Start is called before the first frame update
    void Start()
    {
        pos.x = 0;
        pos.y = 100;
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = lane-2.5f;
        pos.y = -2.85f + global.speed * (delay - global.runtime);
        if((delay - global.runtime) < 0.0f){
            global.beat(lane, noteef, delay, false);
            Destroy(gameObject);
        }
        transform.position = pos;
    }
}

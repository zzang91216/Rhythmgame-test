using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note : MonoBehaviour
{
    private SpriteRenderer rend;
    public int lane;
    public float delay;
    Vector3 pos; 
    // Start is called before the first frame update
    void Start()
    {
        pos.x = 0;
        pos.y = 100;
        rend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = lane-2.5f;
        pos.y = -2.85f + global.speed * (delay - global.runtime);
        if(lane == 1){
            rend.color = new Color(0.8f,0.8f,0.8f,1);
        }
        if(lane == 2){
            rend.color = new Color(0.8f,0.7f,0.5f,1);
        }
        if(lane == 3){
            rend.color = new Color(0.8f,0.7f,0.5f,1);
        }
        if(lane == 4){
            rend.color = new Color(0.8f,0.8f,0.8f,1);
        }

        if((delay - global.runtime) < -0.3f){
            global.Savereplayjudge("C:/Users/YBK/replayjudge/"+global.personname[global.person]+"/replay"+global.song.ToString()+".txt", lane, delay, -0.25f);
            global.Savereplayline_note("Assets/replay/"+global.personname[global.person]+"/replay"+global.pattern.ToString()+"_"+global.repeatnum[global.pattern].ToString()+".txt", 1 << (lane-1), delay);
            Destroy(gameObject);
            global.notetotal += 1;
            global.error += 0.0625f;
            global.notemiss += 1;
        }
        transform.position = pos;
    }
}

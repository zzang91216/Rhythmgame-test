using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longnote : MonoBehaviour
{
    private SpriteRenderer rend;
    public int lane;
    public float delay;
    public float delay2;
    Vector3 pos; 
    public bool click;
    public float alpha;
    public GameObject noteeffect;
    private float maxeffect;
    private float effecttime;
    // Start is called before the first frame update
    void Start()
    {
        alpha = 1f;
        click = false;
        pos.x = 0;
        pos.y = 100;
        rend = GetComponent<SpriteRenderer>();
        maxeffect = 0.1f;
        
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = (float)lane-2.5f;
        if(click == false){
            pos.y = -3f + global.speed * ((delay+delay2)/2f - global.runtime);
            transform.localScale = new Vector3(1f, global.speed * (delay2-delay), 1f);
        }
        else{
            pos.y = -3f + global.speed * (delay2 - global.runtime)/2f;
            transform.localScale = new Vector3(1f, global.speed * (delay2-global.runtime), 1f);
        }

        rend.color = new Color(0.8f,0.65f + (Mathf.Abs(2.5f-(float)lane)*0.1f),0.35f + (Mathf.Abs(2.5f-(float)lane)*0.3f),alpha);

        if(click == false && (delay - global.runtime) < -0.3f && alpha > 0.8f){
            global.Savereplayjudge("C:/Users/YBK/replayjudge/"+global.personname[global.person]+"/replay"+global.song.ToString()+".txt", lane, delay, -0.25f);
            global.Savereplayline_longnote("Assets/replay/"+global.personname[global.person]+"/replay"+global.pattern.ToString()+"_"+global.repeatnum[global.pattern].ToString()+".txt", 1 << (lane-1), delay, delay2);
            
            global.notetotal += 1;
            global.error += 0.0625f;
            global.notemiss += 1;
            alpha = 0.5f;
        }
        if((delay2 - global.runtime) < 0.0f){
            Destroy(gameObject);
        }
        if(click == true && releasenote()){
            if((delay2 - global.runtime) < 0.1f){
                global.notetotal += 1;
                global.error += 0.0f;
                Destroy(gameObject);
            }
            else{
                global.notetotal += 1;
                global.error += 0.0625f;
                global.notemiss += 1;
                alpha = 0.5f;
                click = false;
            }
            
        }
        transform.position = pos;

        if(click == true){
            effecttime += Time.deltaTime;
            if(effecttime > maxeffect){
                effecttime -= maxeffect;
                Vector3 pos = new Vector3(-2.5f+(float)lane,-2.85f,0);
                Instantiate(noteeffect, pos, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    bool releasenote(){
        if((lane == 1 && Input.GetKeyUp(KeyCode.S))||(lane == 2 && Input.GetKeyUp(KeyCode.D))||(lane == 3 && Input.GetKeyUp(KeyCode.L))||(lane == 4 && Input.GetKeyUp(KeyCode.Semicolon)))
        {
            return(true);
        }
        else{
            return(false);
        }
    }
}
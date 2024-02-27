using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneeffect : MonoBehaviour
{
    private SpriteRenderer rend;
    public int lane;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if((lane == 1 && Input.GetKey(KeyCode.S))||(lane == 2 && Input.GetKey(KeyCode.D))||(lane == 3 && Input.GetKey(KeyCode.L))||(lane == 4 && Input.GetKey(KeyCode.Semicolon))){
            rend.color = new Color(1,1,1,0.15f);
        }
        else{
            rend.color = new Color(1,1,1,0.0f);
        }
    }
}

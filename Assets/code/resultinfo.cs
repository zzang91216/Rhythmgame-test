using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultinfo : MonoBehaviour
{
    public Text info;
    float tempfloat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(global.notetotal == 0){tempfloat = 0;}
        else{tempfloat = 1000f*Mathf.Sqrt(global.error/global.notetotal);}
        info.text = "bpm : "+ global.bpm.ToString() + "\nerror : "+ tempfloat.ToString("F2") + " ms\nmiss : " +  global.notemiss.ToString();
    }
}

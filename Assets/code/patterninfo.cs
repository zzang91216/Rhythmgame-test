using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class patterninfo : MonoBehaviour
{
    public Text info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        info.text = global.patternfilename[global.pattern/2] + "\n bpm : "+ global.startbpm[global.pattern].ToString() + "\n repeat number : " + global.repeatnum[global.pattern].ToString();
        
    }
}

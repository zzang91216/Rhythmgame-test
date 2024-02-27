using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class replayparser : MonoBehaviour
{
    public GameObject replaynote;
    private GameObject replaynoteinst;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < global.replaynotenum.Length; i++){
            if(global.replaynotenum[i] != 0 && (global.speed * (global.replaynotetime[i] - global.runtime)) < 10f){
                    replaynoteinst = Instantiate(replaynote, transform.position, transform.rotation);
                    replaynoteinst.GetComponent<replaynote>().lane = global.replaynotenum[i];
                    replaynoteinst.GetComponent<replaynote>().delay = global.replaynotetime[i];
                    global.replaynotenum[i] = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parser : MonoBehaviour
{
    public GameObject note;
    private GameObject noteinst;
    public GameObject longnote;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < global.notenum.Length; i++){
            if(global.notenum[i] != 0 && (global.speed * (global.notetime[i] - global.runtime)) < 10f){
                if(global.notenum[i] > 7){
                    noteinst = Instantiate(note, transform.position, transform.rotation);
                    noteinst.GetComponent<note>().lane = 4;
                    noteinst.GetComponent<note>().delay = global.notetime[i];
                    global.notenum[i] -= 8;
                }
                if(global.notenum[i] > 3){
                    noteinst = Instantiate(note, transform.position, transform.rotation);
                    noteinst.GetComponent<note>().lane = 3;
                    noteinst.GetComponent<note>().delay = global.notetime[i];
                    global.notenum[i] -= 4;
                }
                if(global.notenum[i] > 1){
                    noteinst = Instantiate(note, transform.position, transform.rotation);
                    noteinst.GetComponent<note>().lane = 2;
                    noteinst.GetComponent<note>().delay = global.notetime[i];
                    global.notenum[i] -= 2;
                }
                if(global.notenum[i] > 0){
                    noteinst = Instantiate(note, transform.position, transform.rotation);
                    noteinst.GetComponent<note>().lane = 1;
                    noteinst.GetComponent<note>().delay = global.notetime[i];
                    global.notenum[i] -= 1;
                }
            }
        }
        for(int lanenum = 0; lanenum < 4; lanenum ++){
            for(int i = 0; i < global.longnotetime[lanenum].Length/2; i++){
                if((global.speed * (global.longnotetime[lanenum][i*2] - global.runtime)) < 10f && global.longnotetime[lanenum][i*2] > 0f){
                    noteinst = Instantiate(longnote, transform.position, transform.rotation);
                    noteinst.GetComponent<longnote>().lane = lanenum+1;
                    noteinst.GetComponent<longnote>().delay = global.longnotetime[lanenum][i*2];
                    noteinst.GetComponent<longnote>().delay2 = global.longnotetime[lanenum][i*2+1];
                    global.longnotetime[lanenum][i*2] = -1f;
                }
            }
        }
    }
}

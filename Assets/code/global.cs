using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;




public class global : MonoBehaviour
{
    public static float speed;
    public static int song;
    public static int bpm;
    public static float runtime;
    public static int songnum;
    public static int[] notenum;
    public static float[] notetime;
    public static int[] replaynotenum;
    public static float[] replaynotetime;
    private static bool stop;
    public static string[] patterns;

    public static float[][] longnotetime;

    private int len;

    public bool automode;
    public int bpmchange;
    public int missmax;
    public float errormax;
    public static float missrange;
    public float missrangetemp;

    private int tempint;

    public static int pattern;
    public static int notemiss;
    public static float error;
    public static int notetotal;

    private int personnum;
    public static int person;
    public static string[] personname;

    public static string[] patternfilename;
    public static int[] startbpm;
    public static int[] repeatnum;


    public GameObject noteef;
    public GameObject tempnote;

    private float speedchanger;
    public static StreamWriter sw;
    public static StreamReader sr;
    
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        Application.targetFrameRate = 300;
        runtime = 0f;
        speed = 15.0f;
        song = 0;
        person = 0;
        songnum = 8;
        bpm = 150;
        speedchanger = 15.0f;


        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        missrange = missrangetemp;
        if(SceneManager.GetActiveScene().name == "start"){
            SceneManager.LoadScene("lobby");
            DirectoryInfo di = new DirectoryInfo("Assets/pattern");
            len = di.GetFiles("*.txt").Length;
            patternfilename = new string[len];
            
            for(int i = 0; i < len; i++){
                patternfilename[i] = di.GetFiles("*.txt")[i].Name;
            }
            startbpm = new int[len*2];
            repeatnum = new int[len*2];

            Loadperson();
            Loaddefault();
            Loaduserinfo();
        }
        if(SceneManager.GetActiveScene().name == "lobby"){
            if(Input.GetKeyDown(KeyCode.F1)){
                Savedefault();
                if(!Directory.Exists("Assets/replay/"+personname[person])){
                    Directory.CreateDirectory("Assets/replay/"+personname[person]);
                }
                if(!Directory.Exists("C:/Users/YBK/replayjudge/"+personname[person])){
                    Directory.CreateDirectory("C:/Users/YBK/replayjudge/"+personname[person]);
                }
                
                if (File.Exists("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt") == true){
                    try {
                        File.Delete("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt");
                    }
                    catch (System.Exception) {
                        Debug.Log("failed");
                    }
                }

                if (File.Exists("C:/Users/YBK/replayjudge/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt") == true){
                    try {
                        File.Delete("C:/Users/YBK/replayjudge/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt");
                    }
                    catch (System.Exception) {
                        Debug.Log("failed");
                    }
                }
                SceneManager.LoadScene("pattern");
                runtime = 0f;
                bpm = startbpm[pattern];
                if(pattern %2 == 0){
                    Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],3f, repeatnum[pattern],false);
                }
                else{
                    Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],3f, repeatnum[pattern],true);
                }
                Loaddefault();
                notemiss = 0;
                notetotal = 0;
                error = 0f;
                stop = false;
                

            }

            
            if(Input.GetKeyDown(KeyCode.F2)){
                Savedefault();
                if(!Directory.Exists("Assets/replay/"+personname[person])){
                    Directory.CreateDirectory("Assets/replay/"+personname[person]);
                }
                if(!Directory.Exists("C:/Users/YBK/replayjudge/"+personname[person])){
                    Directory.CreateDirectory("C:/Users/YBK/replayjudge/"+personname[person]);
                }
                SceneManager.LoadScene("replay");
                runtime = 0f;
                Loadreplay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt");
                Loaddefault();
                stop = false;
            }
            

            if(Input.GetKeyDown(KeyCode.F3)){
                Saveuserinfo();
                
                person+=1;
                if(person>=personnum){person -= personnum;}

                Loaduserinfo();
            }
            if(Input.GetKeyDown(KeyCode.F4)){
                Saveuserinfo();

                person-=1;
                if(person<0){person += personnum;}

                Loaduserinfo();
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                Saveuserinfo();
                pattern-=1;
                if(pattern<0){pattern += patternfilename.Length*2;}
                if(pattern%2 == 0){
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], false);
                }
                else{
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], true);
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                Saveuserinfo();
                pattern+=1;
                if(pattern>=patternfilename.Length*2){pattern -= patternfilename.Length*2;}
                if(pattern%2 == 0){
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], false);
                }
                else{
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], true);
                }
            }
            if(GameObject.FindGameObjectsWithTag("Finish").Length == 0){
                if(pattern%2 == 0){
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], false);
                }
                else{
                    makepatternpreview("Assets/pattern/"+patternfilename[(int)(pattern/2)], true);
                }
            }
            if(Input.GetKeyDown(KeyCode.PageUp)){
                startbpm[pattern] += bpmchange;
                Saveuserinfo();
            }
            if(Input.GetKeyDown(KeyCode.PageDown)){
                startbpm[pattern] -= bpmchange;
                Saveuserinfo();
            }
            if(Input.GetKeyDown(KeyCode.Home)){
                repeatnum[pattern] += 1;
                Saveuserinfo();
            }
            if(Input.GetKeyDown(KeyCode.End)){
                repeatnum[pattern] -= 1;
                Saveuserinfo();
            }
        }

        if(SceneManager.GetActiveScene().name == "pattern"){
            tempint = 0;
            foreach (int item in notenum) {
                tempint += item;
            }
            if(GameObject.FindGameObjectsWithTag("Respawn").Length == 0 && tempint <= 0){
                if(automode == true){
                    if(((error/notetotal) > errormax*errormax) || notemiss > missmax){
                        bpm -= bpmchange;
                        if(bpm < 20){bpm = 20;}
                    }
                    else{bpm += bpmchange;}
                    startbpm[pattern] = bpm;
                    if(pattern %2 == 0){
                        Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],runtime + 3f, repeatnum[pattern],false);
                    }
                    else{
                        Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],runtime + 3f, repeatnum[pattern],true);
                    }
                    
                    notemiss = 0;
                    notetotal = 0;
                    error = 0f;
                }
                else{
                    if(Input.GetKeyDown(KeyCode.R)){
                        if(pattern %2 == 0){
                            Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],runtime + 3f, repeatnum[pattern],false);
                        }
                        else{
                            Loadpattern("Assets/pattern/"+patternfilename[(int)(pattern/2)],runtime + 3f, repeatnum[pattern],true);
                        }
                        notemiss = 0;
                        notetotal = 0;
                        error = 0f;
                    }
                    if(Input.GetKeyDown(KeyCode.PageUp)){
                        bpm += bpmchange;
                        startbpm[pattern] = bpm;
                    }
                    if(Input.GetKeyDown(KeyCode.PageDown)){
                        bpm -= bpmchange;
                        if(bpm < 20){bpm = 20;}
                        startbpm[pattern] = bpm;
                    }
                }

            }
            
            if(Input.GetKeyDown(KeyCode.Escape)){
                SceneManager.LoadScene("lobby");
                Saveuserinfo();
                Savedefault();
            }
            if(stop == false){
                runtime += Time.deltaTime;
            }
            if(Input.GetKeyDown(KeyCode.S)){
                 Savereplayline_replay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", 1, runtime);
                 beat(1,noteef, runtime, true);
            }
            if(Input.GetKeyDown(KeyCode.D)){
                 Savereplayline_replay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", 2, runtime);
                 beat(2,noteef, runtime, true);
            }
            if(Input.GetKeyDown(KeyCode.L)){
                 Savereplayline_replay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", 3, runtime);
                 beat(3,noteef, runtime, true);
            }
            if(Input.GetKeyDown(KeyCode.Semicolon)){
                 Savereplayline_replay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", 4, runtime);
                 beat(4,noteef, runtime, true);
            }
            
        }
        if(SceneManager.GetActiveScene().name == "replay"){
            if(Input.GetKeyDown(KeyCode.Escape)){
                SceneManager.LoadScene("lobby");
                Savedefault();
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                stop = !stop;
            }
            if(Input.GetKeyDown(KeyCode.R)){
                runtime = 0f;
                Loadreplay("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt");
            }
            if(stop == false){
                runtime += Time.deltaTime;
            }
            
        }
        speedchanger += Input.GetAxis("Mouse ScrollWheel")*2.0f;
        speed = speedchanger;
    }

    void Savedefault()
    {
        if (File.Exists("Assets/data/savedefault.txt") == true){
            try {
                File.Delete("Assets/data/savedefault.txt");
            }
            catch (System.Exception) {
                Debug.Log("save failed");
            }
        }
        sw = new StreamWriter("Assets/data/savedefault.txt");
        sw.WriteLine(pattern.ToString());
        sw.WriteLine(speed.ToString());
        sw.WriteLine(person.ToString());
        sw.WriteLine(missmax.ToString());
        sw.WriteLine(errormax.ToString());
        sw.WriteLine(missrange.ToString());
        sw.Flush();
        sw.Close();
    }

    void Loaddefault()
    {
        if (File.Exists("Assets/data/savedefault.txt") == true){
            sr = new StreamReader("Assets/data/savedefault.txt");
            pattern = int.Parse(sr.ReadLine());
            speedchanger = float.Parse(sr.ReadLine());
            person = int.Parse(sr.ReadLine());
            missmax = int.Parse(sr.ReadLine());
            errormax = float.Parse(sr.ReadLine());
            missrange = float.Parse(sr.ReadLine());
            sr.Close();
            Debug.Log("dat loaded");
        }
        else{
            Debug.Log("fail to load dat");
        }
    }

    void Loadsongname()
    {
        if (File.Exists("Assets/data/songname.txt") == true){
            songnum = File.ReadAllLines("Assets/data/songname.txt").Length;
            sr = new StreamReader("Assets/data/songname.txt");
            patterns = new string[songnum];
            for(int i = 0; i < songnum; i++){
                patterns[i] = sr.ReadLine();
            }
            sr.Close();
            Debug.Log("songs loaded");
        }
        else{
            Debug.Log("fail to load songs");
        }
    }

    void Loadperson()
    {
        if (File.Exists("Assets/data/personname.txt") == true){
            personnum = File.ReadAllLines("Assets/data/personname.txt").Length;
            sr = new StreamReader("Assets/data/personname.txt");
            personname = new string[personnum];
            for(int i = 0; i < personnum; i++){
                personname[i] = sr.ReadLine();
            }
            sr.Close();
            Debug.Log("persons loaded");
        }
        else{
            Debug.Log("fail to load person");
        }
    }

    void Loaduserinfo()
    {
        string[] tempstring;
        for(int i = 0; i < startbpm.Length; i++){
            startbpm[i] = 40;
            repeatnum[i] = 4;
        }
        if (File.Exists("Assets/data/userinfo/"+personname[person]+".txt") == true){
            len = File.ReadAllLines("Assets/data/userinfo/"+personname[person]+".txt").Length;
            sr = new StreamReader("Assets/data/userinfo/"+personname[person]+".txt");
            
            for(int i = 0; i < len; i++){
                tempstring = sr.ReadLine().Split(' ');
                startbpm[i] = int.Parse(tempstring[0]);
                repeatnum[i] = int.Parse(tempstring[1]);
            }
            sr.Close();
            Debug.Log("userinfo loaded");
        }
        else{
            Debug.Log("fail to load user info");
        }
    }

    void Saveuserinfo()
    {
        if (File.Exists("Assets/data/userinfo/"+personname[person]+".txt") == true){
            try {
                File.Delete("Assets/data/userinfo/"+personname[person]+".txt");
            }
            catch (System.Exception) {
                Debug.Log("delete failed");
            }
        }
        sw = new StreamWriter("Assets/data/userinfo/"+personname[person]+".txt");
        for(int i = 0; i < startbpm.Length; i++){
            sw.WriteLine(startbpm[i].ToString() + " " + repeatnum[i].ToString());
        }
        sw.Flush();
        sw.Close();
        Debug.Log("save info");

    }

    void Loadpattern(string filedir, float offset, int repeatnumber, bool rev){
        string[] tempstring;
        float curtime = offset;
        if (File.Exists(filedir) == true){
            
            sr = new StreamReader(filedir);
            tempstring = sr.ReadLine().Split(' ');
            len = tempstring.Length;
            notenum = new int[len*repeatnumber];
            notetime = new float[len*repeatnumber];
            for(int i = 0; i < len; i++){
                if(rev){notenum[i] = reverse(int.Parse(tempstring[i]), 4);}
                else{notenum[i] = int.Parse(tempstring[i]);}
                notetime[i] =  curtime;
                curtime += 120f/((float)bpm*(float)len);
            }
            for(int i = 1; i < repeatnumber; i++){
                for(int j = 0; j < len; j++){
                    notenum[j+i*len] = notenum[j];
                    notetime[j+i*len] =  curtime;
                    curtime += 120f/((float)bpm*(float)len);
                }
            }
            longnotetime = new float[4][];

            for(int lanenum = 0; lanenum < 4; lanenum++){
                int inlane;
                if(rev == false){inlane = lanenum;}
                else{inlane = 3-lanenum;}
                curtime = offset;
                tempstring = sr.ReadLine().Split(' ');
                len = tempstring.Length;
                
                longnotetime[inlane] = new float[len*repeatnumber*2];
                for(int i = 0; i < len; i++){
                    if(int.Parse(tempstring[i]) > 0){
                        Debug.Log(int.Parse(tempstring[i]));
                        longnotetime[inlane][i*2] = curtime;
                        longnotetime[inlane][i*2+1] = curtime + float.Parse(tempstring[i])*120f/((float)bpm*(float)tempstring.Length);
                    }
                    else{
                        longnotetime[inlane][i*2] = -1f;
                    }
                    curtime += 120f/((float)bpm*(float)len);
                }
                for(int i = 1; i < repeatnumber; i++){
                    for(int j = 0; j < len; j++){
                        if(longnotetime[inlane][j*2] > 0){
                            longnotetime[inlane][j*2+i*2*len] = curtime;
                            longnotetime[inlane][j*2+1+i*2*len] =  curtime + longnotetime[inlane][j*2+1] - longnotetime[inlane][j*2];
                        }
                        else{
                            longnotetime[inlane][j*2+i*2*len] = -1f;
                        }
                        curtime += 120f/((float)bpm*(float)len);
                    }
                }
            }
            sr.Close();
        }
        else{
            Debug.Log("fail to load pattern");
        }
    }

    void Loadsong(string filedir)
    {
        int linenum = 0;
        int line = 0;
        float curtime = 2f;
        float tempbpm = 150f;
        string[] tempstring;
        if (File.Exists(filedir) == true){
            sr = new StreamReader(filedir);
            linenum = int.Parse(sr.ReadLine());
            notenum = new int[linenum*8];
            notetime = new float[linenum*8];
            while(true){
                tempstring = sr.ReadLine().Split(' ');
                if(tempstring.Length < 2){
                    tempbpm = float.Parse(tempstring[0]);
                }
                else{
                    for(int i = 0; i < 8; i++){
                        notenum[line*8 + i] = int.Parse(tempstring[i]);
                        notetime[line*8 + i] =  curtime;
                        curtime += 15f/tempbpm;
                    }
                    line += 1;
                    if(line >= linenum){
                        break;
                    }
                }
            }
            sr.Close();
            Debug.Log("song loaded");
        }
        else{
            Debug.Log("fail to load song");
        }
    }

    void Savedata(int filenum)
    {
        
    }
    void Loaddata(int filenum)
    {
        
    }

    void Loadreplay(string filedir){
        int linenum;
        int tempnum;
        string[] tempstrings;
        if (File.Exists(filedir) == true){
            linenum = File.ReadAllLines(filedir).Length;
            sr = new StreamReader(filedir);
            notenum = new int[linenum];
            notetime = new float[linenum];
            replaynotenum = new int[linenum];
            replaynotetime = new float[linenum];
            longnotetime = new float[4][];
            for(int lanenum = 0; lanenum < 4; lanenum++){
                longnotetime[lanenum] = new float[linenum*2];
            }
            for(int i = 0; i < linenum; i++){
                tempstrings = sr.ReadLine().Split(' ');
                if(tempstrings.Length == 2){
                    replaynotenum[i] = int.Parse(tempstrings[0]);
                    replaynotetime[i] = float.Parse(tempstrings[1]);
                    notenum[i] = 0;
                    notetime[i] = replaynotetime[i];
                    for(int lanenum = 0; lanenum < 4; lanenum++){
                        longnotetime[lanenum][i*2] = -1f;
                    }
                }
                else{
                    if(tempstrings.Length == 3){
                        replaynotenum[i] = 0;
                        replaynotetime[i] = float.Parse(tempstrings[1]);
                        notenum[i] = int.Parse(tempstrings[0]);;
                        notetime[i] = replaynotetime[i];
                        for(int lanenum = 0; lanenum < 4; lanenum++){
                            longnotetime[lanenum][i*2] = -1f;
                        }
                    }
                    else{
                        replaynotenum[i] = 0;
                        replaynotetime[i] = float.Parse(tempstrings[1]);
                        notenum[i] = 0;
                        notetime[i] = replaynotetime[i];
                        for(int lanenum = 0; lanenum < 4; lanenum++){
                            longnotetime[lanenum][i*2] = -1f;
                        }
                        tempnum = int.Parse(tempstrings[0]);
                        longnotetime[tempnum][i*2] = float.Parse(tempstrings[1]);
                        longnotetime[tempnum][i*2+1] = float.Parse(tempstrings[2]);
                    }
                }

            }
            sr.Close();
            Debug.Log("replay loaded");
        }
        else{
            Debug.Log("replay load failed");
        }
        
    }

    void Savereplayline_replay(string filedir, int lane, float beattime){
        if (File.Exists(filedir) == true){
            sw = File.AppendText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString());
            sw.Flush();
            sw.Close();
        }
        else{
            sw = File.CreateText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString());
            sw.Flush();
            sw.Close();
        }
        
    }
    public static void Savereplayline_note(string filedir, int lane, float beattime){
        if (File.Exists(filedir) == true){
            sw = File.AppendText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString() + " N");
            sw.Flush();
            sw.Close();
        }
        else{
            sw = File.CreateText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString() + " N");
            sw.Flush();
            sw.Close();
        }
        
    }

    public static void Savereplayline_longnote(string filedir, int lane, float beattime, float endtime){
        if (File.Exists(filedir) == true){
            sw = File.AppendText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString()+ " " + endtime.ToString() + " L");
            sw.Flush();
            sw.Close();
        }
        else{
            sw = File.CreateText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString()+ " " + endtime.ToString() + " L");
            sw.Flush();
            sw.Close();
        }
        
    }

    public static void Savereplayjudge(string filedir, int lane, float beattime, float diff){
        if (File.Exists(filedir) == true){
            sw = File.AppendText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString() + " " + diff.ToString());
            sw.Flush();
            sw.Close();
        }
        else{
            sw = File.CreateText(filedir);
            sw.WriteLine(lane.ToString() + " " + beattime.ToString() + " " + diff.ToString());
            sw.Flush();
            sw.Close();
        }
        
    }
    
    public static void beat(int lane, GameObject noteeffect, float beattime, bool tosave){
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Respawn");
        Vector3 pos = new Vector3(-2.5f+(float)lane,-2.85f,0);
        int notemin = -1;
        float notemindelay = 1000f;
        int longnotemin = -1;
        float longnotemindelay = 1000f;
        float longnotemindelay2 = 1000f;
        for(int i = 0; i < notes.Length; i++){
            if(notes[i].name.Equals("note(Clone)")){
                if(notes[i].GetComponent<note>().lane == lane){
                    if(notes[i].GetComponent<note>().delay < notemindelay){
                        notemin = i;
                        notemindelay = notes[i].GetComponent<note>().delay;
                    }
                }
            }
            else{
                if(notes[i].GetComponent<longnote>().lane == lane){
                    if(notes[i].GetComponent<longnote>().delay < longnotemindelay && notes[i].GetComponent<longnote>().alpha > 0.8f && notes[i].GetComponent<longnote>().click == false){
                        longnotemin = i;
                        longnotemindelay = notes[i].GetComponent<longnote>().delay;
                        longnotemindelay2 = notes[i].GetComponent<longnote>().delay2;
                    }
                }
            }
        }
        if((notemindelay -  beattime) < 0.3f || (longnotemindelay -  beattime) < 0.3f){

            if(notemindelay < longnotemindelay){
                if((notemindelay -  beattime) < 0.3f){
                    Destroy(notes[notemin]);
                    if(tosave == true){
                        Savereplayjudge("C:/Users/YBK/replayjudge/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", lane, notemindelay, notemindelay - beattime);
                        Savereplayline_note("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", 1 << (lane-1), notemindelay);
                    }
                    notetotal += 1;
                    error += (notemindelay - beattime)*(notemindelay - beattime);
                    if(((notemindelay - beattime)<-missrange) || ((notemindelay - beattime)>missrange)){
                        notemiss += 1;
                    }
                    Instantiate(noteeffect, pos, Quaternion.Euler(0, 0, 0));
                }
            }
            else{
                if(tosave == true){
                    Savereplayjudge("C:/Users/YBK/replayjudge/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", lane, longnotemindelay, longnotemindelay - beattime);
                    Savereplayline_longnote("Assets/replay/"+personname[person]+"/replay"+pattern.ToString()+"_"+repeatnum[pattern].ToString()+".txt", lane, longnotemindelay, longnotemindelay2);
                }
                notetotal += 1;
                error += (longnotemindelay - beattime)*(longnotemindelay - beattime);
                if(((longnotemindelay - beattime)<-missrange) || ((longnotemindelay - beattime)>missrange)){
                    notemiss += 1;
                    notes[longnotemin].GetComponent<longnote>().alpha = 0.5f;
                }
                else{
                    notes[longnotemin].GetComponent<longnote>().click = true;
                }
            }
        }
    }

    void makepatternpreview(string filedir, bool rev){
        foreach(GameObject note in GameObject.FindGameObjectsWithTag("Finish")){
            Destroy(note);
        }
        string[] tempstring;
        Vector3 pos = new Vector3(0,0,0);
        sr = new StreamReader(filedir);
        tempstring = sr.ReadLine().Split(' ');
        len = tempstring.Length;
        for(int i = 0; i < len; i++){
            tempint = int.Parse(tempstring[i]);
            pos.y = (8.0f*(float)i/(float)len) - 3.85f;
            if(tempint > 7){
                pos.x = 1.5f;
                if(rev == true){pos.x = -pos.x;}
                Instantiate(tempnote, pos, Quaternion.Euler(0, 0, 0));
                tempint -= 8;
            }
            if(tempint > 3){
                pos.x = 0.5f;
                if(rev == true){pos.x = -pos.x;}
                Instantiate(tempnote, pos, Quaternion.Euler(0, 0, 0));
                tempint -= 4;
            }
            if(tempint > 1){
                pos.x = -0.5f;
                if(rev == true){pos.x = -pos.x;}
                Instantiate(tempnote, pos, Quaternion.Euler(0, 0, 0));
                tempint -= 2;
            }
            if(tempint > 0){
                pos.x = -1.5f;
                if(rev == true){pos.x = -pos.x;}
                Instantiate(tempnote, pos, Quaternion.Euler(0, 0, 0));
                tempint -= 1;
            }
        }
        for(int i = 0; i < 4; i++){
            tempstring = sr.ReadLine().Split(' ');
            len = tempstring.Length;
            for(int j = 0; j < len; j++){
                tempint = int.Parse(tempstring[j]);
                pos.y = (8.0f*((float)j+((float)tempint/2f))/(float)len) - 4.0f;
                pos.x = -1.5f+(float)i;
                if(rev == true){pos.x = -pos.x;}
                GameObject tempinst = Instantiate(tempnote, pos, Quaternion.Euler(0, 0, 0));
                tempinst.transform.localScale = new Vector3(1f, 8f*(float)tempint/(float)len, 1f);
            }
        }
    }

    int reverse(int num, int lanemax){
        int res = 0;
        int templane = num;
        for(int i = 0; i < lanemax; i++){
            res *= 2;
            if(templane%2 == 1){
                res += 1;
            }
            templane>>= 1;
        }
        return(res);
    }
}

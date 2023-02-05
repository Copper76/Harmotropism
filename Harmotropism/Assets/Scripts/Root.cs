using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public GameObject downtip;
    public GameObject lefttip;
    public GameObject uptip;
    public GameObject righttip;
    public GameObject downstraight;
    public GameObject leftstraight;
    public GameObject upstraight;
    public GameObject rightstraight;
    public GameObject downleftcurve;
    public GameObject downrightcurve;
    public GameObject upleftcurve;
    public GameObject uprightcurve;
    public int dir = 0; //0 = down, 1 = left, 2 = up, 3 = right
    public List<Vector3> path = new List<Vector3>();
    public List<GameObject> roots = new List<GameObject>();
    public GameObject parentRoot; 
    public float nextGrow;
    public float nextRetract;
    public int prev_dir = 0;
    public Vector3 tip_pos;
    public float interval;
    public float retractInterval;
    public GameObject root;
    public GameObject cameraObj;
    public bool end;
    public bool win;
    public GameObject rocks;
    public GameObject waters;
    public Vector3 check_point;
    public AudioSource moveSound;
    public AudioSource backgroundMusic;
    public AudioSource retractMusic;

    private List<Vector3> rock_pos = new List<Vector3>();
    private List<Vector3> water_pos = new List<Vector3>(); 
    private float unitLength = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
        interval = 1f;
        retractInterval = 0.5f;
        tip_pos = this.gameObject.transform.position;
        nextGrow = interval;
        GameObject new_root = Instantiate(downtip, tip_pos,downtip.transform.rotation);
        new_root.transform.SetParent(parentRoot.transform);
        roots.Add(new_root);
        path.Add(tip_pos);
        foreach (Transform rock in rocks.transform){
            rock_pos.Add(rock.position);
        }
        foreach (Transform water in waters.transform){
            water_pos.Add(water.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            //Debug.Log(Time.time);
            if (Input.GetKey(KeyCode.Space))
            {
                backgroundMusic.volume = 0.0f;
                retractMusic.volume = 1.0f;
                if (Time.time >= nextRetract && path.Count > 1 && tip_pos != check_point)
                {
                    retract();
                }
            }
            else
            {
                backgroundMusic.volume = 1.0f;
                retractMusic.volume = 0.0f;
                if (Input.GetKeyDown("a") && prev_dir != 3)
                { 
                    dir = 1;
                }
                if (Input.GetKeyDown("d") && prev_dir != 1)
                {
                    dir = 3;
                }
                if (Input.GetKeyDown("s"))
                {
                    dir = 0;
                }
                if (Time.time >= nextGrow)
                {
                    move(dir);
                }
            }
            if (Time.time >= nextRetract)
            {
                nextRetract += retractInterval;
            }
            if (Time.time >= nextGrow)
            {
                nextGrow += interval;
            }
        }
    }

    void move(int dir)
    {
        Destroy(roots[roots.Count-1]);
        moveSound.Play();
        switch(dir)
        {
            case 0:
            //assumes there is always a root
                switch(prev_dir)
                {
                    case 0:
                        roots[roots.Count-1] = Instantiate(downstraight, tip_pos,downstraight.transform.rotation);
                        break;
                    case 1:
                        roots[roots.Count-1] = Instantiate(uprightcurve, tip_pos,uprightcurve.transform.rotation);
                        break;
                    case 3:
                        roots[roots.Count-1] = Instantiate(upleftcurve, tip_pos,upleftcurve.transform.rotation);
                        break;
                }
                tip_pos += new Vector3(0,unitLength*-1,0);
                break;
            case 1:
                switch(prev_dir)
                {
                    case 1:
                        roots[roots.Count-1] = Instantiate(leftstraight, tip_pos,leftstraight.transform.rotation);
                        break;
                    case 0:
                        roots[roots.Count-1] = Instantiate(downleftcurve, tip_pos,downleftcurve.transform.rotation);
                        break;
                }
                tip_pos += new Vector3(unitLength*-1,0,0);
                break;
            case 3:
                switch(prev_dir)
                {
                    case 3:
                        roots[roots.Count-1] = Instantiate(rightstraight, tip_pos,rightstraight.transform.rotation);
                        break;
                    case 0:
                        roots[roots.Count-1] = Instantiate(downrightcurve, tip_pos,downrightcurve.transform.rotation);
                        break;
                }
                tip_pos += new Vector3(unitLength,0,0);
                break;
        }
        roots[roots.Count-1].transform.SetParent(parentRoot.transform);
        GameObject new_root;
        switch(dir)
        {
            case 0:
                new_root = Instantiate(downtip, tip_pos+downtip.transform.localPosition,downtip.transform.rotation);
                new_root.transform.SetParent(parentRoot.transform);
                roots.Add(new_root);
                break;
            case 1:
                new_root = Instantiate(lefttip, tip_pos+lefttip.transform.localPosition,lefttip.transform.rotation);
                new_root.transform.SetParent(parentRoot.transform);
                roots.Add(new_root);
                break;
            case 3:
                new_root = Instantiate(righttip, tip_pos+righttip.transform.localPosition,righttip.transform.rotation);
                new_root.transform.SetParent(parentRoot.transform);
                roots.Add(new_root);
                break;
        }
        if (rock_pos.Contains(tip_pos))
        {
            end = true;
        }
        foreach (Vector3 pos in water_pos){
            if (pos.x == tip_pos.x && pos.y == tip_pos.y)
            {
                check_point = tip_pos;
                break;
            }
        }
        path.Add(tip_pos);
        prev_dir = dir;
        cameraObj.transform.position = tip_pos + new Vector3(0,0,-4);
    }

    void retract()
    {
        Destroy(roots[roots.Count-1]);
        roots.RemoveAt(roots.Count-1);
        Destroy(roots[roots.Count-1]);
        path.RemoveAt(path.Count-1);
        tip_pos = path[path.Count-1];
        if (path.Count>2)
        {
            Debug.Log(path[path.Count-1]-path[path.Count-2]);
            switch(path[path.Count-1]-path[path.Count-2])
            {
                case Vector3 v when v.Equals(Vector3.down*0.5f):
                    roots[roots.Count-1] = Instantiate(downtip, tip_pos+downtip.transform.localPosition,downtip.transform.rotation);
                    prev_dir = 0;
                    dir = 0;
                    break;
                case Vector3 v when v.Equals(Vector3.left*0.5f):
                    roots[roots.Count-1] = Instantiate(lefttip, tip_pos+lefttip.transform.localPosition,lefttip.transform.rotation);
                    prev_dir = 1;
                    dir = 1;
                    break;
                case Vector3 v when v.Equals(Vector3.right*0.5f):
                    roots[roots.Count-1] = Instantiate(righttip, tip_pos+righttip.transform.localPosition,righttip.transform.rotation);
                    prev_dir = 3;
                    dir = 3;
                    break;
            }
        }
        else
        {
            roots[roots.Count-1] = Instantiate(downtip, tip_pos,downtip.transform.rotation);
            prev_dir = 0;
            dir = 0;
        }
        cameraObj.transform.position = tip_pos + new Vector3(0,0,-4);
    }
}

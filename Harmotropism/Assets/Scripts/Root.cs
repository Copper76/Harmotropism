using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public GameObject tip;
    public GameObject straight;
    public GameObject curve;
    public int dir = 0; //0 = down, 1 = left, 2 = up, 3 = right
    public List<Vector3> path = new List<Vector3>();
    public List<GameObject> roots = new List<GameObject>();
    public GameObject parentRoot; 
    public float nextAction;
    public int prev_dir = 0;
    public Vector3 tip_pos;
    public int interval;
    public GameObject root;
    public GameObject cameraObj;
    public bool end;

    private int unitLength = 1;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
        interval = 5;
        tip_pos = this.gameObject.transform.position;
        nextAction = interval;
        GameObject new_root = Instantiate(tip, tip_pos,Quaternion.identity);
        new_root.transform.SetParent(parentRoot.transform);
        roots.Add(new_root);
        path.Add(tip_pos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            if (Input.GetKeyDown("a") && dir != (prev_dir+3) % 4)
        { 
            dir = (dir+3) % 4;
        }
        if (Input.GetKeyDown("d") && dir != (prev_dir+1) % 4)
        {
            dir = (dir+1) % 4;
        }
        Debug.Log(Time.time);
        if (Time.time >= nextAction)
        {
            move(dir);
            nextAction += interval;
        }
        }
    }

    void move(int dir)
    {
        Destroy(roots[roots.Count-1]);
        switch(dir)
        {
            case 0:
            //assumes there is always a root
                switch(prev_dir)
                {
                    case 0:
                        roots[roots.Count-1] = Instantiate(straight, tip_pos,Quaternion.identity);
                        break;
                    case 1:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                    case 3:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                }
                tip_pos += new Vector3(0,unitLength*-1,0);
                break;
            case 1:
                switch(prev_dir)
                {
                    case 1:
                        roots[roots.Count-1] = Instantiate(straight, tip_pos,Quaternion.identity);
                        break;
                    case 2:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                    case 0:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                }
                tip_pos += new Vector3(unitLength*-1,0,0);
                break;
            case 2:
                switch(prev_dir)
                {
                    case 2:
                        roots[roots.Count-1] = Instantiate(straight, tip_pos,Quaternion.identity);
                        break;
                    case 1:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                    case 3:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                }
                tip_pos += new Vector3(0,unitLength,0);
                break;
            case 3:
                switch(prev_dir)
                {
                    case 3:
                        roots[roots.Count-1] = Instantiate(straight, tip_pos,Quaternion.identity);
                        break;
                    case 0:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                    case 2:
                        roots[roots.Count-1] = Instantiate(curve, tip_pos,Quaternion.identity);
                        break;
                }
                tip_pos += new Vector3(unitLength,0,0);
                break;
        }
        roots[roots.Count-1].transform.SetParent(parentRoot.transform);
        if (path.Contains(tip_pos))
        {
            end = true;
        }
        else
        {
            GameObject new_root = Instantiate(tip, tip_pos,Quaternion.identity);
            new_root.transform.SetParent(parentRoot.transform);
            roots.Add(new_root);
            path.Add(tip_pos);
            prev_dir = dir;
            cameraObj.transform.position = tip_pos + new Vector3(0,0,-10);
        }
    }
}

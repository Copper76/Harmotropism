using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public int dir = 1; //1 = down, 2 = left, 3 = up, 4 = right
    public List<Vector3> path = new List<Vector3>();
    public List<GameObject> roots = new List<GameObject>();
    public float timer;
    public GameObject tip;
    public GameObject straight;
    public GameObject curve;

    private int unitLength = 20;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer % 1 == 0)
        {
            move(dir);
        }
    }

    void move(int dir)
    {
        switch(dir)
        {
            case 1:
                this.gameObject.
        }
    }
}

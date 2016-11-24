using UnityEngine;
using System.Collections;

public class PathMover : MonoBehaviour {

    public GameObject arrowPrefab;
    public float speed = 2f;
    public float delay = 1f;

    private GameObject[] arrows;
    private Path path;
    private int loop;
    //private int loopTemp = 0;
    private float timeNextStep;

	// Use this for initialization
	void Start () {
        path = GetComponent<Path>();
        arrows = new GameObject[path.PointsCount - 1];
        for(int i = 0; i < arrows.Length; i++)
        {
            arrows[i] = Instantiate(arrowPrefab);
            arrows[i].transform.parent = transform;
            arrows[i].transform.position = path.GetPoint(path.PointsCount - (i + 1));
        }
        timeNextStep = Time.time + delay;
        loop = arrows.Length - 1;
	}
	
	// Update is called once per frame
	void Update () {


        for (int i = 0; i < arrows.Length; i++)
        {
            if (loop < 0)
            {
                loop = arrows.Length + loop;
            }
            arrows[i].transform.position = Vector3.Lerp(arrows[i].transform.position, path.GetPoint(loop), speed * Time.deltaTime);
            loop--;
        }
        if (Time.time >= timeNextStep)
        {
            timeNextStep = Time.time + delay;
            int ind = loop < 0 ? arrows.Length + loop : loop;
            arrows[ind].transform.position = path.GetPoint(path.PointsCount - 1);
            loop--;
        }


    }

    /*
            for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].transform.position = Vector3.Lerp(arrows[i].transform.position, path.GetPoint(path.PointsCount - (loop)), 5f * Time.deltaTime);
            if (Vector3.Distance(arrows[i].transform.position, path.GetPoint(path.PointsCount - loop)) < 0.01f)
            {
                //Debug.Log(((path.GetPoint(path.PointsCount - loop) - arrows[i].transform.position).sqrMagnitude));
                Debug.Log(i);
                loop++;
            }
            loop++;
            if ((path.PointsCount - loop) < 0)
            {
                  loop = 2;
            }
        }
        if(loop != loopTemp)
        {
            int t = (arrows.Length - loop) + 2;
            t = t == arrows.Length ? 0 : t;
            arrows[t].transform.position = path.GetPoint(path.PointsCount - 1);
            loopTemp = loop;
        }
        */
}

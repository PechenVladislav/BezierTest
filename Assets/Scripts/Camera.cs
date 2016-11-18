using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    private Vector3 posTemp;
    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        posTemp = transform.position;
        posTemp.x = target.position.x;
        transform.position = posTemp;

	}
}

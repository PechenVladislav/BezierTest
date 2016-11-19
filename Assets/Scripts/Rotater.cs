using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {

    public float rotationSpeed = 1f;

    private bool isRotate = false;
    private float t = 0;
    private Quaternion destRot;
    private int direction = 1;

    private void Start()
    {
        destRot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }

	// Update is called once per frame
	void Update () {
        if (Input.touchCount >= 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isRotate = true;
                destRot = Quaternion.FromToRotation(Vector3.up * direction, Vector3.down);
                direction *= -1;
                t = 0;
            }
            if (isRotate)
            {
                t += Time.deltaTime * rotationSpeed;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, destRot, t);
                if (transform.localRotation == destRot || t >= 1f)
                {
                    isRotate = false;
                }
            }
        }
        
    }
}

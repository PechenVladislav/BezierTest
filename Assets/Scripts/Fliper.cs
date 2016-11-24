using UnityEngine;
using System.Collections;

public class Fliper : MonoBehaviour
{

    public float flipSpeed = 1f;

    private bool isRotate = false;
    private Quaternion destRot;
    private int direction = 1;

    private void Start()
    {
        destRot = Quaternion.FromToRotation(Vector3.up, Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRotate = true;
            destRot = Quaternion.FromToRotation(Vector3.up * direction, Vector3.down);
            direction *= -1;
        }
        if (isRotate)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, destRot, Time.deltaTime * flipSpeed);
            if (transform.localRotation == destRot)
            {
                isRotate = false;
            }
        }


    }
}

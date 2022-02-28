using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float swInput = Input.GetAxis("Mouse ScrollWheel");
        if(swInput > 0 && transform.position.z < -8.5)
        {
            swInput = 0;
        }
        else if (swInput < 0 && transform.position.z > -5)
        {
            swInput = 0;
        }
        transform.position += new Vector3(0, 0, -1*swInput * Time.deltaTime * 1000);
    }
}

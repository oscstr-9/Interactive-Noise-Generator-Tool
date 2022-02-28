using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCube : MonoBehaviour
{
  public void ResetCubeRotation()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(0, 1, -6);

    }
}

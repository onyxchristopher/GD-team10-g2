using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTp : MonoBehaviour
{
    public void MovePlayer(Transform location)
    {
        transform.position = location.position;
    }
}

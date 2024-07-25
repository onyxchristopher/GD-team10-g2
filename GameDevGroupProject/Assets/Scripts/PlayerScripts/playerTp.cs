using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTp : MonoBehaviour
{
    // Start is called before the first frame update
    public void MovePlayer(Transform location)
    {
        transform.position = location.position;
    }
}

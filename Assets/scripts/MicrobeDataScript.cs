using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrobeDataScript : MonoBehaviour
{
    private Vector3 startingPosition;

    public void SetStartingPosition(Vector3 pos)
    {
        startingPosition = pos;
    }

    public Vector3 GetStartingPosition()
    {
        return startingPosition;
    }
}

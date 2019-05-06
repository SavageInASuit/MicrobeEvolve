using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolResizerScript : MonoBehaviour
{
    [SerializeField]
    private Transform[] walls;
    [SerializeField]
    private Transform floor;
    [SerializeField]
    private Transform[] waterSurfaces;
    [SerializeField]
    private float poolHeight;

    // Start is called before the first frame update
    void Start()
    {
        float scale = InstanceData.PoolScale;
        float width = scale * 10f;

        walls[0].localScale = new Vector3(width, poolHeight, 1f);
        walls[0].localPosition = new Vector3(0, -(poolHeight / 2f), -(width / 2f));

        walls[1].localScale = new Vector3(width, poolHeight, 1f);
        walls[1].localPosition = new Vector3(0, -(poolHeight / 2f), width / 2f);

        walls[2].localScale = new Vector3(1f, poolHeight, width);
        walls[2].localPosition = new Vector3(width / 2f, -(poolHeight / 2f), 0);

        walls[3].localScale = new Vector3(1f, poolHeight, width);
        walls[3].localPosition = new Vector3(-(width / 2f), -(poolHeight / 2f), 0);

        floor.localScale = new Vector3(width, 1f, width);
        floor.localPosition = new Vector3(0f, -poolHeight, 0f);

        waterSurfaces[0].localScale = new Vector3(scale, 1f, scale);
        waterSurfaces[1].localScale = new Vector3(scale, 1f, scale);
    }
}

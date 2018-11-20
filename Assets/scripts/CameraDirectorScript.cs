using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;
using UnityEngine;

public class CameraDirectorScript : MonoBehaviour {
    [SerializeField] FreeLookCam cam;

    public void SetCameraTarget(GameObject target){
        cam.SetTarget(target.transform);
    }
}

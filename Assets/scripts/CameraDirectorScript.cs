using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraDirectorScript : MonoBehaviour {
    [SerializeField] FreeLookCam cam;

    public GameObject[] objects;
    public TextMeshProUGUI text;
    public Button switchButton;

    private int m_CurrentActiveObject;
    private Text buttonText;

    private float mouseDownTime;
    private readonly float mouseClickDelay = 0.3f;
    private bool mouseDown = false;

    private int activeCam;

    public void Start()
    {
        switchButton.onClick.AddListener(NextCamera);
        buttonText = switchButton.GetComponentInChildren<Text>();
         
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == m_CurrentActiveObject);
            if (i == m_CurrentActiveObject)
                buttonText.text = objects[(i + 1) % objects.Length].name;
        }

        // Set overview camera height relative to pool size
        Transform ot = objects[1].transform;
        ot.localPosition = ot.localPosition + Vector3.up * 300f;
    }

    private void OnEnable()
    {
        text.text = objects[m_CurrentActiveObject].name;
    }

    private void Update()
    {
        bool casting = false;
        // Need to do this only when clicking and not holding mouse down/dragging...
        if(!Input.GetAxis("Fire1").Equals(0) && !mouseDown){
            mouseDownTime = Time.time;
            mouseDown = true;
        }else if(mouseDown && Input.GetAxis("Fire1").Equals(0)){ // '> epsilon' for float comparison...
            // Only cast a ray if the mouse has been clicked, rather than dragged
            if(Time.time - mouseDownTime < mouseClickDelay){
                mouseDownTime = 0;
                casting = true;
            }
            mouseDown = false;
        }

        if (casting){
            Camera camera = objects[m_CurrentActiveObject].GetComponent<Camera>();
            Ray r = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, 1000) && hit.transform.tag == "MicrobePart")
            {
                Vector3 dir = camera.ScreenToWorldPoint(Input.mousePosition) - camera.transform.position;
                Debug.DrawRay(objects[m_CurrentActiveObject].transform.position, dir, Color.red, 4);
                SetCameraTarget(hit.transform.gameObject);
            }

            casting = false;
        }

        if (activeCam == 1){
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 10;
            Vector3 pos = objects[activeCam].transform.localPosition;
            if ((scroll > float.Epsilon && pos.y < 600) || (scroll < -float.Epsilon && pos.y > 0))
                pos += Vector3.up * scroll;
            objects[activeCam].transform.localPosition = pos;
        }
    }


    public void NextCamera()
    {
        int nextactiveobject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == nextactiveobject);
            if (i == nextactiveobject)
                buttonText.text = objects[(i + 1) % objects.Length].name;
        }

        activeCam = nextactiveobject;

        m_CurrentActiveObject = nextactiveobject;
        text.text = objects[m_CurrentActiveObject].name;
    }

    public void SetCameraTarget(GameObject target){
        cam.SetTarget(target.transform);
    }
}

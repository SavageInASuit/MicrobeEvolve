using System.Collections;
using System.Collections.Generic;
using MicrobeApplication;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MicrobeBuilderScript))]
public class LoadedPopulationManagerScript : MonoBehaviour {
    //[SerializeField]
    //private TextMeshProUGUI microbeText;
    [SerializeField]
    private TextMeshProUGUI distanceText;
    [SerializeField]
    private TextMeshProUGUI simSpeedText;

    private GameObject loadedMicrobe;

    MicrobeBuilderScript microbeBuilder;

    // public ScoreListScript listScript;

    // Use this for initialization
    void Start () {
        microbeBuilder = GetComponent<MicrobeBuilderScript>();

        Chromosome c = new Chromosome(InstanceData.ChromosomeString);

        loadedMicrobe = microbeBuilder.CreateInitialMicrobe(c);
    }

    // Update is called once per frame
    void Update() {
        if (loadedMicrobe != null)
        {
            distanceText.text = "Distance: " + GetFitness(loadedMicrobe).ToString("n2") + "m";
        }

        CheckSpeed();
    }

    private float GetFitness(GameObject microbe)
    {
        Vector2 pos = new Vector2(microbe.transform.position.x, microbe.transform.position.z);

        if(pos.magnitude > 115.0f)
        {
            return 0.1f;
        }

        return pos.magnitude;
    }

    private void CheckSpeed()
    {
        if (Input.GetKeyDown("space"))
        {
            Time.timeScale = 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }else if (Input.GetKeyDown("up"))
        {
            Time.timeScale += 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
        else if (Input.GetKeyDown("down") && Time.timeScale > 0)
        {
            Time.timeScale -= 1;
            simSpeedText.text = "Sim Speed: " + Time.timeScale + "x";
        }
    }
}

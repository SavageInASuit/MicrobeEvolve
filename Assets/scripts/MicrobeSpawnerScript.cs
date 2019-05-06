using UnityEngine;
using MicrobeApplication;
using UnityStandardAssets.Cameras;
using UnityEngine.SceneManagement;

public class MicrobeSpawnerScript : MonoBehaviour
{ 
    [SerializeField]
    private FreeLookCam freeLookCam;

    private GameObject microbe;
    private Transform microbeTransform;
    // Start is called before the first frame update
    void Start()
    {
        MicrobeBuilderScript builder = GetComponent<MicrobeBuilderScript>();
        Chromosome chromosome = new Chromosome(InstanceData.ChromosomeString);
        microbe = builder.CreateInitialMicrobe(chromosome);

        // Stop Microbe from sinking by turning off gravity
        Rigidbody[] rbs = microbe.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.useGravity = false;
        }

        // Turn booster force to 0 to stop movement
        BoosterScript[] boosters = microbe.GetComponentsInChildren<BoosterScript>();
        foreach (BoosterScript bs in boosters)
        {
            bs.SetBoostForce(0f);
        }

        InstanceData.SimSpeed = 1f;
        Time.timeScale = 1f;

        microbeTransform = microbe.transform;
        microbeTransform.position = Vector3.zero;

        freeLookCam.SetTarget(microbeTransform);
    }

    public void OnStartInstanceButtonClick()
    {
        SceneManager.LoadScene("LoadedMicrobeScene");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("LoadMicrobeMenuScene");
    }
}

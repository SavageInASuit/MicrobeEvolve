using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrobeBuilderScript : MonoBehaviour {
    [SerializeField] private GameObject[] hulls;
    [SerializeField] private GameObject[] components;
    [SerializeField] private GameObject microbeContainer;

    private void Start()
    {

    }

    // Use this for initialization
    public void CreateInitialMicrobe() {
        GameObject container = Instantiate(microbeContainer);
        // TODO: Change this to be random
        GameObject mainBody = Instantiate(hulls[0]);
        mainBody.transform.SetParent(container.transform);

        Mesh mainBodyMesh = mainBody.GetComponent<MeshFilter>().mesh;
        // Have to create a new instance of a mesh so that the rotation doesn't
        // affect the real mesh...
        // The transform is rotated not the mesh, in reality, but this throws 
        // off mesh vertices and norms for placement purposes
        Mesh newMesh = new Mesh
        {
            vertices = mainBodyMesh.vertices,
            normals = mainBodyMesh.normals,
            triangles = mainBodyMesh.triangles
        };

        List<Vector3> origVerts = new List<Vector3>(newMesh.vertices);

        // As the initial rotation of the body may not be (0, 0, 0) and vertices
        // and normals of the mesh are used to position components, we need to
        // rotate them
        Vector3 rot = mainBody.transform.eulerAngles;
        Quaternion rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        Matrix4x4 m = Matrix4x4.Rotate(rotation);
        int i = 0;
        List<Vector3> meshVertices = new List<Vector3>();
        while (i < origVerts.Count)
        {
            meshVertices.Add(m.MultiplyPoint3x4(origVerts[i]));
            i++;
        }
        newMesh.vertices = meshVertices.ToArray();
        // As the vertices have been altered, we need to recalculate the normals
        newMesh.RecalculateNormals();

        List<Vector3> meshNorms = new List<Vector3>(newMesh.normals);


        //Debug.Log("Vertices: " + meshVertices.Count);
        //Debug.Log("Normals: " + meshNorms.Count);
        //for (i = 0; i < meshVertices.Count; i++){
        //    Debug.Log(i + ": " + meshVertices[i]);
        //}

        // Seed the RNG for testing purposes
        //Random.InitState(123);
        // TESTING PURPOSES: Connect the single component to all ports on the main body
        for (i = 0; i < 5; i++)
        {
            // Try to position to first part at the port of the main body
            GameObject obj = Instantiate(components[0]);
            obj.tag = "Player";
            // Transform target = componentPorts[i];

            // Empty GO to get a transform and create a port at a random vertex
            GameObject dynamicPort = new GameObject();
            Transform target = dynamicPort.transform;
            Destroy(dynamicPort);

            // Get the random index for us to select the vertex
            int ind = Random.Range(0, meshVertices.Count);

            target.position = mainBody.transform.position + meshVertices[ind];
            target.rotation = Quaternion.LookRotation(meshNorms[ind]);

            //obj.transform.rotation = Quaternion.Inverse(Quaternion.LookRotation(-target.forward, target.up));
            //target.rotation *= Quaternion.Euler(target.right * 180);
            obj.transform.rotation = target.rotation;
            // The above rotation places the component the wrong way around, so
            // rotate around local y axis
            obj.transform.Rotate(Vector3.up * 180, Space.Self);

            ComponentPortScript component = obj.GetComponent<ComponentPortScript>();
            if (component != null)
            {
                // Set the position of the component to the port on the main body sub the difference between
                // its transform pos and its port pos
                obj.transform.position = target.position + (obj.transform.position - component.port.transform.position);
                obj.transform.SetParent(mainBody.transform);
                FixedJoint joint = mainBody.AddComponent<FixedJoint>();
                joint.connectedBody = obj.GetComponent<Rigidbody>();
            }
            else
            {
                Debug.Log("No component port script found on component \'" + obj.name + "\'");
            }

            // Remove the vertex and normal that have been used from the list of viable locations
            meshVertices.RemoveAt(ind);
            meshNorms.RemoveAt(ind);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MicrobeApplication;

public class MicrobeBuilderScript : MonoBehaviour
{
    [SerializeField] private GameObject[] hulls;
    [SerializeField] private GameObject[] components;
    [SerializeField] private GameObject microbeContainer;
    [SerializeField] private GameObject water;
    [SerializeField]
    private Transform centerPole;

    CameraDirectorScript director;
    
    private GameObject RandomChoice(GameObject[] objects){
        int choice = Random.Range(0, objects.Length);
        return objects[choice];
    }

    // Use this for initialization
    public void CreateInitialMicrobe() {
        if(director == null){
            director = GetComponent<CameraDirectorScript>();
        }

        GameObject container = Instantiate(microbeContainer);
        container.transform.position = Vector3.up * 5;

        GameObject mainBody = Instantiate(RandomChoice(hulls), container.transform);
        //mainBody.transform.SetParent(container.transform);

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
            //TODO: Get component ID from the chromosome, instead of using hard coded value
            // Try to position to first part at the port of the main body
            GameObject obj = Instantiate(components[1]);
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
                obj.transform.SetParent(container.transform);
                FixedJoint joint = container.AddComponent<FixedJoint>();
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

        director.SetCameraTarget(mainBody);
        BuoyancyControlScript b = container.GetComponent<BuoyancyControlScript>();
        if (b != null)
        {
            b.waterSurface = water.transform;
            b.SetRigidBody(container.GetComponent<Rigidbody>());
        }
        else
        {
            Debug.Log("BuoyancyControlCScript was null........");
        }
    }

    public GameObject CreateInitialMicrobe(Chromosome chromosome)
    {
        if (director == null)
        {
            director = GetComponent<CameraDirectorScript>();
        }

        //Debug.Log("Creating microbe from chromosome:");
        //Debug.Log(chromosome.ChromosomeString);

        GameObject container = Instantiate(microbeContainer);
        container.transform.position = Vector3.up * 5;

        Rigidbody contRb = container.GetComponent<Rigidbody>();
        contRb.mass = chromosome.HullMass;

        int hullId = chromosome.HullId % hulls.Length;
        GameObject mainBody = Instantiate(hulls[hullId], container.transform);
        //mainBody.transform.SetParent(container.transform);

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

        // As the initial scale may not be (1,1,1) and rotation of the body may 
        // not be (0, 0, 0) we need to scale and rotate the vertex vectors

        Vector3 rot = mainBody.transform.eulerAngles;
        Quaternion rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        Matrix4x4 m = Matrix4x4.Rotate(rotation);
        int i = 0;
        List<Vector3> meshVertices = new List<Vector3>();

        Vector3 scale = mainBody.transform.localScale;
        scale.x *= chromosome.HullScale;
        scale.y *= chromosome.HullScale;
        scale.z *= chromosome.HullScale;
        mainBody.transform.localScale = scale;

        while (i < origVerts.Count)
        {
            Vector3 newV = origVerts[i];
            newV.x *= mainBody.transform.localScale.x;
            newV.y *= mainBody.transform.localScale.y;
            newV.z *= mainBody.transform.localScale.z;
            newV = m.MultiplyPoint3x4(newV);
            meshVertices.Add(newV);
            i++;
        }
        newMesh.vertices = meshVertices.ToArray();
        // As the vertices have been altered, we need to recalculate the normals
        newMesh.RecalculateNormals();

        List<Vector3> meshNorms = new List<Vector3>(newMesh.normals);

        for (i = 0; i < chromosome.ComponentCount; i++)
        {
            int componentId = chromosome.ComponentData[i].Id % components.Length;
            GameObject obj = Instantiate(components[componentId]);

            // Empty GO to get a transform and create a port at a random vertex
            GameObject dynamicPort = new GameObject();
            Transform target = dynamicPort.transform;
            Destroy(dynamicPort);

            // Get the index from the chromosome for this component
            int ind = chromosome.ComponentData[i].MeshVertex % meshVertices.Count;

            // TODO: Use the rotation from the chromosome component data to rotate the component
            // TODO: Use mass and buoyancy values from chromosome
            target.position = mainBody.transform.position + meshVertices[ind];
            target.rotation = Quaternion.LookRotation(meshNorms[ind]);

            obj.transform.rotation = target.rotation;
            // The above rotation places the component the wrong way around, so
            // rotate around local y axis
            obj.transform.Rotate(Vector3.up * 180, Space.Self);

            ComponentPortScript component = obj.GetComponent<ComponentPortScript>();
            if (component != null)
            {
                Animator anim = obj.GetComponent<Animator>();

                if(anim != null)
                {
                    // use the scale value for this component to offset the component animation
                    // map the scale factor from its original range to the possible range of animation times
                    float idleStart = Map(chromosome.ComponentData[i].Scale, 
                                          0, 
                                          Chromosome.COMPONENT_SCALE_BITS, 
                                          0, 
                                          anim.GetCurrentAnimatorStateInfo(0).length);
                    anim.Play("LegMovementAnimation", 0, idleStart);
                }

                // Set the position of the component to the port on the main body sub the difference between
                // its transform pos and its port pos
                component.transform.rotation = component.port.transform.rotation;
                obj.transform.position = target.position + (obj.transform.position - component.port.transform.position);
                obj.transform.SetParent(container.transform);
                FixedJoint joint = container.AddComponent<FixedJoint>();
                Rigidbody compRb = obj.GetComponent<Rigidbody>();
                joint.connectedBody = compRb;
            }
            else
            {
                Debug.Log("No component port script found on component \'" + obj.name + "\'");
            }

            // Remove the vertex and normal that have been used from the list of viable locations
            meshVertices.RemoveAt(ind);
            meshNorms.RemoveAt(ind);
        }

        if (director)
            director.SetCameraTarget(mainBody);
        BuoyancyControlScript b = container.GetComponent<BuoyancyControlScript>();
        if (b && water)
        {
            b.waterSurface = water.transform;
            b.SetRigidBody(container.GetComponent<Rigidbody>());
            return container;
        }

        Debug.Log("BuoyancyControlCScript was null........");

        return container;
    }

    public GameObject CreateMicrobeAtPosition(Chromosome chromosome, Vector3 position)
    {
        GameObject microbe = CreateInitialMicrobe(chromosome);
        microbe.transform.position = position;
        return microbe;
    }

    private float Map(float orig, float inFrom, float inTo, float outFrom, float outTo)
    {
        return outFrom + (((inFrom - orig) / (inTo - inFrom)) * (outTo - outFrom));
    }
}

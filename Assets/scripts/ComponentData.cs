using System;
using UnityEngine;
namespace MicrobeApplication
{
    public class ComponentData
    {
        private int id, meshVertex; 
        private float scale;
        private Vector3 rotation;

        public ComponentData(int id, int meshVertex, float scale, Vector3 rotation)
        {
            this.id = id;
            this.meshVertex = meshVertex;
            this.scale = scale;
            this.rotation = rotation;
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public int MeshVertex
        {
            get
            {
                return meshVertex;
            }

            set
            {
                meshVertex = value;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }
    }
}

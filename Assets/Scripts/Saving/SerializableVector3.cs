using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableVector3
    {
        private float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 DeserializeToVector3(SerializableVector3 serializableVector3)
        {
            Vector3 vector = new Vector3();
            vector.x = serializableVector3.x;
            vector.y = serializableVector3.y;
            vector.z = serializableVector3.z;
            return vector;
        }
    }

}
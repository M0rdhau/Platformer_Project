using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableVector
{
    float x, y, z;

    public SerializableVector(Vector3 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
}

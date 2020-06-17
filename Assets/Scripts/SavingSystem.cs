using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class SavingSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        print("Saving to " + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            byte[] VectorArray = SerializeVector(GetPlayerTransform().position);
            stream.Write(VectorArray, 0, VectorArray.Length);
        }
    }



    public void Load(string saveFile)
    {
        var path = GetPathFromSaveFile(saveFile);
        print("Loading from " + path);
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            Vector3 playerPos = DeserializeVector(buffer);
            FindObjectOfType<PlayerController>().transform.position = playerPos;
        }

    }

    private Vector3 DeserializeVector(byte[] buffer)
    {
        Vector3 res = new Vector3();
        res.x = BitConverter.ToSingle(buffer, 0);
        res.y = BitConverter.ToSingle(buffer, 4);
        res.z = BitConverter.ToSingle(buffer, 8);
        return res;
    }

    private byte[] SerializeVector(Vector3 position)
    {
        byte[] vectorBytes = new byte[3*4];
        BitConverter.GetBytes(position.x).CopyTo(vectorBytes, 0);
        BitConverter.GetBytes(position.y).CopyTo(vectorBytes, 4);
        BitConverter.GetBytes(position.z).CopyTo(vectorBytes, 8);
        return vectorBytes;
    }

    private Transform GetPlayerTransform()
    {
        return FindObjectOfType<PlayerController>().transform;
    }

    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}

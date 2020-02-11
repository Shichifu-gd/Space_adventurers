using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System;

public class SceneData : MonoBehaviour
{
    public static void SaveSceneData(PlayerController playerController)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + "/data")) Directory.CreateDirectory(Application.dataPath + "/data");
        FileStream stream = new FileStream(Application.dataPath + $"/data/SceneData{playerController.GetSceneName()}.sav", FileMode.Create);
        DataScene data = new DataScene(playerController);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static int[] LoadSceneData(SceneName sceneName)
    {
        if (File.Exists(Application.dataPath + $"/data/SceneData{sceneName}.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + $"/data/SceneData{sceneName}.sav", FileMode.Open);
            DataScene data = bf.Deserialize(stream) as DataScene;
            stream.Close();
            return data.sceneData;
        }
        else return new int[0];
    }
}

[Serializable]
public class DataScene
{
    public int[] sceneData;

    public DataScene(PlayerController playerController)
    {
        sceneData = new int[1];
        sceneData[0] = playerController.Score;
    }
}
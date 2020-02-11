using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System;

public class PlayerData
{
    public static void SavePlayerData(PlayerController playerController)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.dataPath + "/data")) Directory.CreateDirectory(Application.dataPath + "/data");
        FileStream stream = new FileStream(Application.dataPath + "/data/PlayerData.sav", FileMode.Create);
        DataPlayer data = new DataPlayer(playerController);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static int[] LoadPlayerData()
    {
        if (File.Exists(Application.dataPath + "/data/PlayerData.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/data/PlayerData.sav", FileMode.Open);
            DataPlayer data = bf.Deserialize(stream) as DataPlayer;
            stream.Close();
            return data.playerData;
        }
        else return new int[0];
    }
}

[Serializable]
public class DataPlayer
{
    public int[] playerData;

    public DataPlayer(PlayerController playerController)
    {
        playerData = new int[1];
        playerData[0] = playerController.Money;
    }
}
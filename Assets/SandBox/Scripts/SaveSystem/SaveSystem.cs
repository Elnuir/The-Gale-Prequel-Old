using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
   public static void SavePlayer(ScoreCounter number)
   {
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/player.fun";
      
      PlayerData data = new PlayerData(number);

      using var stream = File.OpenWrite(path);
      formatter.Serialize(stream, data);
   }

   public static PlayerData LoadPlayer()
   {
      string path = Application.persistentDataPath + "/player.fun";
      if (File.Exists(path))
      {
         BinaryFormatter formatter = new BinaryFormatter();

         using var stream = File.OpenRead(path);
         return formatter.Deserialize(stream) as PlayerData;

      }
      else
      {
         Debug.LogError("Save file not found in" + path);
         return null;
      }
   }
}

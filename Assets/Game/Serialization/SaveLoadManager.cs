using System;
using System.IO;
using MessagePack;
using UnityEngine;

namespace Game.Serialization
{
    public static class SaveLoadManager
    {
        public static bool Save<T>(T data, string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(filePath)) return false;
            
            File.WriteAllBytes(filePath, MessagePackSerializer.Serialize(data));
            return true;
        }

        public static T Load<T>(string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + fileName;
            if (!File.Exists(filePath)) throw new NullReferenceException("");
            
            var bytes = File.ReadAllBytes(filePath);
            return MessagePackSerializer.Deserialize<T>(bytes);
        }
    }
}
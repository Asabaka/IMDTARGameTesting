using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using ProtoBuf;
using System.Threading.Tasks;
using LitJson;

public class BinaryTool
{
    public static bool ProtoSerialize(string path, System.Object obj)
    {
        try
        {
            using (Stream file = File.Create(path))
            {
                Serializer.Serialize(file, obj);
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public static T ProtoDeSerialize<T>(string path) where T : class
    {
        try
        {
            using (Stream file = File.OpenRead(path))
            {
                return Serializer.Deserialize<T>(file);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public static byte[] ProtoSerialize(System.Object obj)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                byte[] result = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(result, 0, result.Length);
                return result;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("序列化失败 ： " + e.ToString());
            return null;
        }
    }


    public static T ProtoDeSerialize<T>(byte[] msg) where T : class
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(msg, 0, msg.Length);
                ms.Position = 0;
                return Serializer.Deserialize<T>(ms);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public static byte[] JsonSerialize(System.Object obj)
    {
        var json = JsonMapper.ToJson(obj);
        Debug.Log(json);
        return System.Text.Encoding.UTF8.GetBytes(json);
    }

    public static T JsonDeSerialize<T>(byte[] msg) where T : class
    {
        var json = System.Text.Encoding.UTF8.GetString(msg);
        Debug.Log(json);
        return JsonMapper.ToObject<T>(json);
    }
}

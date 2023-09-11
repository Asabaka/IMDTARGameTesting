using System.Text;
using System.IO;
using UnityEngine;

public static class FileTool
{
    public static void SaveJson(string data,string filePath,string fileName)
	{
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
		string path = Path.Combine(filePath, fileName);
		FileStream fs;
		fs = new FileStream(path, FileMode.Create);
		byte[] bts = Encoding.UTF8.GetBytes(data);
		fs.Write(bts, 0, bts.Length);
		if(fs != null)
		{
			fs.Close();
		}
	}

	public static void DeleteFile(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
			Debug.Log("删除成功");
		}
		else
		{
			Debug.Log("文件不存在："+path);
		}
	}
}

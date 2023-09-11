using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class VXtxtPusher : MonoBehaviour
{
    public Text txt2push;
    public int howlong = 10;
    [HideInInspector]public int sumline;//bie gai(dont modify this) - zym
    public void PushTxt(string pushStr) {
        sumline = 0;
        //pushStr = StringUtility.BreakLongString(pushStr, howlong);
        pushStr = BreakLongString(pushStr, howlong);
        txt2push.text = pushStr;
    }


    public string BreakLongString(string SubjectString, int lineLength)
    {
        StringBuilder sb = new StringBuilder(SubjectString);
        int offset = 0;
        ArrayList indexList = buildInsertIndexList(SubjectString, lineLength);
        for (int i = 0; i < indexList.Count; i++)
        {
            sb.Insert((int)indexList[i] + offset, '\n');
            offset++;
            sumline++;
        }
        return sb.ToString();
    }

    public bool IsChinese(char c)
    {
        return (int)c >= 0x4E00 && (int)c <= 0x9FA5;
    }

    private ArrayList buildInsertIndexList(string str, int maxLen)
    {
        int nowLen = 0;
        ArrayList list = new ArrayList();
        for (int i = 1; i < str.Length; i++)
        {
            if (IsChinese(str[i]))
            {
                nowLen += 2;
            }
            else
            {
                nowLen++;
            }
            if (nowLen > maxLen)
            {
                nowLen = 0;
                list.Add(i);
            }
        }
        return list;
    }
}


//public class StringUtility
//{

//    public static string BreakLongString(string SubjectString, int lineLength)
//    {
//        StringBuilder sb = new StringBuilder(SubjectString);
//        int offset = 0;
//        ArrayList indexList = buildInsertIndexList(SubjectString, lineLength);
//        for (int i = 0; i < indexList.Count; i++)
//        {
//            sb.Insert((int)indexList[i] + offset, '\n');
//            offset++;
//        }
//        return sb.ToString();
//    }

//    public static bool IsChinese(char c)
//    {
//        return (int)c >= 0x4E00 && (int)c <= 0x9FA5;
//    }

//    private static ArrayList buildInsertIndexList(string str, int maxLen)
//    {
//        int nowLen = 0;
//        ArrayList list = new ArrayList();
//        for (int i = 1; i < str.Length; i++)
//        {
//            if (IsChinese(str[i]))
//            {
//                nowLen += 2;
//            }
//            else
//            {
//                nowLen++;
//            }
//            if (nowLen > maxLen)
//            {
//                nowLen = 0;
//                list.Add(i);
//            }
//        }
//        return list;
//    }
//}
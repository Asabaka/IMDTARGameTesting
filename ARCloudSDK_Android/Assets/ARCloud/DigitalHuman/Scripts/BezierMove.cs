using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BezierMove : MonoBehaviour
{
    public Transform startTrans;
    public Transform endTrans;
    public Transform center;
    public float endScale = 0.2f;
    public float destoryTime = 2.2f;
    public int resolution = 10;
    public float t =0.5f;
    public float centerMax;
    public float centerMin;
    public GameObject[] moveObjs;
    public Transform effectCenter;
    // Start is called before the first frame update

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickSayHollow() {
        int index = Random.Range(0, 4);
        GameObject obj = Instantiate(moveObjs[index]);
        obj.SetActive(true);
        obj.AddComponent<AutoDestory>().destoryTime = destoryTime;
        obj.transform.SetParent(this.transform);
        float radius = 0.3f;
        float x = Random.Range(-radius, radius);
        float y = Random.Range(-radius, radius);
        obj.transform.localPosition = effectCenter.localPosition+ new Vector3(x, y,0);
        StartCoroutine(DelayMove(obj));
    }
    IEnumerator DelayMove(GameObject obj) {
        yield return new WaitForSeconds(0.8f);
        obj.transform.DOScale(endScale, 1.5f);
        obj.transform.DOLocalPath(NewPath(), 1.5f).SetEase(Ease.OutQuint);
    }
    public Vector3[] NewPath() { 
        Vector3[] path;
        var startPoint = startTrans.localPosition;
        var endPoint = endTrans.localPosition;
      
        float wight = Random.Range(centerMin, centerMax);
        var bezierControlPoint = center.localPosition + center.up* wight;
        path = new Vector3[resolution];//resolutionΪint���ͣ���ʾҪȡ��·����������ֵԽ��ȡ�õ�·����Խ�࣬�������Խƽ��
        for (int i = 0; i < resolution; i++)
        {
            var time = (i + 1) / (float)resolution;//�黯��0~1��Χ
            path[i] = GetBezierPoint(time, startPoint, bezierControlPoint, endPoint);//ʹ�ñ��������ߵĹ�ʽȡ��tʱ��·����
        }
        return path;
    }
    /// <param name="t">0��1��ֵ��0��ȡ���ߵ���㣬1������ߵ��յ�</param>
    /// <param name="start">���ߵ���ʼλ��</param>
    /// <param name="center">����������״�Ŀ��Ƶ�</param>
    /// <param name="end">���ߵ��յ�</param>
    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }
}

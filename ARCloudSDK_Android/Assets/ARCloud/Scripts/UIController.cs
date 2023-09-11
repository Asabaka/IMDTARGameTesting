using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public Transform ScanObj;
    public Image ScanSuccess;
    public Image loading;
    // Start is called before the first frame update
    void OnEnable()
    {
        ResetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetUI() {
        ScanObj.gameObject.SetActive(false);
        ScanSuccess.gameObject.SetActive(false);
    }
    public void ShowScanObj() {
        ScanObj.gameObject.SetActive(true);
    }
    public void HideScanObj() {
        ScanObj.gameObject.SetActive(false);
    }
    public void ShowScanSuccess() {
        ScanSuccess.gameObject.SetActive(true);
        HideScanObj();
        ScanSuccess.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(
        delegate
        {
            ScanSuccess.DOColor(new Color(255f, 255f, 255f, 0), 1).OnComplete(delegate {
                ScanSuccess.transform.localScale = Vector3.one * 0.3f;
                ScanSuccess.color = new Color(255f, 255f, 255f, 255f);
                ScanSuccess.gameObject.SetActive(false);
            });
        }
        );
  
    }
}

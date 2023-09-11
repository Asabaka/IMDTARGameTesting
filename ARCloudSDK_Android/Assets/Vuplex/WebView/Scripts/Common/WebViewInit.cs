using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;
public class WebViewInit : MonoBehaviour
{
    public Vector2 m_Size;
    public Vector3 m_Pos;
    private WebViewPrefab m_WebPrefab;
    // Start is called before the first frame update
    void Start()
    {
        m_WebPrefab = GetComponentInChildren<WebViewPrefab>();
        if (m_WebPrefab != null)
        {
            m_WebPrefab.transform.localPosition = m_Pos;
            m_WebPrefab._sizeForInitialization = m_Size;
            
            m_WebPrefab._options = new WebViewOptions();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

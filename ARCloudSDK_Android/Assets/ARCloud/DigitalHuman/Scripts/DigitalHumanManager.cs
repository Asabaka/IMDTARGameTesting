using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class DigitalHumanManager : MonoBehaviour
{
    public string m_PhoneNum;
    public string m_UUID;
    public LayerMask LayerMask;
    private BezierMove m_BezierMove;
    private Text m_HotNum;
   // private Text m_Name;
 //   private Image m_HeadPortrait;
    private Transform showInfoTran;
    private Transform selected;
    private GameObject sayHollowBtn;
    private Animator m_Anim;
    private GameObject m_Btns;
    private GameObject m_MessageParent;
    private GameObject m_LeaveMessage;
    private VideoPlayer m_Video;
    public List<string> animTriggers;
    public string jumpUrl;
    private string getHotUrl = "http://14.204.63.176:14000/arCloudUnityPlus/client/api/digitalPerson/";
    private string sendHotUrl = "http://14.204.63.176:14000/arCloudUnityPlus/client/api/like";
    int currenHotNum;
    // Start is called before the first frame update
    void Start()
    {
        showInfoTran = transform.Find("ShowInfo");
        m_HotNum = transform.Find("ShowInfo/info/Canvas/hot").GetComponent<Text>();
        m_BezierMove = transform.Find("ShowInfo/info/BezierMove").GetComponent<BezierMove>();
        selected = transform.Find("描边");
        sayHollowBtn = transform.Find("ShowInfo/info/Canvas/SayHollowBtn").gameObject;
      //  m_Name = transform.Find("ShowInfo/info/Canvas/Name").GetComponent<Text>();
     //   m_HeadPortrait = transform.Find("ShowInfo/info/Canvas/HeadPortrait").GetComponent<Image>();
        m_Btns = transform.Find("ShowInfo/info/Canvas/LeaveMessage/Btns").gameObject;
        m_MessageParent = transform.Find("ShowInfo/info/Canvas/LeaveMessage/MessageParent").gameObject;
        m_LeaveMessage = transform.Find("ShowInfo/info/Canvas/LeaveMessage").gameObject;
        m_Video = transform.parent.Find("MP4").GetComponent<VideoPlayer>();
        sayHollowBtn.SetActive(false);
        m_LeaveMessage.SetActive(false);
        m_MessageParent.SetActive(false);
        selected.gameObject.SetActive(false);
        if (m_Video != null) {
            m_Video.Pause();
        }
        m_Anim = transform.GetComponent<Animator>();
        GetHotNum();
    }
    public void GetHotNum() {
        StartCoroutine(GetDigitalHumanHot());
    }
    IEnumerator GetDigitalHumanHot()
    {
        
        UnityWebRequest request = UnityWebRequest.Get(getHotUrl+ m_UUID);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            JsonData resultData = JsonMapper.ToObject(request.downloadHandler.text);
            if (resultData["code"].ToString() == "200") {
                m_HotNum.text = resultData["result"]["heatNumber"].ToString();
                currenHotNum = int.Parse(m_HotNum.text.ToString());
              //  StartCoroutine(LoadImage(resultData["result"]["avatarUrl"].ToString(), m_HeadPortrait));
            //    m_Name.text = resultData["result"]["nickName"].ToString();
            }
        }
    }
    IEnumerator LoadImage(string path, Image image)
    {
        string newUrl = path;
        WWW www = new WWW(newUrl);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                Texture2D tex = www.texture;
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                image.sprite = sprite;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null) return;
        if (showInfoTran == null) return;
    
        ChargeSelectDigitalHuman();
        LookAtCamera();
    }
    RaycastHit hit;
    bool isSelected = false;
    void ChargeSelectDigitalHuman() {

        if (Input.GetMouseButtonDown(0))
        {
            Ray myray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myray, out hit, 1000f, LayerMask))
            {
                if (hit.collider.gameObject.name == gameObject.name)
                {
                    ShowHidSelected(true);
                    if (!isSelected)
                    {
                        EventCenter.Broadcast(MyEventType.OnClickDigitalHuman, m_UUID);
                        Debug.Log("选中" + m_UUID);
                        isSelected = true;
                    }
                }
                else {
                    Debug.Log("Name" + hit.collider.gameObject.name);
                }
            

            }
            else
            {

                if (isSelected)
                {
                    ShowHidSelected(false);
                    EventCenter.Broadcast(MyEventType.OnCancleSelectDigitalHuman);
                    isSelected = false;
                    Debug.Log("取消选中" + m_UUID);
                }

            }
            //if (Physics.Raycast(myray, out hit, 1000f, LayerMask)&&hit.collider.gameObject.name == gameObject.name) {
            //    ShowHidSelected(true);
            //    if (!isSelected) {
            //        EventCenter.Broadcast(MyEventType.OnClickDigitalHuman, m_UUID);
            //        Debug.Log("选中"+m_UUID);
            //        isSelected = true;
            //    }

            //}
            //else
            //{

            //    if (isSelected) {
            //        ShowHidSelected(false);
            //        EventCenter.Broadcast(MyEventType.OnCancleSelectDigitalHuman);
            //        isSelected = false;
            //        Debug.Log("取消选中" + m_UUID);
            //    }

            //}

        }
    }
    void ShowHidSelected(bool isShow) {
        selected.gameObject.SetActive(isShow);
        sayHollowBtn.SetActive(isShow);
        m_LeaveMessage.SetActive(isShow);
        if (!isShow) {
            m_MessageParent.SetActive(false);
          
        }
        if (isShow)
        {
            if (m_Video != null) {
                m_Video.Play();
            }
            
        }
        else {
            if (m_Video != null) {
                m_Video.Pause();
            }
          
        }
    }
    void LookAtCamera() {
        showInfoTran.transform.forward = new Vector3(showInfoTran.transform.position.x, 0, showInfoTran.transform.position.z) - new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
    }
    int clickCount = 0;
    int animIndex = -1;
    public void OnClickSayHollow() {
        clickCount += 1;
        animIndex += 1;
        m_HotNum.text = (currenHotNum+ clickCount).ToString();
        m_BezierMove.OnClickSayHollow();
        StartCoroutine(SendClickHotCount(1));
        if (animIndex <= animTriggers.Count-1) {
            m_Anim.SetTrigger(animTriggers[animIndex]);
            if (animIndex == animTriggers.Count - 1) {
                animIndex = -1;
            }
        }
    
    }
    IEnumerator SendClickHotCount(int count) {
        WWWForm form = new WWWForm();
#if UNITY_EDITOR
        form.AddField("deviceModel", "iPhone10,6");
#else
        form.AddField("deviceModel", SystemInfo.deviceModel);
#endif

        form.AddField("heatNumber", count);
        form.AddField("signatureCode", m_UUID);
 
        UnityWebRequest request = UnityWebRequest.Post(sendHotUrl, form);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
        }
    }
    public void OnClickHotMessageBtn() {
        m_MessageParent.SetActive(!m_MessageParent.activeSelf);
        Debug.Log(m_MessageParent.activeSelf);
    }
    public void OnClickDetilBtn() {
        if (!string.IsNullOrEmpty(jumpUrl)) {
            Application.OpenURL(jumpUrl);
        }
       
    }
}

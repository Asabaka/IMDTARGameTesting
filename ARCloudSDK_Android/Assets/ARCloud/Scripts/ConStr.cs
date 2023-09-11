using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConStr
{
    //网页路径
    public const string WEBURL = "http://wxgame.jicf.net:29004";
    public const string AB_PUBLICMODLE = WEBURL + "/AR/user/aradmin/armodel";
    public const string AB_PUBLICMODLE_ANDROID = WEBURL + "/AR/user/aradmin/armodelandroid";

    //服务器IP
    public const string ServerUrl = "http://14.204.63.176:14000";//http://192.168.1.181:8088

    //本地数据路径
    public static string OpenId = string.Empty;
    public static string Token = string.Empty;
    public const string TokenJsonName = "Token.json";
    public static string TokenJsonPath = Application.persistentDataPath + "/GameData/Data/Json";
    public static string NickName = string.Empty;
    public static string Phone = string.Empty;
    public static string CustomerNumber = string.Empty;
    public static string CurrentSceneId = string.Empty;
    public static string CurrentAvatarUrl = string.Empty;
    public static string CurrentVideoUrl = string.Empty;
    public static string CurrentVideoContent = string.Empty;
    public static string CommentId = string.Empty;
    public static string ToId = string.Empty;
    public static string NeedDeleteCommentId = string.Empty;
    public static byte[] imageData;
    public static byte[] thumbData;
    public static string SignatureCode = string.Empty;
    public static string MyCommentContentTitle = string.Empty;
    public static string MyCommentContent = string.Empty;
    public static string MyCommentCreateTime = string.Empty;

    //界面名称
    public const string LOADINGPANEL = "LoadingPanel.prefab";
    //public const string MENUPANEL = "MenuPanel.prefab";
    //public const string MENUPANELTEST = "MenuPanelTest.prefab";
    public const string HOTFIX = "HotFixPanel.prefab";
    public const string FIGHTPANEL = "FightPanel.prefab";
    public const string TESTLOGINPANEL = "TestLoginPanel.prefab";
    public const string LOGINPANEL = "LoginPanel.prefab";
    public const string MYSELFPANEL = "MyselfPanel.prefab";
    //public const string VIDEOPANEL = "VideoPanel.prefab";
    public const string COORPERATIONPANEL = "CooperationPanel.prefab";
    public const string UPDATEMYINFOPANEL = "UpdateMyInfoPanel.prefab";
    public const string UPDATEPHONEPANEL = "UpdatePhonePanel.prefab";
    //public const string DEVELOPINGPANEL = "DevelopingPanel.prefab";
    public const string WECHATBINDPHONEPANEL = "WeChatBindPhonePanel.prefab";
    public const string ARPANEL = "ArPanel.prefab";
    public const string TIPPANEL = "TipPanel.prefab";
    public const string SocialIntercoursePanel = "SocialIntercoursePanel.prefab";
    public const string COMMENTPANEL = "CommentPanel.prefab";
    public const string MYPANEL = "MyPanel.prefab";
    public const string HOMEPAGEPANEL = "HomePagePanel.prefab";
    public const string MYCOMMENTPANEL = "MyCommentPanel.prefab";
    public const string SHAREPANEL = "SharePanel.prefab";
    public const string ABOUTARDIGITALHUMANPANEL = "AboutARDigitalHumanPanel.prefab";
    public const string UESHELPPANEL = "UseHelpPanel.prefab";

    //场景名称
    public const string MENUSCENE = "Assets/GameData/Scenes/Menu.unity";
    public const string MAINSCENE = "Assets/GameData/Scenes/Main.unity";
    public const string FIGHTSCENE = "Assets/GameData/Scenes/Fight.unity";
    public const string FIGHTTESTSCENE = "Assets/GameData/Scenes/FightTest.unity";
    public const string TESTSCENE = "Assets/GameData/Scenes/Test.unity";
    public const string ARScene = "Assets/GameData/Scenes/ARScene.unity";

    //临时prefab路径
    public const string ATTACK = "Assets/GameData/Prefabs/Attack.prefab";

    //临时音乐资源
    public const string MENUSOUND = "Assets/GameData/Sounds/menusound.mp3";

    //数据表路径
    public const string TABLE_SKILL = "Assets/GameData/Data/ProtobufData/FAllSkillData.bytes";
    public const string TABLE_BUFF = "Assets/GameData/Data/ProtobufData/FAllBuffData.bytes";

    #region WebUrl
    //向服务器发送手机号获取验证码
    public const string CodeUrl = ServerUrl + "/arCloudUnityPlus/client/api/sendCode";

    //登陆请求
    public const string LoginUrl = ServerUrl + "/arCloudUnityPlus/client/api/sign";

    //获取登陆信息请求
    public const string GetLoginInfoUrl = ServerUrl + "/arCloudUnityPlus/client/api/user";

    //获取分类信息
    public const string GetSortInfoUrl = ServerUrl + "/arCloudUnityPlus/client/api/types";

    //获取内容列表
    public static string GetContentListUrl(int typeId, int currentPage, int pageSize)
    {
        return string.Format("http://192.168.1.181:8081/arCloudUnityPlus//client/api/home/{0}/{1}/{2}", typeId, currentPage, pageSize);

    }
    //= "http://192.168.1.181:8081/arCloudUnity/client/api/home/{typeId}/{currentPage}/{pageSize}";

    //获取视频
    public static string GetVideoUrl(string sceneId)
    {
        return string.Format("http://192.168.1.181:8081/arCloudUnity/client/api/scene/{0}", sceneId);
    }

    //获取评论
    public static string GetComentUrl(string sceneId, int currentPage, int pageSize)
    {
        return string.Format("{0}/arCloudUnityPlus/client/api/comment/{1}/{2}/{3}", ServerUrl,sceneId, currentPage, pageSize);
    }

    //发送评论
    public const string PostCommentUrl = ServerUrl + "/arCloudUnityPlus/client/api/comment";

    //删除评论
    public static string DeleteCommentUrl(string commentId)
    {
        return string.Format("{0}/arCloudUnity/client/api/deleteComment/{1}", ServerUrl, commentId);
    }

    //点赞url
    public const string LikeUrl = ServerUrl + "/arCloudUnityPlus//client/api/like";

    //点赞评论url
    public const string LikeCommentUrl = ServerUrl + "/arCloudUnityPlus/client/api/likeComment";

	//获取数字人详情
	public static string GetDigitalPerson(string signatureCode)
	{
		return string.Format("{0}/arCloudUnityPlus/client/api/digitalPerson/{1}", ServerUrl, signatureCode);
	}

	//数字人关注接口
	public const string DigitalMenFollowUrl = ServerUrl + "/arCloudUnityPlus/client/api/follow";

    // /arCloudUnityPlus/client/api/digitalPersons/{style}/{currentPage}/{pageSize}
    //数字人作品列表
    public static string DigitalHumanMyContentListUrl(int style,int currentPage,int pageSize)
	{
        return string.Format("{0}/arCloudUnityPlus/client/api/digitalPersons/{1}/{2}/{3}", ServerUrl, style, currentPage, pageSize);
    }

    //获取头像列表-修改信息面板
    public const string GetAvatarUrl = ServerUrl + "/arCloudUnityPlus/client/api/headPortraits";

    //修改信息
    public const string ModifyInformationUrl = ServerUrl + "/arCloudUnityPlus/client/api/user";

    //我要合作url
    public const string CooperationUrl = ServerUrl + "/arCloudUnityPlus/client/api/cooperation";

    //更改手机号url
    public const string UpdatePhoneUrl = ServerUrl + "/arCloudUnityPlus/client/api/userPhoneNumber";

    //协议
    public const string ProrotalUrl = ServerUrl + "/arCloudUnityPlus/user.html";

    //微信登录
    public const string WeChatLoginUrl = ServerUrl + "/arCloudUnityPlus/client/api/wechatSign";

    //微信绑定手机号
    public const string WeChatBindPhoneUrl = ServerUrl + "/arCloudUnityPlus/client/api/wechatBindMobileNumber";
    #endregion
}

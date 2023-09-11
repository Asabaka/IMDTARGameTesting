using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class VideoCro : MonoBehaviour
{

	public static VideoCro instance;

	private VideoPlayer vPlayer;

	public GameObject videoImage;//播放image
	private Button BtnPlay, BtnPause, BtnReStart;//开始，暂停，重播
	public Slider sliderVideo;//进度条
							  //private Button BtnX;//关闭
							  //private Image VideoPanel;//视频背景
	private Text NowTime;//播放时间
	private Text TotalTime;//总时间

	private float tt;//视频总时长
	private float Index_t;//进度条计时时间

	private float hour, min, second;

	private bool IsPlay = true;

	private Slider Audio_Slider;//声音进度条
	private Text AudioNum;//声音数字显示
	public AudioSource audioSource;//声音播放器
	private GameObject audioGameObject;//声音及数字整体组件
	private Button Btn_Audio;//喇叭按钮
	bool a = true;//控制喇叭是否显示

	void Awake()
	{
		instance = this;
		vPlayer = videoImage.GetComponent<VideoPlayer>();
		BtnPlay = transform.Find("Kaishi").GetComponent<Button>();
		BtnPause = transform.Find("Zanting").GetComponent<Button>();
		BtnReStart = transform.Find("BtnReStart").GetComponent<Button>();


		sliderVideo = transform.Find("SliderVideo").GetComponent<Slider>();
		//VideoPanel = transform.Find("VideoPanel").GetComponent<Image>();
		//BtnX = transform.Find("CloseButton").GetComponent<Button>();
		TotalTime = transform.Find("ZongTimeText").GetComponent<Text>();
		NowTime = transform.Find("NowTimeText").GetComponent<Text>();

		Btn_Audio = transform.Find("Btn_Audio").GetComponent<Button>();
		audioGameObject = transform.Find("Audio").gameObject;
		Audio_Slider = transform.Find("Audio/Audio_Slider").GetComponent<Slider>();
		AudioNum = transform.Find("Audio/AudioNum").GetComponent<Text>();
		audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
	}
	/// <summary>
	/// 点击音量按钮，第一次开启，第二次关闭
	/// </summary>
	public void AudioTrue()
	{
		if (a)
		{
			audioGameObject.SetActive(true);
			a = false;
		}
		else
		{
			audioGameObject.SetActive(false);
			a = true;
		}

	}
	public void OnEnable()
	{
		BtnReStart.onClick.AddListener(ClickReStart);
		BtnReStart.onClick.AddListener(ClickReStart);
		//BtnX.onClick.AddListener(ClickBtnX);
		BtnPlay.onClick.AddListener(ClickKaishi);
		BtnPause.onClick.AddListener(ClickZanting);
		Btn_Audio.onClick.AddListener(AudioTrue);
	}
	public void OnDisable()
	{
		BtnReStart.onClick.RemoveListener(ClickReStart);
		BtnReStart.onClick.RemoveListener(ClickReStart);
		//BtnX.onClick.RemoveListener(ClickBtnX);
		BtnPlay.onClick.RemoveListener(ClickKaishi);
		BtnPause.onClick.RemoveListener(ClickZanting);
		Btn_Audio.onClick.RemoveListener(AudioTrue);
		audioGameObject.SetActive(false);
	}
	// Use this for initialization
	void Start()
	{
		ClickKaishi();//是否自动播放

		tt = (float)vPlayer.clip.length;

		sliderVideo.maxValue = tt;

		min = (int)tt / 60;
		second = (int)tt % 60;
		TotalTime.text = string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString());

		AudioChange();
	}
	void Update()
	{
		//播放
		if (IsPlay)
		{
			vPlayer.Play();
			Index_t += Time.deltaTime;
			if (Index_t >= 0.1f)
			{
				sliderVideo.value += 0.1f;
				Index_t = 0;
			}
		}
		else
		{
			vPlayer.Pause();
		}
		//进度条到底停止播放
		if (sliderVideo.maxValue - sliderVideo.value <= 0.1f)
		{
			ClickReStart();
		}
		ChangeTime((float)vPlayer.time);
		AudioChange();
	}
	/// <summary>
	/// 更改视频音量，同时赋值给音量文字
	/// </summary>
	private void AudioChange()
	{
		AudioNum.text = ((int)Audio_Slider.value * 100).ToString() + "%";
		audioSource.volume = Audio_Slider.value;
	}
	public void ChangeVideo(float value)
	{
		vPlayer.time = value;
	}
	/// <summary>
	/// 播放时间显示
	/// </summary>
	/// <param name="value"></param>
	void ChangeTime(float value)
	{
		min = (int)value / 60;
		second = (int)value % 60;
		NowTime.text = string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString());
	}
	/// <summary>
	/// 重播按钮
	/// </summary>
	public void ClickReStart()
	{
		sliderVideo.value = 0;
		vPlayer.time = 0;
	}
	/// <summary>
	/// 关闭按钮
	/// </summary>
	//public void ClickBtnX()
	//{
	//	VideoPanel.gameObject.SetActive(false);
	//}
	/// <summary>
	/// 开始按钮
	/// </summary>
	public void ClickKaishi()
	{
		if (sliderVideo.value == sliderVideo.maxValue)
		{
			sliderVideo.value = 0;
		}
		videoImage.SetActive(true);
		IsPlay = true;
		BtnPause.gameObject.SetActive(true);
		BtnPlay.gameObject.SetActive(false);
	}
	/// <summary>
	/// 暂停按钮
	/// </summary>
	public void ClickZanting()
	{
		IsPlay = false;
		BtnPause.gameObject.SetActive(false);
		BtnPlay.gameObject.SetActive(true);
	}

}
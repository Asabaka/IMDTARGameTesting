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

	public GameObject videoImage;//����image
	private Button BtnPlay, BtnPause, BtnReStart;//��ʼ����ͣ���ز�
	public Slider sliderVideo;//������
							  //private Button BtnX;//�ر�
							  //private Image VideoPanel;//��Ƶ����
	private Text NowTime;//����ʱ��
	private Text TotalTime;//��ʱ��

	private float tt;//��Ƶ��ʱ��
	private float Index_t;//��������ʱʱ��

	private float hour, min, second;

	private bool IsPlay = true;

	private Slider Audio_Slider;//����������
	private Text AudioNum;//����������ʾ
	public AudioSource audioSource;//����������
	private GameObject audioGameObject;//�����������������
	private Button Btn_Audio;//���Ȱ�ť
	bool a = true;//���������Ƿ���ʾ

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
	/// ���������ť����һ�ο������ڶ��ιر�
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
		ClickKaishi();//�Ƿ��Զ�����

		tt = (float)vPlayer.clip.length;

		sliderVideo.maxValue = tt;

		min = (int)tt / 60;
		second = (int)tt % 60;
		TotalTime.text = string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString());

		AudioChange();
	}
	void Update()
	{
		//����
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
		//����������ֹͣ����
		if (sliderVideo.maxValue - sliderVideo.value <= 0.1f)
		{
			ClickReStart();
		}
		ChangeTime((float)vPlayer.time);
		AudioChange();
	}
	/// <summary>
	/// ������Ƶ������ͬʱ��ֵ����������
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
	/// ����ʱ����ʾ
	/// </summary>
	/// <param name="value"></param>
	void ChangeTime(float value)
	{
		min = (int)value / 60;
		second = (int)value % 60;
		NowTime.text = string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString());
	}
	/// <summary>
	/// �ز���ť
	/// </summary>
	public void ClickReStart()
	{
		sliderVideo.value = 0;
		vPlayer.time = 0;
	}
	/// <summary>
	/// �رհ�ť
	/// </summary>
	//public void ClickBtnX()
	//{
	//	VideoPanel.gameObject.SetActive(false);
	//}
	/// <summary>
	/// ��ʼ��ť
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
	/// ��ͣ��ť
	/// </summary>
	public void ClickZanting()
	{
		IsPlay = false;
		BtnPause.gameObject.SetActive(false);
		BtnPlay.gameObject.SetActive(true);
	}

}
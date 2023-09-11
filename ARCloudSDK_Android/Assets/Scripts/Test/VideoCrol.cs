using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class VideoCrol : MonoBehaviour
{

	public static VideoCrol instance;

	private VideoPlayer vPlayer;

	public GameObject videoImage;//����image
	private Button BtnPlay, BtnPause, BtnReStart;//��ʼ����ͣ���ز�
	public Slider sliderVideo;//������
	public Button BtnX;//�ر�
	public Image VideoPanel;//��Ƶ����
	private Text NowTime;//����ʱ��
	private Text TotalTime;//��ʱ��

	private float tt;//��Ƶ��ʱ��
	private float Index_t;//��������ʱʱ��

	private float hour, min, second;

	private bool IsPlay = true;

	void Awake()
	{
		instance = this;
		vPlayer = GetComponent<VideoPlayer>();
		BtnPlay = GameObject.Find("Kaishi").GetComponent<Button>();
		BtnPause = GameObject.Find("Zanting").GetComponent<Button>();
		BtnReStart = GameObject.Find("ReWindButton").GetComponent<Button>();
		sliderVideo = GameObject.Find("SliderVideo").GetComponent<Slider>();
		//VideoPanel = GameObject.Find("VideoPanel").GetComponent<Image>();
		BtnX = GameObject.Find("CloseButton").GetComponent<Button>();
		TotalTime = GameObject.Find("ZongTimeText").GetComponent<Text>();
		NowTime = GameObject.Find("NowTimeText").GetComponent<Text>();
	}

	public void OnEnable()
	{
		BtnReStart.onClick.AddListener(ClickReStart);
		BtnReStart.onClick.AddListener(ClickReStart);
		BtnX.onClick.AddListener(ClickBtnX);
		BtnPlay.onClick.AddListener(ClickKaishi);
		BtnPause.onClick.AddListener(ClickZanting);
	}
	public void OnDisable()
	{
		BtnReStart.onClick.RemoveListener(ClickReStart);
		BtnReStart.onClick.RemoveListener(ClickReStart);
		BtnX.onClick.RemoveListener(ClickBtnX);
		BtnPlay.onClick.RemoveListener(ClickKaishi);
		BtnPause.onClick.RemoveListener(ClickZanting);
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
	public void ClickBtnX()
	{
		//VideoPanel.gameObject.SetActive(false);
	}
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
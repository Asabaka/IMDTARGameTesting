using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoTest : MonoBehaviour
{
    public static VideoTest instance;
    VideoPlayer videoPlayer;
    public Slider sliderVideo;
    public Button startBtn;
    public Image image1;
    public Image image2;
    float totalTime;
    float Index_t;
    float min, second;
    bool isPlay = false;
	// Start is called before the first frame update
	private void Awake()
	{
        instance = this;
        videoPlayer = GetComponent<VideoPlayer>();
    }
	void Start()
    {
        totalTime = (float)videoPlayer.clip.length;
        sliderVideo.maxValue = totalTime;
        min = (int)totalTime / 60;
        second = (int)totalTime % 60;
        Debug.Log(string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString()));
        startBtn.onClick.AddListener(ClickKaishi);
    }

    // Update is called once per frame
    void Update()
    {
        //²¥·Å
        if (isPlay)
        {
            videoPlayer.Play();
            Index_t += Time.deltaTime;
            if (Index_t >= 0.1f)
            {
                sliderVideo.value += 0.1f;
                Index_t = 0;
            }
            ChangeTime((float)videoPlayer.time);
        }
        else
        {
            videoPlayer.Pause();
        }
        if (sliderVideo.maxValue - sliderVideo.value <= 0.1f)
        {
            isPlay = false;
            Debug.Log("Í£Ö¹²¥·Å");
        }
		if (RectTransformUtility.RectangleContainsScreenPoint(image1.rectTransform, Input.mousePosition) && Input.GetMouseButtonDown(0))
		{
            Debug.Log("image11111111");
		}
        if (RectTransformUtility.RectangleContainsScreenPoint(image2.rectTransform, Input.mousePosition) && Input.GetMouseButtonDown(0))
        {
            Debug.Log("image222222222");
        }
    }
    public void ClickKaishi()
    {
        if (sliderVideo.value == sliderVideo.maxValue)
        {
            sliderVideo.value = 0;
        }
        isPlay = true;
    }


    void ChangeTime(float value)
    {
        min = (int)value / 60;
        second = (int)value % 60;
        Debug.Log(string.Format("{0:D2}:{1:D2}", min.ToString(), second.ToString()));
    }

    public void ChangeVideo(float value)
    {
        videoPlayer.time = value;
    }

}

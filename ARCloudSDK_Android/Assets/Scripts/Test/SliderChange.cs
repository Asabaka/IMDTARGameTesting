using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class SliderChange : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    //public VideoPlayer videoPlayer;
    /// <summary>
    /// 拖动改变视频进度
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        VideoTest.instance.ChangeVideo(VideoTest.instance.sliderVideo.value);
    }
    /// <summary>
    /// 点击改变视频进度
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        VideoTest.instance.ChangeVideo(VideoTest.instance.sliderVideo.value);
    }
}
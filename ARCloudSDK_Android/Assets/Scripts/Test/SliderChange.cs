using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class SliderChange : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    //public VideoPlayer videoPlayer;
    /// <summary>
    /// �϶��ı���Ƶ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        VideoTest.instance.ChangeVideo(VideoTest.instance.sliderVideo.value);
    }
    /// <summary>
    /// ����ı���Ƶ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        VideoTest.instance.ChangeVideo(VideoTest.instance.sliderVideo.value);
    }
}
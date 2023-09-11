using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewShowerAni : MonoBehaviour
{
    public ScrollViewAniObj[] scrollviewAniObjs;
    public void OnScrollViewVLChanged(Vector2 value) {
        Debug.Log("x:"+value.x+",y:"+value.y);

        foreach (var itemObj in scrollviewAniObjs)
        {
            itemObj.MoveAniObj2Target(value.y);//GetComponent<ScrollViewAniObj>()
        }
    }
}

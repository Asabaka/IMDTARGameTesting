using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewAniObj : MonoBehaviour
{
    [SerializeField] float whereAmI;//0-1
    [SerializeField] Transform moveTarget;
    [SerializeField] Transform originalTarget;
    //Vector3 originalPos;
    //Vector3 targetPos;

    private void Awake()
    {
        //originalPos = transform.position;
        //targetPos = moveTarget.position;
    }
    public void MoveAniObj2Target(float curr) {
        Debug.Log(curr - whereAmI);
        if (curr > whereAmI)
        {//moveTarget.position
            transform.position = Vector3.Lerp(transform.position, originalTarget.position, Mathf.Abs(curr - whereAmI));//targetPos
        }
        else
        {//originalTarget
            transform.position = Vector3.Lerp(transform.position, moveTarget.position, Mathf.Abs(curr - whereAmI));//targetPos
        }
        //transform.position = Vector3.Lerp(originalTarget.position, moveTarget.position, Mathf.Abs(curr - whereAmI));//targetPos
        
    }
}

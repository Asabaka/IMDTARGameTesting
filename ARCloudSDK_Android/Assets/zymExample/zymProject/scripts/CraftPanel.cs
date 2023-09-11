using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftPanel : MonoBehaviour
{
    public List<CraftObject> _craftobjs;
    public GameObject CraftIns;
    public GameObject CraftIns_peifang1;
    public GameObject CraftIns_peifang2;
    GameObject currIns;

    private void Update()
    {
  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CraftObject>() != null) {
            _craftobjs.Add(other.GetComponent<CraftObject>());
        }

        //do craft
        //demo todo:
        bool ky = false; bool ym = false;
        foreach (var item in _craftobjs)
        {
            if (item.CraftObjName == "gy_ky")
            {
                ky = true;
            }
            if (item.CraftObjName == "gy_ym")
            {
                ym = true;
            }
        }

        if (currIns == null) {
            if (ky && ym)
            {
                currIns = Instantiate(CraftIns, this.transform.position, Quaternion.identity);
            }
        }

        //--
        bool qiongzhi = false;//agar
        bool putaotang = false;//glucose
        bool dianfen = false;//gy_starch
        bool malingshu = false;//gy_potato
        bool weishengsu = false;//gy_B12
        bool danbaizhi = false;//gy_protein-1
        foreach (var item in _craftobjs)
        {
            if (item.CraftObjName == "gy_agar")
            {
                qiongzhi = true;
            }
            if (item.CraftObjName == "gy_glucose")
            {
                putaotang = true;
            }
            if (item.CraftObjName == "gy_starch")
            {
                dianfen = true;
            }
            if (item.CraftObjName == "gy_potato")
            {
                malingshu = true;
            }
            if (item.CraftObjName == "gy_B12")
            {
                weishengsu = true;
            }
            if (item.CraftObjName == "gy_protein")
            {
                danbaizhi = true;
            }
        }

        if (currIns == null)
        {
            if (qiongzhi && putaotang && malingshu && weishengsu && danbaizhi)
            {
                currIns = Instantiate(CraftIns_peifang2, this.transform.parent.parent.position, Quaternion.identity);
            }
            else if (qiongzhi && putaotang && dianfen) {
                currIns = Instantiate(CraftIns_peifang1, this.transform.parent.parent.position, Quaternion.identity);
            }
            else {

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CraftObject>() != null)
        {
            _craftobjs.Remove(other.GetComponent<CraftObject>());
        }
    }

    void elderTst() {
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            Ray _ray = Camera.main.ScreenPointToRay(_touch.position);
            RaycastHit _hitter;
            if (Physics.Raycast(_ray, out _hitter))
            {
                if (_hitter.transform.gameObject == this.gameObject)
                {
                    //do craft
                    //demo todo:
                    bool ky = false; bool ym = false;
                    foreach (var item in _craftobjs)
                    {
                        if (item.CraftObjName == "gy_ky")
                        {
                            ky = true;
                        }
                        if (item.CraftObjName == "gy_ym")
                        {
                            ym = true;
                        }
                    }

                    if (currIns != null) { DestroyImmediate(currIns); }
                    if (ky && ym)
                    {
                        currIns = Instantiate(CraftIns, this.transform);
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //Touch _touch = Input.GetTouch(0);
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hitter;
            if (Physics.Raycast(_ray, out _hitter))
            {
                if (_hitter.transform.gameObject == this.gameObject)
                {
                    //do craft
                    //demo todo:
                    bool ky = false; bool ym = false;
                    foreach (var item in _craftobjs)
                    {
                        if (item.CraftObjName == "gy_ky")
                        {
                            ky = true;
                        }
                        if (item.CraftObjName == "gy_ym")
                        {
                            ym = true;
                        }
                    }

                    if (currIns != null) { DestroyImmediate(currIns); }
                    if (ky && ym)
                    {
                        currIns = Instantiate(CraftIns, this.transform);
                    }
                }
            }
        }
    }
}

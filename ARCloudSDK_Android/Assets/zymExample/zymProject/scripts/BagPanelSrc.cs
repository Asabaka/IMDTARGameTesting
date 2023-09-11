using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanelSrc : MonoBehaviour
{
    public GameObject bagPanel;
    public GameObject maliaomogu;
    public BaginItemTishi tishidonghua;
    CanvasGroup thiscvsgrp;
    //public bool qiongzhi
    //{
    //    get {
    //        return qiongzhi1;
    //    }
    //    set {
    //        if (value != qiongzhi1) {
    //            //on value changed
    //            OnboolVlChanged("«Ì÷¨");
    //        }
    //        qiongzhi1 = value;
    //    }
    //}//gy_agar
    //public bool putaotang
    //{
    //    get
    //    {
    //        return putaotang1;
    //    }
    //    set
    //    {
    //        if (value != putaotang1)
    //        {
    //            //on value changed
    //            OnboolVlChanged("∆œÃ—Ã«");
    //        }
    //        putaotang1 = value;
    //    }
    //}//gy_glucose
    //public bool dianfen
    //{
    //    get
    //    {
    //        return dianfen1;
    //    }
    //    set
    //    {
    //        if (value != dianfen1)
    //        {
    //            //on value changed
    //            OnboolVlChanged("µÌ∑€");
    //        }
    //        dianfen1 = value;
    //    }
    //}//gy_starch
    //public bool malingshu
    //{
    //    get
    //    {
    //        return malingshu1;
    //    }
    //    set
    //    {
    //        if (value != malingshu1)
    //        {
    //            //on value changed
    //            OnboolVlChanged("¬Ì¡Â Ì");
    //        }
    //        malingshu1 = value;
    //    }
    //}//gy_potato
    //public bool weishengsu
    //{
    //    get
    //    {
    //        return weishengsu1;
    //    }
    //    set
    //    {
    //        if (value != weishengsu1)
    //        {
    //            //on value changed
    //            OnboolVlChanged("Œ¨…˙Àÿ");
    //        }
    //        weishengsu1 = value;
    //    }
    //}//gy_B12
    //public bool danbaizhi
    //{
    //    get
    //    {
    //        return danbaizhi1;
    //    }
    //    set
    //    {
    //        if (value != danbaizhi1)
    //        {
    //            //on value changed
    //            OnboolVlChanged("µ∞∞◊÷ ");
    //        }
    //        danbaizhi1 = value;
    //    }
    //}//gy_protein
    public bool qiongzhi
    {
        get
        {
            return _items[0];
        }
        set
        {
            if (value != _items[0])
            {
                //on value changed
                OnboolVlChanged("«Ì÷¨");
            }
            _items[0] = value;
        }
    }//gy_agar
    public bool putaotang
    {
        get
        {
            return _items[1];
        }
        set
        {
            if (value != _items[1])
            {
                //on value changed
                OnboolVlChanged("∆œÃ—Ã«");
            }
            _items[1] = value;
        }
    }//gy_glucose
    public bool dianfen
    {
        get
        {
            return _items[2];
        }
        set
        {
            if (value != _items[2])
            {
                //on value changed
                OnboolVlChanged("µÌ∑€");
            }
            _items[2] = value;
        }
    }//gy_starch
    public bool malingshu
    {
        get
        {
            return _items[3];
        }
        set
        {
            if (value != _items[3])
            {
                //on value changed
                OnboolVlChanged("¬Ì¡Â Ì");
            }
            _items[3] = value;
        }
    }//gy_potato
    public bool weishengsu
    {
        get
        {
            return _items[4];
        }
        set
        {
            if (value != _items[4])
            {
                //on value changed
                OnboolVlChanged("Œ¨…˙Àÿ");
            }
            _items[4] = value;
        }
    }//gy_B12
    public bool danbaizhi
    {
        get
        {
            return _items[5];
        }
        set
        {
            if (value != _items[5])
            {
                //on value changed
                OnboolVlChanged("µ∞∞◊÷ ");
            }
            _items[5] = value;
        }
    }//gy_protein


    //bool qiongzhi1 = false;//gy_agar
    //bool putaotang1 = false;//gy_glucose
    //bool dianfen1 = false;//gy_starch
    //bool malingshu1 = false;//gy_potato
    //bool weishengsu1 = false;//gy_B12
    //bool danbaizhi1 = false;//gy_protein
    bool[] _items = new bool[6];
    public bool bagisShowed;

    public BagClickBttn[] ItemUis;

    //bagclick
    public bool[] bagClick = new bool[6];
    private void Awake()
    {
        thiscvsgrp = GetComponent<CanvasGroup>();
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = false;
        }
        foreach (var _itemUI in ItemUis)
        {
            _itemUI._ownbgpanel = this;
        }
    }
    public void ItemInBag(string itemName) {
        if (itemName == "gy_agar") {
            qiongzhi = true;
        }
        if (itemName == "gy_glucose")
        {
            putaotang = true;
        }
        if (itemName == "gy_starch")
        {
            dianfen = true;
        }
        if (itemName == "gy_potato")
        {
            malingshu = true;
        }
        if (itemName == "gy_B12")
        {
            weishengsu = true;
        }
        if (itemName == "gy_protein")
        {
            danbaizhi = true;
        }
    }
    public void ShowBagCvs() {
        if (bagisShowed == false) {
            bagisShowed = true;
            //thiscvsgrp.interactable = true;
            thiscvsgrp.blocksRaycasts = true;
            bagPanel.SetActive(true);

            for (int i = 0; i < ItemUis.Length; i++)
            {
                ItemUis[i].gameObject.SetActive(_items[i]);
            }
        }
    }

    public void DisableBagCvs() {
        if (bagisShowed == true) {
            //thiscvsgrp.interactable = false;
            thiscvsgrp.blocksRaycasts = false;
            bagisShowed = false;
            bagPanel.SetActive(false);
            maliaomogu.SetActive(false);

            for (int i = 0; i < bagClick.Length; i++)
            {
                bagClick[i] = false;
                ItemUis[i].ResetToggle();
            }
        }
    }

    public void hechengmogu() {
        //if (_items[0] && _items[1] && !_items[2] && !_items[3] && !_items[4] && !_items[5]) 
        //{
        //    Debug.Log("maliao");
        //    maliaomogu.SetActive(true);
        //}
        if (bagClick[0] && bagClick[1] && !bagClick[2] && !bagClick[3] && !bagClick[4] && !bagClick[5])
        {
            Debug.Log("maliao");
            maliaomogu.SetActive(true);
        }
    }

    void OnboolVlChanged(string tishi) {
        //text
        //animation
        tishidonghua.ShowTishi(tishi);
    }
}

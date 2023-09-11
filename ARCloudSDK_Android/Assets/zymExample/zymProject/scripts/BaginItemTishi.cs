using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaginItemTishi : MonoBehaviour
{
    //public GameObject _bagtishi;
    public Text _tst;
    Animator thisani;
    private void Awake()
    {
        //_bagtishi = this.GetComponent<Image>();
        thisani = this.GetComponent<Animator>();
    }

    public void ShowTishi(string tishici) {
        _tst.text ="“" +tishici +"”" +"已加入背包";
        thisani.SetTrigger("play");
    }
}

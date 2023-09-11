using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagClickBttn : MonoBehaviour
{

    public GameObject clckItem;
    public int whichItem;
    [HideInInspector]public BagPanelSrc _ownbgpanel;

    public void BgClickEv() {
        _ownbgpanel.bagClick[whichItem] = !_ownbgpanel.bagClick[whichItem];
        clckItem.SetActive(_ownbgpanel.bagClick[whichItem]);
    }
    public void ResetToggle() {
        clckItem.SetActive(_ownbgpanel.bagClick[whichItem]);
    }
}

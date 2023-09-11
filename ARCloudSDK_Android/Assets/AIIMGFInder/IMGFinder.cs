using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class IMGFinder : MonoBehaviour
{
    private ARTrackedImageManager _trackedImagesManager;
    private ARTrackedObjectManager _trackedobjManager;
    public GameObject[] Arprefbs;
    public GameObject[] Arprefbs_OBJ;
    private readonly Dictionary<string, GameObject> _insttiPrfbs = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, GameObject> _insttiPrfbs_OBJ = new Dictionary<string, GameObject>();
    public BagPanelSrc _bagpanel;
    private void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
        _trackedobjManager = GetComponent<ARTrackedObjectManager>();
    }

    private void OnEnable()
    {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
        _trackedobjManager.trackedObjectsChanged += OnTrackedImagesChanged_OBJ;
    }

    private void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
        _trackedobjManager.trackedObjectsChanged -= OnTrackedImagesChanged_OBJ;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImg in eventArgs.added)
        {
            var imgName = trackedImg.referenceImage.name;
            foreach (var curPrfb in Arprefbs)
            {
                if (string.Compare(curPrfb.name, imgName, System.StringComparison.OrdinalIgnoreCase) == 0
                    && !_insttiPrfbs.ContainsKey(imgName))
                {
                    var newPrefab = Instantiate(curPrfb, trackedImg.transform);
                    _insttiPrfbs[imgName] = newPrefab;
                    if (_bagpanel != null) {
                        _bagpanel.ItemInBag(imgName);
                    }
                }
            }
        }

        foreach (var trackedImg in eventArgs.updated)
        {
            _insttiPrfbs[trackedImg.referenceImage.name].SetActive(trackedImg.trackingState == TrackingState.Tracking);
            //if()
            if (trackedImg.trackingState == TrackingState.Tracking)
            {//&& trackedImg.referenceImage.name != "gy_lyb" && trackedImg.referenceImage.name != "gy_adder"
                _insttiPrfbs[trackedImg.referenceImage.name].transform.SetPositionAndRotation(trackedImg.transform.position, trackedImg.transform.rotation);
                if (trackedImg.referenceImage.name == "gy_adder") {
                    if (_bagpanel != null)
                    {
                        _bagpanel.ShowBagCvs();
                    }
                }
            
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            //Destroy(_insttiPrfbs[trackedImage.referenceImage.name]);
            //_insttiPrfbs.Remove(trackedImage.referenceImage.name);
            _insttiPrfbs[trackedImage.referenceImage.name].SetActive(false);
        }

    }

    void OnTrackedImagesChanged_OBJ(ARTrackedObjectsChangedEventArgs eventArgs)
    {
        foreach (var trackedImg in eventArgs.added)
        {
            var imgName = trackedImg.referenceObject.name;
            foreach (var curPrfb in Arprefbs)
            {
                if (string.Compare(curPrfb.name, imgName, System.StringComparison.OrdinalIgnoreCase) == 0
                    && !_insttiPrfbs.ContainsKey(imgName))
                {
                    var newPrefab = Instantiate(curPrfb, trackedImg.transform);
                    _insttiPrfbs[imgName] = newPrefab;
                }
            }
        }

        foreach (var trackedImg in eventArgs.updated)
        {
            _insttiPrfbs[trackedImg.referenceObject.name].SetActive(trackedImg.trackingState == TrackingState.Tracking);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            //Destroy(_insttiPrfbs[trackedImage.referenceImage.name]);
            //_insttiPrfbs.Remove(trackedImage.referenceImage.name);
            _insttiPrfbs[trackedImage.referenceObject.name].SetActive(false);
        }

    }

}
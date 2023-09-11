using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    public float destoryTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0) {
            Destroy(this.gameObject);
        }
    }
}

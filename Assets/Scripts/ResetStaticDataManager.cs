using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        CuttingCounter.ResetData();
        BaseCounter.ResetData();
        TrashCounter.ResetData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

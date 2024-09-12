using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress 
{
    public event EventHandler<OnProgressBarChangedArgs> OnProgressBarChanged;

    public class OnProgressBarChangedArgs : EventArgs
    {
        public float progressBarNormalize;
    }

    
}

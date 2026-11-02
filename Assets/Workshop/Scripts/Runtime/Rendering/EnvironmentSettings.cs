using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Workshop
{
    [Serializable, VolumeComponentMenu("Workshop/Environment")]
    public class EnvironmentSettings : VolumeComponent
    {
        public FloatParameter effectIntensity = new(0f);
        public ColorParameter effectColor = new(new Color(0f, 0f, 1f));
    }
}
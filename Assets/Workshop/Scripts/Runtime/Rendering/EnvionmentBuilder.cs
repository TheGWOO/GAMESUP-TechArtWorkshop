using UnityEngine;

namespace Workshop
{
    public class EnvironmentBuilder : BaseVolumeGlobalsBuilder<EnvironmentSettings>
    {
        private readonly int EFFECT_INTENSITY_ID = Shader.PropertyToID("_EffectIntensity");
        private readonly int EFFECT_COLOR_ID = Shader.PropertyToID("_EffectColor");
        
        protected override void BuildGlobals(EnvironmentSettings settings, ShaderGlobalsData globals)
        {
            globals.SetFloat(EFFECT_INTENSITY_ID, settings.effectIntensity.value);
            globals.SetColor(EFFECT_COLOR_ID, settings.effectColor.value.linear);
        }
    }
}
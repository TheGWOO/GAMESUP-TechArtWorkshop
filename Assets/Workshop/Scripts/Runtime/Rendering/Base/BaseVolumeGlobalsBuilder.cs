using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Workshop
{
    public abstract class BaseVolumeGlobalsBuilder<TSettings> : BaseShaderGlobalsBuilder where TSettings : VolumeComponent
    {
        private TSettings _defaults;

        protected TSettings GetCameraSettings(ContextContainer frameData)
        {
            CameraType cameraType = frameData.Get<UniversalCameraData>().cameraType;
            if (cameraType != CameraType.Game && cameraType != CameraType.SceneView)
                return GetDefaultSettings();

            TSettings settings = VolumeManager.instance.stack.GetComponent<TSettings>();
            if (settings && settings.active)
                return settings;

            return GetDefaultSettings();
        }

        protected TSettings GetDefaultSettings()
        {
            if (_defaults)
                return _defaults;

            _defaults = ScriptableObject.CreateInstance<TSettings>();
            _defaults.hideFlags = HideFlags.HideAndDontSave;
            _defaults.active = false;
            return _defaults;
        }

        protected override void BuildCameraGlobals(ContextContainer frameData, ShaderGlobalsData globals)
        {
            BuildGlobals(GetCameraSettings(frameData), globals);
        }

        protected override void BuildDefaultGlobals(ShaderGlobalsData globals)
        {
            BuildGlobals(GetDefaultSettings(), globals);
        }

        protected abstract void BuildGlobals(TSettings settings, ShaderGlobalsData globals);

        public override void Dispose()
        {
            try
            {
                base.Dispose();
            }
            finally
            {
                CoreUtils.Destroy(_defaults);
                _defaults = null;
            }
        }
    }
}

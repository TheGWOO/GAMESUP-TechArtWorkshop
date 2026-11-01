using System;
using UnityEngine.Rendering;

namespace Workshop
{
    public abstract class BaseShaderGlobalsBuilder : IDisposable
    {
        internal void AppendCameraGlobals(ContextContainer frameData, ShaderGlobalsData globals)
        {
            BuildCameraGlobals(frameData, globals);
        }

        internal void AppendDefaultGlobals(ShaderGlobalsData globals)
        {
            ResetState();
            BuildDefaultGlobals(globals);
        }

        protected abstract void BuildCameraGlobals(ContextContainer frameData, ShaderGlobalsData globals);
        protected abstract void BuildDefaultGlobals(ShaderGlobalsData globals);

        public virtual void Dispose()
        {
            ResetState();
        }

        // Clear temporal state. May run more than once during teardown;
        // overrides must tolerate repeated calls.
        protected virtual void ResetState() { }
    }
}

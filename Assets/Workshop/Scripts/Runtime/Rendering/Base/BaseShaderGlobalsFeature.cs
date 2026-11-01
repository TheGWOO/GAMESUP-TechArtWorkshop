using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Workshop
{
    public abstract class BaseShaderGlobalsFeature : ScriptableRendererFeature
    {
        protected readonly List<BaseShaderGlobalsBuilder> builders = new();
        private bool _wasActive;
        private ShaderGlobalsPass _pass;
        private readonly ShaderGlobalsData _globalsData = new();

        public sealed override void Create()
        {
            DisposeBuilders();
            RegisterBuilders();
            _pass = new ShaderGlobalsPass(builders);
            _wasActive = isActive;
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        }

        public sealed override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_pass != null)
                renderer.EnqueuePass(_pass);
        }

        protected abstract void RegisterBuilders();

        public void ResetShaderGlobals()
        {
            if (builders.Count == 0)
                return;

            _globalsData.Clear();
            foreach (BaseShaderGlobalsBuilder builder in builders)
            {
                builder.AppendDefaultGlobals(_globalsData);
            }

            CommandBuffer cmd = CommandBufferPool.Get("Reset Shader Globals");
            try
            {
                _globalsData.ApplyTo(CommandBufferHelpers.GetRasterCommandBuffer(cmd));
                Graphics.ExecuteCommandBuffer(cmd);
            }
            finally
            {
                CommandBufferPool.Release(cmd);
            }
        }

        private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (_wasActive && !isActive)
            {
                ResetShaderGlobals();
            }

            _wasActive = isActive;
        }

        private void OnDisable()
        {
            Release();
        }

        protected override void Dispose(bool disposing)
        {
            Release();
            base.Dispose(disposing);
        }

        private void Release()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            DisposeBuilders();
        }

        private void DisposeBuilders()
        {
            ResetShaderGlobals();
            foreach (BaseShaderGlobalsBuilder builder in builders)
            {
                builder.Dispose();
            }

            builders.Clear();
            _pass = null;
        }
    }
}

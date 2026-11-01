using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Workshop
{
    internal sealed class ShaderGlobalsPass : ScriptableRenderPass
    {
        private readonly IReadOnlyList<BaseShaderGlobalsBuilder> _builders;

        public ShaderGlobalsPass(IReadOnlyList<BaseShaderGlobalsBuilder> builders)
        {
            _builders = builders;
            renderPassEvent = RenderPassEvent.BeforeRendering;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<ShaderGlobalsData>("Shader Globals", out var globals);
            
            globals.Clear();

            for (int i = 0; i < _builders.Count; i++)
                _builders[i].AppendCameraGlobals(frameData, globals);
            
            builder.AllowGlobalStateModification(true);
            builder.SetRenderFunc<ShaderGlobalsData>(static (data, context) => data.ApplyTo(context.cmd));
        }
    }
}

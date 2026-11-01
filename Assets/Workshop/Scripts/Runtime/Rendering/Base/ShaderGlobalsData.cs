using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Workshop
{
    public sealed class ShaderGlobalsData
    {
        // Each property uses one value type; repeated writes of that type apply in order.
        private readonly List<(int id, float value)> _floats = new();
        private readonly List<(int id, int value)> _integers = new();
        private readonly List<(int id, Vector4 value)> _vectors = new();
        private readonly List<(int id, Color value)> _colors = new();
        private readonly List<(string name, bool enabled)> _keywords = new();

        public void SetFloat(int id, float value) => _floats.Add((id, value));
        public void SetInteger(int id, int value) => _integers.Add((id, value));
        public void SetVector(int id, Vector4 value) => _vectors.Add((id, value));
        public void SetColor(int id, Color value) => _colors.Add((id, value));
        public void SetKeyword(string name, bool enabled) => _keywords.Add((name, enabled));

        public void Clear()
        {
            _floats.Clear();
            _integers.Clear();
            _vectors.Clear();
            _colors.Clear();
            _keywords.Clear();
        }

        public void ApplyTo(RasterCommandBuffer cmd)
        {
            foreach (var entry in _floats)
                cmd.SetGlobalFloat(entry.id, entry.value);
            foreach (var entry in _integers)
                cmd.SetGlobalInteger(entry.id, entry.value);
            foreach (var entry in _vectors)
                cmd.SetGlobalVector(entry.id, entry.value);
            foreach (var entry in _colors)
                cmd.SetGlobalColor(entry.id, entry.value);
            foreach (var entry in _keywords)
            {
                if (entry.enabled)
                    cmd.EnableShaderKeyword(entry.name);
                else
                    cmd.DisableShaderKeyword(entry.name);
            }
        }
    }
}

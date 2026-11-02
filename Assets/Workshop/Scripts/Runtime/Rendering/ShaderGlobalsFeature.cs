namespace Workshop
{
    public class ShaderGlobalsFeature : BaseShaderGlobalsFeature
    {
        protected override void RegisterBuilders()
        {
            builders.Add(new EnvironmentBuilder());
            builders.Add(new EnvironmentBuilder());
            builders.Add(new EnvironmentBuilder());
            builders.Add(new EnvironmentBuilder());
        }
    }
}

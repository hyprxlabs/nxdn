namespace Hyprx.DotEnv.Serialization;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class EnvIgnoreAttribute : Attribute
{
}
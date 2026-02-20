namespace PaymentServiceApi.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireHmacAttribute : Attribute
{
}
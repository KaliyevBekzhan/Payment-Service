using Domain.Entity;

namespace Domain.Policies;

public static class ConverterPolicy
{
    public static decimal ConvertToTenge(Currency currency, decimal amount)
    {
        return amount * currency.ConversionRate;
    }
}
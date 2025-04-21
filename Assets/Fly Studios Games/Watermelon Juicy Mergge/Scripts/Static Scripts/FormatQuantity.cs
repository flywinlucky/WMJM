using System;

public static class FormatQuantity
{
    public static string FormatQuantityConverter<T>(T quantity)
    {
        float value = Convert.ToSingle(quantity);

        if (value >= 1_000_000_000)
        {
            float formattedQuantity = value / 1_000_000_000f;
            return formattedQuantity.ToString("F1") + "B";
        }
        else if (value >= 1_000_000)
        {
            float formattedQuantity = value / 1_000_000f;
            return formattedQuantity.ToString("F1") + "M";
        }
        else if (value >= 1_000)
        {
            float formattedQuantity = value / 1_000f;
            return formattedQuantity.ToString("F1") + "k";
        }
        else
        {
            return value.ToString();
        }
    }

}
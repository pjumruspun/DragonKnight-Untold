public static class StringExtension
{
    public static string Color(this string input, string colorCode)
    {
        return $"<color=#{colorCode}>" + input + "</color>";
    }

    public static string Size(this string input, int size)
    {
        return $"<size={size.ToString()}>" + input + "</size>";
    }
}

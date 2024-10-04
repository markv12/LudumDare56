public static class StringExtension {
    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);
    public static string TMP_Size(this string str, int size) => string.Format("<size={0}%>{1}</size>", size, str);
    public static string BoldSize(this string str, int size) => string.Format("<b><size={0}>{1}</size></b>", size, str);
}

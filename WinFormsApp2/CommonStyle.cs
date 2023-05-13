using System.Drawing;

public static class CommonStyle
{
    public static Font LabelFont { get; } = new Font("Arial", 12, FontStyle.Regular);
    public static Font TextBoxFont { get; } = new Font("Arial", 12);
    public static Font ButtonFont { get; } = new Font("Arial", 12, FontStyle.Bold);
    public static Color PrimaryColor { get; } = Color.FromArgb(45, 125, 154);
}

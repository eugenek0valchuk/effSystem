public class BaseForm : Form
{
    public BaseForm()
    {
        ApplyCommonDesign();
    }
    protected virtual void ApplyCommonDesign()
    {
        Text = "Investment Project Evaluator";
        Size = new Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;
    }
}

public class CustomLabel : Label
{
    public CustomLabel()
    {
        Font = new Font("Arial", 12, FontStyle.Regular);
        AutoSize = true;
        BackColor = Color.Transparent;
    }
}

public class CustomTextBox : TextBox
{
    public CustomTextBox()
    {
        Font = new Font("Arial", 12);
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(5);
    }
}

public class CustomButton : Button
{
    public CustomButton()
    {
        Font = new Font("Arial", 12, FontStyle.Bold);
        BackColor = Color.FromArgb(45, 125, 154);
        ForeColor = Color.White;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;
        AutoSize = true;
        Padding = new Padding(10, 5, 10, 5);
    }
}
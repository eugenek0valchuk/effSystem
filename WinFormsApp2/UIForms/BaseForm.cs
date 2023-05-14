public class BaseForm : Form
{
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
    public BaseForm()
    {
        ApplyCommonDesign();
    }

    protected virtual void ApplyCommonDesign()
    {
        // Set form properties
        Text = "Investment Project Evaluator";
        Size = new Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        // Set common font styles
        Font labelFont = new Font("Arial", 12, FontStyle.Regular);
        Font buttonFont = new Font("Arial", 12, FontStyle.Bold);

        // Apply common design elements to controls
        ApplyControlStyle(this.Controls, labelFont, buttonFont);
    }

    protected void ApplyControlStyle(Control.ControlCollection controls, Font labelFont, Font buttonFont)
    {
        foreach (Control control in controls)
        {
            if (control is Label label)
            {
                ApplyLabelStyle(label, labelFont);
            }
            else if (control is Button button)
            {
                ApplyButtonStyle(button, buttonFont);
            }
            else if (control is TextBox textBox)
            {
                ApplyTextBoxStyle(textBox);
            }
            else if (control.HasChildren)
            {
                ApplyControlStyle(control.Controls, labelFont, buttonFont);
            }
        }
    }

    protected void ApplyLabelStyle(Label label, Font labelFont)
    {
        label.Font = labelFont;
        label.AutoSize = true;
        label.BackColor = Color.Transparent;
    }

    protected void ApplyButtonStyle(Button button, Font buttonFont)
    {
        button.Font = buttonFont;
        button.BackColor = Color.FromArgb(45, 125, 154);
        button.ForeColor = Color.White;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Cursor = Cursors.Hand;
        button.AutoSize = true;
        button.Padding = new Padding(10, 5, 10, 5);
    }

    protected void ApplyTextBoxStyle(TextBox textBox)
    {
        textBox.Font = new Font("Arial", 12);
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Padding = new Padding(5);
    }
}

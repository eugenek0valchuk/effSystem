namespace InvestmentProjectEvaluator
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            ApplyCommonDesign();
        }

        private void ApplyCommonDesign()
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
            ApplyControlStyle(Controls, labelFont, buttonFont);
        }

        private void ApplyControlStyle(Control.ControlCollection controls, Font labelFont, Font buttonFont)
        {
            foreach (Control control in controls)
            {
                if (control is Label)
                {
                    ApplyLabelStyle((Label)control, labelFont);
                }
                else if (control is Button)
                {
                    ApplyButtonStyle((Button)control, buttonFont);
                }
                else if (control is TextBox)
                {
                    ApplyTextBoxStyle((TextBox)control);
                }
                else if (control.HasChildren)
                {
                    ApplyControlStyle(control.Controls, labelFont, buttonFont);
                }
            }
        }

        private void ApplyLabelStyle(Label label, Font labelFont)
        {
            label.Font = labelFont;
            label.AutoSize = true;
            label.BackColor = Color.Transparent;
        }

        private void ApplyButtonStyle(Button button, Font buttonFont)
        {
            button.Font = buttonFont;
            button.BackColor = CommonStyle.PrimaryColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.AutoSize = true;
            button.Padding = new Padding(10, 5, 10, 5);
        }

        private void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.Font = CommonStyle.TextBoxFont;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Padding = new Padding(5);
        }

        protected Label CreateLabel(string text, Point location)
        {
            return CreateLabel(text, location, CommonStyle.LabelFont);
        }

        protected Label CreateLabel(string text, Point location, Font font)
        {
            return new Label
            {
                Text = text,
                Location = location,
                AutoSize = true,
                Font = font,
                BackColor = Color.Transparent
            };
        }

        protected PictureBox CreatePictureBox(Icon icon, Point location)
        {
            return new PictureBox
            {
                Image = icon.ToBitmap(),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(20, 20),
                Location = location,
                Cursor = Cursors.Help
            };
        }

        protected TextBox CreateTextBox(Point location, Size size)
        {
            return CreateTextBox(location, size, CommonStyle.TextBoxFont);
        }

        protected TextBox CreateTextBox(Point location, Size size, Font font)
        {
            return new TextBox
            {
                Size = size,
                Location = location,
                Font = font
            };
        }

        protected void AddControls(params Control[] controls)
        {
            Controls.AddRange(controls);
        }
    }
}

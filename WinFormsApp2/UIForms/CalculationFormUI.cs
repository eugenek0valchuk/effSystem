public class CalculationFormUI : BaseForm
{
    private TextBox txtInitialInvestment;
    private TextBox txtDiscountRate;
    private TextBox txtCashFlows;
    private TextBox txtInflationRate;
    private TextBox txtTaxRate;
    private TextBox txtPoliticalStabilityRating;
    private DatabaseManager databaseManager;

    public int UserId { get; set; }

    public CalculationFormUI(int userId)
    {
        UserId = userId;
        databaseManager = new DatabaseManager();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Form properties
        Text = "Investment Project Evaluation";
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        // Fonts
        Font labelFont = new Font("Arial", 10, FontStyle.Regular);
        Font textBoxFont = new Font("Arial", 10, FontStyle.Regular);
        Font buttonFont = new Font("Arial", 12, FontStyle.Bold);

        // Create labels, icons, tooltips, and textboxes
        var labelIconSpacing = 10;
        var textBoxIconSpacing = 10;
        var textBoxes = new List<TextBox>();

        var labelsAndTooltips = new[]
        {
        new { LabelText = "Initial Investment:", Icon = SystemIcons.Information, TooltipText = "The initial amount of money invested in the project." },
        new { LabelText = "Discount Rate (%):", Icon = SystemIcons.Information, TooltipText = "The rate at which future cash flows are discounted to determine their present value." },
        new { LabelText = "Cash Flows (comma-separated):", Icon = SystemIcons.Information, TooltipText = "The cash inflows or outflows for each period, separated by commas." },
        new { LabelText = "Inflation Rate (%):", Icon = SystemIcons.Information, TooltipText = "The expected rate of inflation affecting the project." },
        new { LabelText = "Tax Rate (%):", Icon = SystemIcons.Information, TooltipText = "The tax rate applied to the cash flows." },
        new { LabelText = "Political Stability Rating (1-10):", Icon = SystemIcons.Information, TooltipText = "The rating representing the political stability affecting the project. Enter a value between 1 and 10.\n\nExamples:\n1 - Very low stability\n5 - Medium stability\n10 - Very high stability" }
    };

        const int margin = 20;
        const int labelHeight = 30;
        const int textBoxHeight = 30;

        for (int i = 0; i < labelsAndTooltips.Length; i++)
        {
            Label label = new Label
            {
                Text = labelsAndTooltips[i].LabelText,
                Size = new Size(250, labelHeight),
                Location = new Point(margin, margin + i * (labelHeight + labelIconSpacing)),
                Font = labelFont
            };
            Controls.Add(label);

            PictureBox icon = new PictureBox
            {
                Image = labelsAndTooltips[i].Icon.ToBitmap(),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(20, 20),
                Location = new Point(label.Right + labelIconSpacing, label.Top + (labelHeight - 20) / 2),
                Cursor = Cursors.Help
            };
            Controls.Add(icon);

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(icon, labelsAndTooltips[i].TooltipText);

            TextBox textBox = new TextBox
            {
                Location = new Point(icon.Right + textBoxIconSpacing, label.Top),
                Size = new Size(250, textBoxHeight),
                Font = textBoxFont,
                Padding = new Padding(5),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Enter value"
            };
            textBoxes.Add(textBox);
            Controls.Add(textBox);
        }

        Button btnEvaluate = new Button
        {
            Text = "Evaluate",
            Location = new Point(margin, margin + labelsAndTooltips.Length * (labelHeight + labelIconSpacing)),
            Size = new Size(120, 40),
            Font = buttonFont
        };
        btnEvaluate.Click += btnEvaluate_Click;
        Controls.Add(btnEvaluate);

        // Store textboxes in fields for later access
        txtInitialInvestment = textBoxes[0];
        txtDiscountRate = textBoxes[1];
        txtCashFlows = textBoxes[2];
        txtInflationRate = textBoxes[3];
        txtTaxRate = textBoxes[4];
        txtPoliticalStabilityRating = textBoxes[5];
    }

    private void btnEvaluate_Click(object sender, EventArgs e)
    {
        try
        {
            if (double.TryParse(txtInitialInvestment.Text, out double initialInvestment) &&
                double.TryParse(txtDiscountRate.Text, out double discountRate) &&
                double.TryParse(txtInflationRate.Text, out double inflationRate) &&
                double.TryParse(txtTaxRate.Text, out double taxRate) &&
                int.TryParse(txtPoliticalStabilityRating.Text, out int politicalStabilityRating))
            {
                string[] cashFlowsInput = txtCashFlows.Text.Split(',');
                double[] cashFlows = new double[cashFlowsInput.Length];
                for (int i = 0; i < cashFlowsInput.Length; i++)
                {
                    if (double.TryParse(cashFlowsInput[i], out double cashFlow))
                    {
                        cashFlows[i] = cashFlow;
                    }
                    else
                    {
                        MessageBox.Show("Invalid cash flow format. Please enter numeric values separated by commas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                double npv = CalculateNPV(initialInvestment, cashFlows, discountRate, inflationRate, taxRate, politicalStabilityRating);

                bool saved = databaseManager.SaveCalculation(UserId, initialInvestment, discountRate, inflationRate, taxRate, politicalStabilityRating, npv);
                if (saved)
                {
                    MessageBox.Show("Calculation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save the calculation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show($"Net Present Value (NPV): {npv:C}", "Evaluation Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter numeric values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (FormatException)
        {
            MessageBox.Show("Invalid input format. Please enter numeric values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (OverflowException)
        {
            MessageBox.Show("Input value is too large or too small.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred during evaluation:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private double CalculateNPV(double initialInvestment, double[] cashFlows, double discountRate, double inflationRate, double taxRate, int politicalStabilityRating)
    {
        double npv = -initialInvestment;
        double discountFactor = 1 + (discountRate / 100);
        for (int i = 0; i < cashFlows.Length; i++)
        {
            double cashFlow = cashFlows[i];

            cashFlow *= (1 + (inflationRate / 100));
            cashFlow *= (1 - (taxRate / 100));
            cashFlow *= (politicalStabilityRating / 10.0);

            npv += cashFlow / Math.Pow(discountFactor, i + 1);
        }

        return npv;
    }
}


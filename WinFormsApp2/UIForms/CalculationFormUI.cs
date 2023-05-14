public class CalculationFormUI : BaseForm
{
    private TextBox txtInitialInvestment;
    private TextBox txtDiscountRate;
    private TextBox txtCashFlows;
    private TextBox txtInflationRate;
    private TextBox txtTaxRate;
    private TextBox txtPoliticalStabilityRating;
    private DatabaseManager databaseManager; 
    public int UserId { get; private set; }

    public CalculationFormUI(int userId)
    {
        UserId = userId;
        databaseManager = new DatabaseManager();
        InitializeComponents();
    }
    private void InitializeComponents()
    {
        Text = "Investment Project Evaluation";
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.White;

        var labelFont = new Font("Arial", 10, FontStyle.Regular);
        var textBoxFont = new Font("Arial", 10, FontStyle.Regular);
        var buttonFont = new Font("Arial", 12, FontStyle.Bold);

        var labelsAndTooltips = new[]
        {
        new { LabelText = "Initial Investment:", TooltipText = "The initial amount of money invested in the project." },
        new { LabelText = "Discount Rate (%):", TooltipText = "The rate at which future cash flows are discounted to determine their present value." },
        new { LabelText = "Cash Flows (comma-separated):", TooltipText = "The cash inflows or outflows for each period, separated by commas." },
        new { LabelText = "Inflation Rate (%):", TooltipText = "The expected rate of inflation affecting the project." },
        new { LabelText = "Tax Rate (%):", TooltipText = "The tax rate applied to the cash flows." },
        new { LabelText = "Political Stability Rating (1-10):", TooltipText = "The rating representing the political stability affecting the project. Enter a value between 1 and 10." }
    };

        const int margin = 20;
        const int labelHeight = 30;
        const int textBoxHeight = 30;

        var textBoxes = new List<TextBox>(labelsAndTooltips.Length);

        for (int i = 0; i < labelsAndTooltips.Length; i++)
        {
            var label = new Label
            {
                Text = labelsAndTooltips[i].LabelText,
                Size = new Size(250, labelHeight),
                Location = new Point(margin, margin + i * (labelHeight + margin)),
                Font = labelFont
            };
            Controls.Add(label);

            var textBox = new TextBox
            {
                Location = new Point(label.Right + margin, label.Top),
                Size = new Size(250, textBoxHeight),
                Font = textBoxFont,
                Padding = new Padding(5),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle
            };
            textBoxes.Add(textBox);
            Controls.Add(textBox);

            var toolTip = new ToolTip();
            toolTip.SetToolTip(label, labelsAndTooltips[i].TooltipText);
        }

        txtInitialInvestment = textBoxes[0];
        txtDiscountRate = textBoxes[1];
        txtCashFlows = textBoxes[2];
        txtInflationRate = textBoxes[3];
        txtTaxRate = textBoxes[4];
        txtPoliticalStabilityRating = textBoxes[5];

        var btnEvaluate = new Button
        {
            Text = "Evaluate",
            Location = new Point(margin, margin + labelsAndTooltips.Length * (labelHeight + margin)),
            Size = new Size(120, 40),
            Font = buttonFont
        };
        btnEvaluate.Click += btnEvaluate_Click;
        Controls.Add(btnEvaluate);
    }

    private void btnEvaluate_Click(object sender, EventArgs e)
    {
        decimal initialInvestment;
        decimal discountRate;
        decimal inflationRate;
        decimal taxRate;
        decimal politicalStabilityRating;
        decimal[] cashFlows;

        try
        {
            initialInvestment = decimal.Parse(txtInitialInvestment.Text);
            discountRate = decimal.Parse(txtDiscountRate.Text);
            inflationRate = decimal.Parse(txtInflationRate.Text);
            taxRate = decimal.Parse(txtTaxRate.Text);
            politicalStabilityRating = decimal.Parse(txtPoliticalStabilityRating.Text);
            cashFlows = txtCashFlows.Text.Split(',').Select(decimal.Parse).ToArray();
        }
        catch (FormatException)
        {
            MessageBox.Show("Invalid input format. Please enter numeric values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        catch (OverflowException)
        {
            MessageBox.Show("Input value is too large or too small.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        decimal npv = CalculateNPV(initialInvestment, cashFlows, discountRate, inflationRate, taxRate, politicalStabilityRating);

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

    private decimal CalculateNPV(decimal initialInvestment, decimal[] cashFlows, decimal discountRate, decimal inflationRate, decimal taxRate, decimal politicalStabilityRating)
    {
        decimal npv = -initialInvestment;
        decimal discountFactor = 1 + (discountRate / 100);

        for (int i = 0; i < cashFlows.Length; i++)
        {
            decimal cashFlow = cashFlows[i];

            cashFlow *= (1 + (inflationRate / 100));
            cashFlow *= (1 - (taxRate / 100));
            cashFlow *= (politicalStabilityRating / 10m);

            npv += cashFlow / (decimal)Math.Pow((double)discountFactor, i + 1);
        }

        return npv;
    }
}
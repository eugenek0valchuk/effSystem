public class MainMenuFormUI : BaseForm
{
    private CustomButton btnCalculation;
    private CustomButton btnHistory;
    private int userId;

    public MainMenuFormUI(int userId)
    {
        this.userId = userId;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Main Menu";
        Size = new System.Drawing.Size(400, 300);

        btnCalculation = new CustomButton
        {
            Text = "Open Calculation",
            Location = new System.Drawing.Point(100, 100),
        };

        btnHistory = new CustomButton
        {
            Text = "View Calculation History",
            Location = new System.Drawing.Point(100, 160),
        };

        btnCalculation.Click += btnCalculation_Click;
        btnHistory.Click += btnHistory_Click;

        Controls.Add(btnCalculation);
        Controls.Add(btnHistory);
    }

    private void btnCalculation_Click(object sender, EventArgs e)
    {
        CalculationFormUI calculationForm = new CalculationFormUI(userId);
        calculationForm.ShowDialog();
    }

    private void btnHistory_Click(object sender, EventArgs e)
    {
        CalculationInfoFormUI historyForm = new CalculationInfoFormUI(userId);
        historyForm.ShowDialog();
    }
}
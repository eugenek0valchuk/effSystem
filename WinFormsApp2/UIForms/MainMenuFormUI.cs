using InvestmentProjectEvaluator;

public class MainMenuFormUI : Form
{
    private Button btnCalculation;
    private Button btnHistory;
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

        btnCalculation = new Button
        {
            Text = "Open Calculation",
            Location = new System.Drawing.Point(100, 100),
            Size = new System.Drawing.Size(200, 40),
            Font = new Font("Arial", 12, FontStyle.Bold)
        }; 
        btnHistory = new Button
        {
            Text = "View Calculation History",
            Location = new System.Drawing.Point(100, 160),
            Size = new System.Drawing.Size(200, 40),
            Font = new Font("Arial", 12, FontStyle.Bold)
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
        CalculationHistoryFormUI historyForm = new CalculationHistoryFormUI(userId);
        historyForm.ShowDialog();
    }
}
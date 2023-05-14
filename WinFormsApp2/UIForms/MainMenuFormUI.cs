public class MainMenuFormUI : BaseForm
{
    private CustomButton btnCalculation;
    private CustomButton btnHistory;
    private CustomButton btnExit;
    private CustomButton btnUserPanel;
    private int userId;

    public MainMenuFormUI(int userId)
    {
        this.userId = userId;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Main Menu";
        Size = new Size(400, 300);

        btnCalculation = new CustomButton
        {
            Text = "Open Calculation",
            Location = new Point(100, 50),
        };

        btnHistory = new CustomButton
        {
            Text = "View Calculation History",
            Location = new Point(100, 110),
        };

        btnUserPanel = new CustomButton
        {
            Text = "User Panel",
            Location = new Point(100, 170),
        };

        btnExit = new CustomButton
        {
            Text = "Exit",
            Location = new Point(100, 230),
        };

        btnCalculation.Click += btnCalculation_Click;
        btnHistory.Click += btnHistory_Click;
        btnUserPanel.Click += btnUserPanel_Click;
        btnExit.Click += btnExit_Click;

        Controls.Add(btnCalculation);
        Controls.Add(btnHistory);
        Controls.Add(btnUserPanel);
        Controls.Add(btnExit);
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

    private void btnUserPanel_Click(object sender, EventArgs e)
    {
        UserControlPanelUI userPanelForm = new UserControlPanelUI(userId);
        userPanelForm.ShowDialog();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}
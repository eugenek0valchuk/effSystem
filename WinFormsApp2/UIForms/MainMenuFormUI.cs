public class MainMenuFormUI : BaseForm
{
    private Button btnCalculation;
    private Button btnHistory;
    private Button btnExit;
    private Button btnUserPanel;
    private Button btnSettings;
    private int userId;

    public MainMenuFormUI(int userId)
    {
        this.userId = userId;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Main Menu";
        Size = new Size(400, 400);

        btnCalculation = CreateButton("Open Calculation", new Point(100, 50));
        btnCalculation.Click += btnCalculation_Click;

        btnHistory = CreateButton("View Calculation History", new Point(100, 110));
        btnHistory.Click += btnHistory_Click;

        btnUserPanel = CreateButton("User Panel", new Point(100, 170));
        btnUserPanel.Click += btnUserPanel_Click;

        btnSettings = CreateButton("Settings", new Point(100, 230));
        btnSettings.Click += btnSettings_Click;

        btnExit = CreateButton("Exit", new Point(100, 290));
        btnExit.Click += btnExit_Click;

        Controls.Add(btnCalculation);
        Controls.Add(btnHistory);
        Controls.Add(btnUserPanel);
        Controls.Add(btnSettings);
        Controls.Add(btnExit);
    }

    private Button CreateButton(string text, Point location)
    {
        Button button = new Button
        {
            Text = text,
            Location = location,
            Size = new Size(200, 40),
            Font = new Font("Arial", 12, FontStyle.Bold),
            BackColor = Color.FromArgb(45, 125, 154),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };

        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 105, 134);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 145, 174);

        button.Paint += (sender, e) =>
        {
            Button btn = (Button)sender;
            ControlPaint.DrawBorder(e.Graphics, btn.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        };

        return button;
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

    private void btnSettings_Click(object sender, EventArgs e)
    {
        SettingsFormUI settingsForm = new SettingsFormUI();
        settingsForm.ShowDialog();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    protected override void ApplyCommonDesign()
    {
        base.ApplyCommonDesign();
        Text = "Main Menu";
        Size = new Size(600, 600);
    }
}

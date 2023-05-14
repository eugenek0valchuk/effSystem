public class SettingsFormUI : BaseForm
{
    private Button btnSaveSettings;
    private TextBox txtSetting1;
    private TextBox txtSetting2;

    public SettingsFormUI()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Text = "Settings";
        Size = new Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;

        Label lblSetting1 = new Label
        {
            Text = "Setting 1:",
            Location = new Point(20, 20),
            AutoSize = true
        };

        txtSetting1 = new TextBox
        {
            Location = new Point(120, 20),
            Size = new Size(150, 25)
        };

        Label lblSetting2 = new Label
        {
            Text = "Setting 2:",
            Location = new Point(20, 60),
            AutoSize = true
        };

        txtSetting2 = new TextBox
        {
            Location = new Point(120, 60),
            Size = new Size(150, 25)
        };

        btnSaveSettings = new Button
        {
            Text = "Save",
            Location = new Point(100, 110),
            Size = new Size(100, 30)
        };
        btnSaveSettings.Click += btnSaveSettings_Click;

        Controls.Add(lblSetting1);
        Controls.Add(txtSetting1);
        Controls.Add(lblSetting2);
        Controls.Add(txtSetting2);
        Controls.Add(btnSaveSettings);
    }

    private void btnSaveSettings_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    protected override void ApplyCommonDesign()
    {
        base.ApplyCommonDesign();
        Text = "Settings";
        Size = new Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
    }
}
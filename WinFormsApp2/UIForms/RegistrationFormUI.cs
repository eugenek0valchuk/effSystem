public class RegistrationFormUI : BaseForm
{
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnSubmit;
    private DatabaseManager databaseManager;
    private string registeredUsername;
    private string registeredPassword;

    public RegistrationFormUI()
    {
        InitializeComponents();
        databaseManager = new DatabaseManager();
    }

    protected override void ApplyCommonDesign()
    {
        base.ApplyCommonDesign();
        Text = "Registration Form";
        Size = new Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }

    public string GetRegisteredUsername()
    {
        return registeredUsername;
    }

    public string GetRegisteredPassword()
    {
        return registeredPassword;
    }

    private void InitializeComponents()
    {
        Label lblUsername = new Label
        {
            Text = "Username:",
            Location = new Point(20, 20),
            AutoSize = true
        };

        txtUsername = new TextBox
        {
            Location = new Point(120, 20),
            Size = new Size(150, 25)
        };

        Label lblPassword = new Label
        {
            Text = "Password:",
            Location = new Point(20, 60),
            AutoSize = true
        };

        txtPassword = new TextBox
        {
            Location = new Point(120, 60),
            Size = new Size(150, 25),
            PasswordChar = '*'
        };

        btnSubmit = new Button
        {
            Text = "Submit",
            Location = new Point(50, 110),
            Size = new Size(80, 30)
        };
        btnSubmit.Click += btnSubmit_Click;

        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(btnSubmit);
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            if (databaseManager.RegisterUser(username, password))
            {
                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                registeredUsername = username;
                registeredPassword = password;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Username already exists. Please choose a different username.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else
        {
            MessageBox.Show("Please enter both a username and password.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

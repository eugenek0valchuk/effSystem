public class LoginFormUI : BaseForm
{
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnRegister;
    private DatabaseManager databaseManager;

    public int UserId { get; private set; }

    public LoginFormUI()
    {
        InitializeComponents();
        databaseManager = new DatabaseManager();
        databaseManager.InitializeDatabase();
        this.FormClosing += LoginFormUI_FormClosing;
    }
    protected override void ApplyCommonDesign()
    {
        base.ApplyCommonDesign();
        Text = "Login Form";
        Size = new Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
    }
    private void InitializeComponents()
    {
        Text = "Login Form";
        Size = new Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

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

        btnLogin = new Button
        {
            Text = "Login",
            Location = new Point(50, 110),
            Size = new Size(80, 30)
        };
        btnLogin.Click += btnLogin_Click;

        btnRegister = new Button
        {
            Text = "Register",
            Location = new Point(150, 110),
            Size = new Size(80, 30)
        };
        btnRegister.Click += btnRegister_Click;
        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
    }

    private void LoginFormUI_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (DialogResult != DialogResult.OK)
        {
            Application.Exit();
        }
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Please enter both a username and password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int userId = databaseManager.AuthenticateUser(username, password);
        if (userId != -1)
        {
            MessageBox.Show("Authentication successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UserId = userId;
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password. Please try again.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPassword.Clear();
        }
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        RegistrationFormUI registrationForm = new RegistrationFormUI();
        if (registrationForm.ShowDialog() == DialogResult.OK)
        {
            string username = registrationForm.GetRegisteredUsername();
            string password = registrationForm.GetRegisteredPassword();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                int userId = databaseManager.AuthenticateUser(username, password);
                if (userId != -1)
                {
                    MessageBox.Show("Registration successful! You are now logged in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UserId = userId;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                }
            }
            else
            {
                MessageBox.Show("Please enter both a username and password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
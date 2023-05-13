public class LoginFormUI : Form
{
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnRegister;
    private DatabaseManager databaseManager;

    public int UserId { get; set; }

    public LoginFormUI()
    {
        InitializeComponents();
        databaseManager = new DatabaseManager();
        databaseManager.InitializeDatabase();
    }

    private void InitializeComponents()
    {
        // Set form properties
        Text = "Login Form";
        Size = new System.Drawing.Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Create controls
        Label lblUsername = new Label
        {
            Text = "Username:",
            Location = new System.Drawing.Point(20, 20),
            AutoSize = true
        };

        txtUsername = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(150, 25)
        };

        Label lblPassword = new Label
        {
            Text = "Password:",
            Location = new System.Drawing.Point(20, 60),
            AutoSize = true
        }; txtPassword = new TextBox
        {
            Location = new System.Drawing.Point(120, 60),
            Size = new System.Drawing.Size(150, 25),
            PasswordChar = '*'
        };

        btnLogin = new Button
        {
            Text = "Login",
            Location = new System.Drawing.Point(50, 110),
            Size = new System.Drawing.Size(80, 30)
        };
        btnLogin.Click += btnLogin_Click;

        btnRegister = new Button
        {
            Text = "Register",
            Location = new System.Drawing.Point(150, 110),
            Size = new System.Drawing.Size(80, 30)
        };
        btnRegister.Click += btnRegister_Click;

        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblPassword);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            int userId = databaseManager.AuthenticateUser(username, password);
            if (userId != -1)
            {
                MessageBox.Show("Authentication successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserId = userId;
                this.DialogResult = DialogResult.OK;
                this.Close();
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


    private void btnRegister_Click(object sender, EventArgs e)
    {
        RegistrationFormUI registrationForm = new RegistrationFormUI();
        registrationForm.ShowDialog();
    }
}
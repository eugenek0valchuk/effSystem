public class RegistrationFormUI : Form
{
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnSubmit;
    private DatabaseManager databaseManager;

    public RegistrationFormUI()
    {
        InitializeComponents();
        databaseManager = new DatabaseManager();
    }

    private void InitializeComponents()
    {
        // Set form properties
        Text = "Registration Form";
        Size = new System.Drawing.Size(300, 200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;// Create controls
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
        };

        txtPassword = new TextBox
        {
            Location = new System.Drawing.Point(120, 60),
            Size = new System.Drawing.Size(150, 25),
            PasswordChar = '*'
        };

        btnSubmit = new Button
        {
            Text = "Submit",
            Location = new System.Drawing.Point(100, 110),
            Size = new System.Drawing.Size(100, 30)
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
public class RegistrationFormUI : BaseForm
{
    private CustomTextBox txtUsername;
    private CustomTextBox txtPassword;
    private CustomButton btnSubmit;
    private DatabaseManager databaseManager;

    public RegistrationFormUI()
    {
        InitializeComponents();
        databaseManager = new DatabaseManager();
    }

    private void InitializeComponents()
    {
        Text = "Registration Form";
        Size = new System.Drawing.Size(300, 200);

        var lblUsername = new CustomLabel
        {
            Text = "Username:",
            Location = new System.Drawing.Point(20, 20),
        };

        txtUsername = new CustomTextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(150, 25),
        };

        var lblPassword = new CustomLabel
        {
            Text = "Password:",
            Location = new System.Drawing.Point(20, 60),
        };

        txtPassword = new CustomTextBox
        {
            Location = new System.Drawing.Point(120, 60),
            Size = new System.Drawing.Size(150, 25),
            PasswordChar = '*',
        };

        btnSubmit = new CustomButton
        {
            Text = "Submit",
            Location = new System.Drawing.Point(100, 110),
            Size = new System.Drawing.Size(100, 30),
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
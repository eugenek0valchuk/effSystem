public class UserControlPanelUI : BaseForm
{
    private int userId;
    private DatabaseManager databaseManager;
    private Button btnUpdateCredentials;
    private TextBox txtNewPassword;
    private TextBox txtNewUsername;

    public UserControlPanelUI(int userId)
    {
        this.userId = userId;
        databaseManager = new DatabaseManager();
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Label lblUsername = new Label
        {
            Text = "Username:",
            Location = new Point(20, 20),
            AutoSize = true
        };

        TextBox txtUsername = new TextBox
        {
            Location = new Point(140, 20),
            Size = new Size(150, 25),
            Text = databaseManager.GetUsername(userId),
            Enabled = false
        };

        Label lblNewUsername = new Label
        {
            Text = "New Username:",
            Location = new Point(20, 60),
            AutoSize = true
        };

        txtNewUsername = new TextBox
        {
            Location = new Point(140, 60),
            Size = new Size(150, 25)
        };

        Label lblNewPassword = new Label
        {
            Text = "New Password:",
            Location = new Point(20, 100),
            AutoSize = true
        };

        txtNewPassword = new TextBox
        {
            Location = new Point(140, 100),
            Size = new Size(150, 25),
            PasswordChar = '*'
        };

        btnUpdateCredentials = new Button
        {
            Text = "Update Credentials",
            Location = new Point(100, 160),
            Size = new Size(150, 30)
        };
        btnUpdateCredentials.Click += btnUpdateCredentials_Click;

        Controls.Add(lblUsername);
        Controls.Add(txtUsername);
        Controls.Add(lblNewUsername);
        Controls.Add(txtNewUsername);
        Controls.Add(lblNewPassword);
        Controls.Add(txtNewPassword);
        Controls.Add(btnUpdateCredentials);

        ApplyCommonDesign();
    }

    private void btnUpdateCredentials_Click(object sender, EventArgs e)
    {
        string newUsername = txtNewUsername.Text;
        string newPassword = txtNewPassword.Text;

        if (!string.IsNullOrEmpty(newUsername) && !string.IsNullOrEmpty(newPassword))
        {
            string currentUsername = databaseManager.GetUsername(userId);

            if (newUsername != currentUsername && databaseManager.UsernameExists(newUsername))
            {
                MessageBox.Show("The new username is already taken. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (databaseManager.UpdateUserCredentials(userId, newUsername, newPassword))
            {
                MessageBox.Show("User credentials updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update user credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Please enter both a new username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    protected override void ApplyCommonDesign()
    {
        base.ApplyCommonDesign();
        Text = "User Control Panel";
        Size = new Size(350, 300);
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using InvestmentProjectEvaluator;

namespace WinFormsApp2.UIForms
{
    public class UserAccountFormUI : BaseForm
    {
        private TextBox txtUsername;
        private Button btnChangeUsername;
        private Label lblMessage;
        private DatabaseManager databaseManager;
        private int userId;

        public UserAccountFormUI(int userId)
        {
            this.userId = userId;
            databaseManager = new DatabaseManager();
            InitializeComponents();
            LoadUsername();
        }

        private void InitializeComponents()
        {
            // Set form properties
            Text = "User Account";
            var formSize = new System.Drawing.Size(800, 600);

            // Create controls
            lblMessage = CreateLabel("", new Point(20, 20));

            Label lblUsername = CreateLabel("Username:", new Point(20, 60));

            txtUsername = CreateTextBox(new Point(120, 60), new Size(200, 25));

            btnChangeUsername = new Button
            {
                Text = "Change Username",
                Location = new Point(120, 100),
                Size = new Size(150, 30)
            };
            btnChangeUsername.Click += btnChangeUsername_Click;

            // Add controls to form
            Controls.Add(lblMessage);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(btnChangeUsername);
        }

        private void LoadUsername()
        {
            string username = databaseManager.GetUsername(userId);
            txtUsername.Text = username;
        }

        private void btnChangeUsername_Click(object sender, EventArgs e)
        {
            string newUsername = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(newUsername))
            {
                lblMessage.Text = "Please enter a new username.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            if (databaseManager.CheckUsernameExists(newUsername))
            {
                lblMessage.Text = "Username already exists. Please choose a different username.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            if (databaseManager.UpdateUsername(userId, newUsername))
            {
                lblMessage.Text = "Username updated successfully.";
                lblMessage.ForeColor = Color.Green;
            }
            else
            {
                lblMessage.Text = "Failed to update username. Please try again.";
                lblMessage.ForeColor = Color.Red;
            }
        }
    }
}

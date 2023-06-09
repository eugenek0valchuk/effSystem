public class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        DatabaseManager databaseManager = new DatabaseManager();
        databaseManager.InitializeDatabase();
        LoginFormUI loginForm = new LoginFormUI();
        DialogResult loginResult = loginForm.ShowDialog();

        if (loginResult == DialogResult.OK && loginForm.UserId != -1)
        {
            int userId = loginForm.UserId;
            MainMenuFormUI mainMenuForm = new MainMenuFormUI(userId);
            Application.Run(mainMenuForm);
        }
    }
}

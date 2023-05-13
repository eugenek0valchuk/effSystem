using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CalculationHistoryFormUI : Form
{
    private DataGridView dgvCalculationHistory;
    private int userId;
    private DatabaseManager databaseManager;

    public CalculationHistoryFormUI(int userId)
    {
        this.userId = userId;
        databaseManager = new DatabaseManager();
        InitializeComponents();
        LoadCalculationHistoryAsync();
    }

    private void InitializeComponents()
    {
        Text = "Calculation History";
        Size = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;

        dgvCalculationHistory = new DataGridView
        {
            Location = new Point(20, 20),
            Size = new Size(760, 500),
            ReadOnly = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        Controls.Add(dgvCalculationHistory);
    }

    private async void LoadCalculationHistoryAsync()
    {
        try
        {
            DataTable calculationHistory = await Task.Run(() => databaseManager.GetCalculationHistory(userId));
            dgvCalculationHistory.DataSource = calculationHistory;
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading the calculation history: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

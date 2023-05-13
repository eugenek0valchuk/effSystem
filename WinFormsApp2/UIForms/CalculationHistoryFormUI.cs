using InvestmentProjectEvaluator;
using System.Data;

namespace WinFormsApp2.UIForms
{
    public class CalculationHistoryFormUI : BaseForm
    {
        private ListBox lstCalculations;
        private Button btnViewDetails;
        private Button btnDelete;
        private DatabaseManager databaseManager;
        private int userId;

        public CalculationHistoryFormUI(int userId)
        {
            this.userId = userId;
            databaseManager = new DatabaseManager();
            InitializeComponents();
            LoadCalculations();
        }

        private void InitializeComponents()
        {
            // Set form properties
            Text = "Calculation History";
            Size = new Size(600, 400);

            // Create list box to display calculations
            lstCalculations = new ListBox
            {
                Location = new Point(20, 20),
                Size = new Size(400, 300),
                Font = new Font("Arial", 12)
            };

            // Create view details button
            btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(440, 20),
                Size = new Size(120, 30),
                Enabled = false
            };
            //btnViewDetails.Click += btnViewDetails_Click;

            // Create delete button
            btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(440, 60),
                Size = new Size(120, 30),
                Enabled = false
            };
            btnDelete.Click += btnDelete_Click;

            // Add controls to form
            Controls.Add(lstCalculations);
            Controls.Add(btnViewDetails);
            Controls.Add(btnDelete);
        }

        private void LoadCalculations()
        {
            // Clear the list box
            lstCalculations.Items.Clear();

            // Get the calculation history from the database
            DataTable calculationHistory = databaseManager.GetCalculationHistory(userId);

            // Add calculations to the list box
            foreach (DataRow row in calculationHistory.Rows)
            {
                string itemText = $"Calculation ID: {row["Id"]} | NPV: {row["NPV"]:C}";
                lstCalculations.Items.Add(itemText);
            }
        }

        //private void btnViewDetails_Click(object sender, EventArgs e)
        //{
        //    if (lstCalculations.SelectedItem is string selectedItem)
        //    {
        //        // Parse the calculation ID from the selected item
        //        int calculationId = GetCalculationIdFromSelectedItem(selectedItem);

        //        // Retrieve the calculation details from the database
        //        Calculation calculation = databaseManager.GetCalculation(calculationId);

        //        if (calculation != null)
        //        {
        //            // Display the calculation details in a new form
        //            CalculationDetailsFormUI detailsForm = new CalculationDetailsFormUI(calculation);
        //            detailsForm.ShowDialog();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Failed to retrieve the calculation details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstCalculations.SelectedItem is string selectedItem)
            {
                // Parse the calculation ID from the selected item
                int calculationId = GetCalculationIdFromSelectedItem(selectedItem);

                // Delete the calculation from the database
                bool deleted = databaseManager.DeleteCalculation(calculationId);

                if (deleted)
                {
                    MessageBox.Show("Calculation deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the list of calculations
                    LoadCalculations();
                }
                else
                {
                    MessageBox.Show("Failed to delete the calculation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int GetCalculationIdFromSelectedItem(string selectedItem)
        {
            // Extract the calculation ID from the selected item
            int startIndex = selectedItem.IndexOf(":") + 1;
            int endIndex = selectedItem.IndexOf("|") - 1;
            string idText = selectedItem.Substring(startIndex, endIndex - startIndex + 1);
            int calculationId = int.Parse(idText.Trim());

            return calculationId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace TeklaFrameGenerator.UI
{
    public class BaysControl : UserControl
    {
        private DataGridView bayGrid;
        private Button btnAdd, btnCopy, btnDelete;
        private TextBox txtNumberOfFrames;
        private RadioButton radioEqualSpacing, radioArbitrarySpacing;
        private TextBox txtTotalSpacing, txtBaySpacing;
        private Panel previewPanel;

        public event EventHandler DataChanged;

        public BaysControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 600);

            // Bay Management Grid
            bayGrid = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(450, 200),
                AutoGenerateColumns = false,
                AllowUserToAddRows = true
            };

            bayGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Number", Name = "Number" });
            bayGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", Name = "Name" });
            bayGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Description", Name = "Description" });
            bayGrid.Columns.Add(new DataGridViewComboBoxColumn { HeaderText = "Identical", Name = "Identical" });
            bayGrid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Mirror", Name = "Mirror" });

            // Buttons
            btnAdd = new Button { Text = "Add", Location = new Point(10, 220), Width = 75 };
            btnCopy = new Button { Text = "Copy", Location = new Point(95, 220), Width = 75 };
            btnDelete = new Button { Text = "Delete", Location = new Point(180, 220), Width = 75 };

            btnAdd.Click += (s, e) => AddBay();
            btnCopy.Click += (s, e) => CopyBay();
            btnDelete.Click += (s, e) => DeleteBay();

            // Structure dimensions
            var lblFrames = new Label { Text = "n (number of frames):", Location = new Point(10, 260) };
            txtNumberOfFrames = new TextBox { Location = new Point(150, 257), Width = 100 };
            txtNumberOfFrames.TextChanged += (s, e) => OnDataChanged();

            radioEqualSpacing = new RadioButton 
            { 
                Text = "Equal spacing of frames", 
                Location = new Point(10, 290),
                Checked = true
            };
            
            radioArbitrarySpacing = new RadioButton 
            { 
                Text = "Arbitrary spacing of frames", 
                Location = new Point(10, 320)
            };

            radioEqualSpacing.CheckedChanged += (s, e) => OnDataChanged();
            radioArbitrarySpacing.CheckedChanged += (s, e) => OnDataChanged();

            var lblTotalSpacing = new Label { Text = "Total spacing (d):", Location = new Point(30, 320) };
            txtTotalSpacing = new TextBox { Location = new Point(150, 317), Width = 100 };
            txtTotalSpacing.TextChanged += (s, e) => OnDataChanged();

            var lblBaySpacing = new Label { Text = "Bay spacing (db):", Location = new Point(30, 350) };
            txtBaySpacing = new TextBox { Location = new Point(150, 347), Width = 100 };
            txtBaySpacing.TextChanged += (s, e) => OnDataChanged();

            // Preview panel
            previewPanel = new Panel
            {
                Location = new Point(470, 10),
                Size = new Size(220, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Add controls
            this.Controls.Add(bayGrid);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnCopy);
            this.Controls.Add(btnDelete);
            this.Controls.Add(lblFrames);
            this.Controls.Add(txtNumberOfFrames);
            this.Controls.Add(radioEqualSpacing);
            this.Controls.Add(radioArbitrarySpacing);
            this.Controls.Add(lblTotalSpacing);
            this.Controls.Add(txtTotalSpacing);
            this.Controls.Add(lblBaySpacing);
            this.Controls.Add(txtBaySpacing);
            this.Controls.Add(previewPanel);

            // Initial preview
            UpdatePreview();
        }

        private void AddBay()
        {
            int rowIndex = bayGrid.Rows.Add();
            bayGrid.Rows[rowIndex].Cells["Number"].Value = bayGrid.Rows.Count;
            bayGrid.Rows[rowIndex].Cells["Name"].Value = $"Bay{bayGrid.Rows.Count}";
            bayGrid.Rows[rowIndex].Cells["Identical"].Value = "--";
            OnDataChanged();
        }

        private void CopyBay()
        {
            if (bayGrid.CurrentRow != null)
            {
                var row = bayGrid.CurrentRow;
                int newIndex = bayGrid.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    bayGrid.Rows[newIndex].Cells[i].Value = row.Cells[i].Value;
                }
                OnDataChanged();
            }
        }

        private void DeleteBay()
        {
            if (bayGrid.CurrentRow != null)
            {
                bayGrid.Rows.Remove(bayGrid.CurrentRow);
                OnDataChanged();
            }
        }

        private void OnDataChanged()
        {
            UpdatePreview();
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePreview()
        {
            // Draw simple frame preview
            using (Graphics g = previewPanel.CreateGraphics())
            {
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Red, 2);
                
                // Draw simple gable frame
                int centerX = previewPanel.Width / 2;
                int baseY = previewPanel.Height - 50;
                int topY = 100;
                
                // Columns
                g.DrawLine(pen, centerX - 80, baseY, centerX - 80, topY + 50);
                g.DrawLine(pen, centerX + 80, baseY, centerX + 80, topY + 50);
                
                // Roof
                g.DrawLine(pen, centerX - 80, topY + 50, centerX, topY);
                g.DrawLine(pen, centerX, topY, centerX + 80, topY + 50);
            }
        }

        public List<Bay> GetData()
        {
            var bays = new List<Bay>();
            
            foreach (DataGridViewRow row in bayGrid.Rows)
            {
                if (row.IsNewRow) continue;

                var bay = new Bay
                {
                    Number = Convert.ToInt32(row.Cells["Number"].Value ?? 0),
                    Name = row.Cells["Name"].Value?.ToString() ?? "",
                    NumberOfFrames = int.TryParse(txtNumberOfFrames.Text, out int n) ? n : 5,
                    EqualSpacing = radioEqualSpacing.Checked,
                    TotalSpacing = double.TryParse(txtTotalSpacing.Text, out double d) ? d : 12000,
                    IsMirror = Convert.ToBoolean(row.Cells["Mirror"].Value ?? false)
                };

                bays.Add(bay);
            }

            return bays;
        }
    }
}
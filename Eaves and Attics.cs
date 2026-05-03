using System;
using System.Windows.Forms;
using System.Drawing;
using Tekla.Structures.Model;

namespace TeklaFrameGenerator.UI
{
    public class EavesAtticsControl : UserControl
    {
        private CheckBox chkEavesLeft, chkEavesRight;
        private ComboBox cboEavesSectionLeft, cboEavesSectionRight;
        private ComboBox cboEavesMaterialLeft, cboEavesMaterialRight;
        private TextBox txtHel, txtHer, txtBel, txtBer, txtAel, txtAer;
        private TextBox txtHal, txtHar;
        private CheckBox chkAtticsLeft, chkAtticsRight;
        private ComboBox cboAtticsSectionLeft, cboAtticsSectionRight;
        private ComboBox cboAtticsMaterialLeft, cboAtticsMaterialRight;
        private Panel previewPanel;

        public event EventHandler DataChanged;

        public EavesAtticsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 600);

            // Eaves Section
            GroupBox grpEaves = new GroupBox
            {
                Text = "Eaves",
                Location = new Point(10, 10),
                Size = new Size(450, 200)
            };

            chkEavesLeft = new CheckBox { Text = "Left", Location = new Point(15, 25) };
            chkEavesRight = new CheckBox { Text = "Right", Location = new Point(150, 25) };

            var lblSection = new Label { Text = "Section:", Location = new Point(15, 55) };
            cboEavesSectionLeft = new ComboBox { Location = new Point(70, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboEavesSectionRight = new ComboBox { Location = new Point(205, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblMaterial = new Label { Text = "Material:", Location = new Point(15, 85) };
            cboEavesMaterialLeft = new ComboBox { Location = new Point(70, 82), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboEavesMaterialRight = new ComboBox { Location = new Point(205, 82), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            // Eaves Dimensions
            var grpEavesDims = new GroupBox
            {
                Text = "Dimensions of eaves",
                Location = new Point(15, 115),
                Size = new Size(420, 75)
            };

            var lblHel = new Label { Text = "h_el:", Location = new Point(15, 25) };
            txtHel = new TextBox { Location = new Point(50, 22), Width = 80, Text = "5300" };
            var lblHer = new Label { Text = "h_er:", Location = new Point(210, 25) };
            txtHer = new TextBox { Location = new Point(245, 22), Width = 80, Text = "5300" };

            var lblBel = new Label { Text = "b_el:", Location = new Point(15, 50) };
            txtBel = new TextBox { Location = new Point(50, 47), Width = 80, Text = "1500" };
            var lblBer = new Label { Text = "b_er:", Location = new Point(210, 50) };
            txtBer = new TextBox { Location = new Point(245, 47), Width = 80, Text = "1500" };

            var lblAel = new Label { Text = "a_el:", Location = new Point(140, 25) };
            txtAel = new TextBox { Location = new Point(175, 22), Width = 80, Text = "10.00" };
            var lblAer = new Label { Text = "a_er:", Location = new Point(340, 25) };
            txtAer = new TextBox { Location = new Point(375, 22), Width = 80, Text = "10.00" };

            grpEavesDims.Controls.AddRange(new Control[] { lblHel, txtHel, lblHer, txtHer, lblBel, txtBel, lblBer, txtBer, lblAel, txtAel, lblAer, txtAer });

            grpEaves.Controls.AddRange(new Control[] { chkEavesLeft, chkEavesRight, lblSection, cboEavesSectionLeft, cboEavesSectionRight, lblMaterial, cboEavesMaterialLeft, cboEavesMaterialRight, grpEavesDims });

            // Attics Section
            GroupBox grpAttics = new GroupBox
            {
                Text = "Attics",
                Location = new Point(10, 220),
                Size = new Size(450, 150)
            };

            chkAtticsLeft = new CheckBox { Text = "Left", Location = new Point(15, 25) };
            chkAtticsRight = new CheckBox { Text = "Right", Location = new Point(150, 25) };

            var lblAtticsSection = new Label { Text = "Section:", Location = new Point(15, 55) };
            cboAtticsSectionLeft = new ComboBox { Location = new Point(70, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboAtticsSectionRight = new ComboBox { Location = new Point(205, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblAtticsMaterial = new Label { Text = "Material:", Location = new Point(15, 85) };
            cboAtticsMaterialLeft = new ComboBox { Location = new Point(70, 82), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboAtticsMaterialRight = new ComboBox { Location = new Point(205, 82), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var grpAtticsDims = new GroupBox
            {
                Text = "Dimensions of attics",
                Location = new Point(15, 110),
                Size = new Size(420, 35)
            };

            var lblHal = new Label { Text = "h_al:", Location = new Point(15, 12) };
            txtHal = new TextBox { Location = new Point(50, 9), Width = 80, Text = "500" };
            var lblHar = new Label { Text = "h_ar:", Location = new Point(210, 12) };
            txtHar = new TextBox { Location = new Point(245, 9), Width = 80, Text = "500" };

            grpAtticsDims.Controls.AddRange(new Control[] { lblHal, txtHal, lblHar, txtHar });

            grpAttics.Controls.AddRange(new Control[] { chkAtticsLeft, chkAtticsRight, lblAtticsSection, cboAtticsSectionLeft, cboAtticsSectionRight, lblAtticsMaterial, cboAtticsMaterialLeft, cboAtticsMaterialRight, grpAtticsDims });

            // Preview Panel
            previewPanel = new Panel
            {
                Location = new Point(470, 10),
                Size = new Size(220, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Add event handlers
            AttachChangeEventHandlers();

            // Populate combo boxes
            PopulateSections();

            this.Controls.Add(grpEaves);
            this.Controls.Add(grpAttics);
            this.Controls.Add(previewPanel);

            UpdatePreview();
        }

        private void AttachChangeEventHandlers()
        {
            EventHandler onChanged = (s, e) => OnDataChanged();

            chkEavesLeft.CheckedChanged += onChanged;
            chkEavesRight.CheckedChanged += onChanged;
            chkAtticsLeft.CheckedChanged += onChanged;
            chkAtticsRight.CheckedChanged += onChanged;

            txtHel.TextChanged += onChanged;
            txtHer.TextChanged += onChanged;
            txtBel.TextChanged += onChanged;
            txtBer.TextChanged += onChanged;
            txtAel.TextChanged += onChanged;
            txtAer.TextChanged += onChanged;
            txtHal.TextChanged += onChanged;
            txtHar.TextChanged += onChanged;

            cboEavesSectionLeft.SelectedIndexChanged += onChanged;
            cboEavesSectionRight.SelectedIndexChanged += onChanged;
            cboAtticsSectionLeft.SelectedIndexChanged += onChanged;
            cboAtticsSectionRight.SelectedIndexChanged += onChanged;
        }

        private void PopulateSections()
        {
            string[] sections = { "HEA200", "HEA250", "HEA300", "HEB200", "HEB250", "HEB300", "IPE300", "IPE400", "IPE500", "W310X38", "W360X39" };
            string[] materials = { "S235", "S275", "S355", "S420", "Metal Stud Layer" };

            foreach (var section in sections)
            {
                cboEavesSectionLeft.Items.Add(section);
                cboEavesSectionRight.Items.Add(section);
                cboAtticsSectionLeft.Items.Add(section);
                cboAtticsSectionRight.Items.Add(section);
            }

            foreach (var material in materials)
            {
                cboEavesMaterialLeft.Items.Add(material);
                cboEavesMaterialRight.Items.Add(material);
                cboAtticsMaterialLeft.Items.Add(material);
                cboAtticsMaterialRight.Items.Add(material);
            }

            // Set defaults
            cboEavesSectionLeft.SelectedIndex = 8;
            cboEavesSectionRight.SelectedIndex = 8;
            cboAtticsSectionLeft.SelectedIndex = 0;
            cboAtticsSectionRight.SelectedIndex = 0;

            cboEavesMaterialLeft.SelectedIndex = 4;
            cboEavesMaterialRight.SelectedIndex = 4;
            cboAtticsMaterialLeft.SelectedIndex = 4;
            cboAtticsMaterialRight.SelectedIndex = 4;
        }

        private void OnDataChanged()
        {
            UpdatePreview();
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePreview()
        {
            using (Graphics g = previewPanel.CreateGraphics())
            {
                g.Clear(Color.White);
                
                Pen columnPen = new Pen(Color.Gray, 3);
                Pen eavesPen = new Pen(Color.Orange, 3);
                Pen atticPen = new Pen(Color.Yellow, 3);

                int centerX = previewPanel.Width / 2;
                int baseY = previewPanel.Height - 30;

                // Draw columns
                g.DrawLine(columnPen, centerX - 60, baseY, centerX - 60, 150);
                g.DrawLine(columnPen, centerX + 60, baseY, centerX + 60, 150);

                // Draw eaves
                if (chkEavesLeft.Checked)
                {
                    g.DrawLine(eavesPen, centerX - 60, 200, centerX - 100, 220);
                }
                if (chkEavesRight.Checked)
                {
                    g.DrawLine(eavesPen, centerX + 60, 200, centerX + 100, 220);
                }

                // Draw attics
                if (chkAtticsLeft.Checked)
                {
                    g.DrawLine(atticPen, centerX - 60, 100, centerX - 60, 60);
                }
                if (chkAtticsRight.Checked)
                {
                    g.DrawLine(atticPen, centerX + 60, 100, centerX + 60, 60);
                }
            }
        }

        public EavesAtticsData GetData()
        {
            return new EavesAtticsData
            {
                EavesLeft = chkEavesLeft.Checked,
                EavesRight = chkEavesRight.Checked,
                EavesSectionLeft = cboEavesSectionLeft.SelectedItem?.ToString() ?? "W310X38",
                EavesSectionRight = cboEavesSectionRight.SelectedItem?.ToString() ?? "W310X38",
                EavesMaterialLeft = cboEavesMaterialLeft.SelectedItem?.ToString() ?? "Metal Stud Layer",
                EavesMaterialRight = cboEavesMaterialRight.SelectedItem?.ToString() ?? "Metal Stud Layer",
                EavesHeightLeft = double.TryParse(txtHel.Text, out var h) ? h : 5300,
                EavesHeightRight = double.TryParse(txtHer.Text, out var hr) ? hr : 5300,
                EavesWidthLeft = double.TryParse(txtBel.Text, out var b) ? b : 1500,
                EavesWidthRight = double.TryParse(txtBer.Text, out var br) ? br : 1500,
                EavesAngleLeft = double.TryParse(txtAel.Text, out var a) ? a : 10.0,
                EavesAngleRight = double.TryParse(txtAer.Text, out var ar) ? ar : 10.0,
                AtticsLeft = chkAtticsLeft.Checked,
                AtticsRight = chkAtticsRight.Checked,
                AtticsSectionLeft = cboAtticsSectionLeft.SelectedItem?.ToString() ?? "HEA200",
                AtticsSectionRight = cboAtticsSectionRight.SelectedItem?.ToString() ?? "HEA200",
                AtticsMaterialLeft = cboAtticsMaterialLeft.SelectedItem?.ToString() ?? "Metal Stud Layer",
                AtticsMaterialRight = cboAtticsMaterialRight.SelectedItem?.ToString() ?? "Metal Stud Layer",
                AtticsHeightLeft = double.TryParse(txtHal.Text, out var hal) ? hal : 500,
                AtticsHeightRight = double.TryParse(txtHar.Text, out var har) ? har : 500
            };
        }
    }

    public class EavesAtticsData
    {
        public bool EavesLeft { get; set; }
        public bool EavesRight { get; set; }
        public string EavesSectionLeft { get; set; }
        public string EavesSectionRight { get; set; }
        public string EavesMaterialLeft { get; set; }
        public string EavesMaterialRight { get; set; }
        public double EavesHeightLeft { get; set; }
        public double EavesHeightRight { get; set; }
        public double EavesWidthLeft { get; set; }
        public double EavesWidthRight { get; set; }
        public double EavesAngleLeft { get; set; }
        public double EavesAngleRight { get; set; }
        public bool AtticsLeft { get; set; }
        public bool AtticsRight { get; set; }
        public string AtticsSectionLeft { get; set; }
        public string AtticsSectionRight { get; set; }
        public string AtticsMaterialLeft { get; set; }
        public string AtticsMaterialRight { get; set; }
        public double AtticsHeightLeft { get; set; }
        public double AtticsHeightRight { get; set; }
    }
}
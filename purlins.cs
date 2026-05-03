using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace TeklaFrameGenerator.UI
{
    public class PurlinsControl : UserControl
    {
        private CheckBox chkPurlins;
        private RadioButton radioNumberOfPurlins, radioPurlinSpacing;
        private TextBox txtNpl, txtNpr, txtDpl, txtDpr;
        private TextBox txtDprl, txtDprr, txtDpcl, txtDpcr;
        private RadioButton radioSinglePurlins, radioContinuousPurlins;
        private CheckedListBox lstPurlinBraces;
        private ComboBox cboPurlinSection, cboPurlinMaterial;
        private CheckBox chkRidgeBeam;
        private ComboBox cboRidgeBeamSection, cboRidgeBeamMaterial;
        private CheckBox chkStrutLeft, chkStrutRight;
        private ComboBox cboStrutSectionLeft, cboStrutSectionRight;
        private ComboBox cboStrutMaterialLeft, cboStrutMaterialRight;
        private Panel previewPanel;

        public event EventHandler DataChanged;

        public PurlinsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 650);

            // Main checkbox
            chkPurlins = new CheckBox { Text = "Purlins", Location = new Point(10, 10), Checked = true };

            // Definition Group
            GroupBox grpDefinition = new GroupBox
            {
                Text = "Definition",
                Location = new Point(10, 40),
                Size = new Size(450, 110)
            };

            radioNumberOfPurlins = new RadioButton { Text = "Number of purlins", Location = new Point(15, 20), Checked = true };
            radioPurlinSpacing = new RadioButton { Text = "Purlin spacing", Location = new Point(15, 45) };

            var lblNpl = new Label { Text = "n_pl:", Location = new Point(40, 70) };
            txtNpl = new TextBox { Location = new Point(80, 67), Width = 60, Text = "4" };
            var lblNpr = new Label { Text = "n_pr:", Location = new Point(240, 70) };
            txtNpr = new TextBox { Location = new Point(280, 67), Width = 60, Text = "4" };

            var lblDpl = new Label { Text = "d_pl:", Location = new Point(40, 95) };
            txtDpl = new TextBox { Location = new Point(80, 92), Width = 60, Text = "1200" };
            var lblDpr = new Label { Text = "d_pr:", Location = new Point(240, 95) };
            txtDpr = new TextBox { Location = new Point(280, 92), Width = 60, Text = "1200" };

            grpDefinition.Controls.AddRange(new Control[] { radioNumberOfPurlins, radioPurlinSpacing, lblNpl, txtNpl, lblNpr, txtNpr, lblDpl, txtDpl, lblDpr, txtDpr });

            // Dimensions Group
            GroupBox grpDimensions = new GroupBox
            {
                Text = "Dimensions",
                Location = new Point(10, 160),
                Size = new Size(450, 75)
            };

            var lblDprl = new Label { Text = "d_prl:", Location = new Point(15, 25) };
            txtDprl = new TextBox { Location = new Point(60, 22), Width = 80, Text = "200" };
            var lblDprr = new Label { Text = "d_prr:", Location = new Point(210, 25) };
            txtDprr = new TextBox { Location = new Point(255, 22), Width = 80, Text = "200" };

            var lblDpcl = new Label { Text = "d_pcl:", Location = new Point(15, 50) };
            txtDpcl = new TextBox { Location = new Point(60, 47), Width = 80, Text = "200" };
            var lblDpcr = new Label { Text = "d_pcr:", Location = new Point(210, 50) };
            txtDpcr = new TextBox { Location = new Point(255, 47), Width = 80, Text = "200" };

            grpDimensions.Controls.AddRange(new Control[] { lblDprl, txtDprl, lblDprr, txtDprr, lblDpcl, txtDpcl, lblDpcr, txtDpcr });

            // Types of Purlins
            GroupBox grpTypes = new GroupBox
            {
                Text = "Types of purlins",
                Location = new Point(10, 245),
                Size = new Size(200, 70)
            };

            radioSinglePurlins = new RadioButton { Text = "Single purlins", Location = new Point(15, 20), Checked = true };
            radioContinuousPurlins = new RadioButton { Text = "Continuous purlins", Location = new Point(15, 45) };

            grpTypes.Controls.AddRange(new Control[] { radioSinglePurlins, radioContinuousPurlins });

            // Purlin Braces
            GroupBox grpBraces = new GroupBox
            {
                Text = "Purlins with braces",
                Location = new Point(220, 245),
                Size = new Size(240, 150)
            };

            lstPurlinBraces = new CheckedListBox { Location = new Point(15, 20), Size = new Size(210, 120) };
            for (int i = 1; i <= 8; i++)
            {
                lstPurlinBraces.Items.Add($"Purlin {i}");
            }

            grpBraces.Controls.Add(lstPurlinBraces);

            // Purlins Section
            GroupBox grpPurlins = new GroupBox
            {
                Text = "Purlins",
                Location = new Point(10, 325),
                Size = new Size(450, 70)
            };

            var lblPurlinSection = new Label { Text = "Section:", Location = new Point(15, 25) };
            cboPurlinSection = new ComboBox { Location = new Point(70, 22), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblPurlinMaterial = new Label { Text = "Material:", Location = new Point(15, 50) };
            cboPurlinMaterial = new ComboBox { Location = new Point(70, 47), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            grpPurlins.Controls.AddRange(new Control[] { lblPurlinSection, cboPurlinSection, lblPurlinMaterial, cboPurlinMaterial });

            // Roof Ridge Beam
            GroupBox grpRidgeBeam = new GroupBox
            {
                Text = "Roof ridge beam",
                Location = new Point(10, 405),
                Size = new Size(450, 100)
            };

            chkRidgeBeam = new CheckBox { Text = "Beam", Location = new Point(15, 20), Checked = true };

            var lblRidgeSection = new Label { Text = "Section:", Location = new Point(15, 45) };
            cboRidgeBeamSection = new ComboBox { Location = new Point(70, 42), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblRidgeMaterial = new Label { Text = "Material:", Location = new Point(15, 70) };
            cboRidgeBeamMaterial = new ComboBox { Location = new Point(70, 67), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            grpRidgeBeam.Controls.AddRange(new Control[] { chkRidgeBeam, lblRidgeSection, cboRidgeBeamSection, lblRidgeMaterial, cboRidgeBeamMaterial });

            // Strut
            GroupBox grpStrut = new GroupBox
            {
                Text = "Strut",
                Location = new Point(10, 515),
                Size = new Size(450, 100)
            };

            chkStrutLeft = new CheckBox { Text = "Left", Location = new Point(15, 20), Checked = true };
            chkStrutRight = new CheckBox { Text = "Right", Location = new Point(150, 20), Checked = true };

            var lblStrutSection = new Label { Text = "Section:", Location = new Point(15, 45) };
            cboStrutSectionLeft = new ComboBox { Location = new Point(70, 42), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboStrutSectionRight = new ComboBox { Location = new Point(230, 42), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblStrutMaterial = new Label { Text = "Material:", Location = new Point(15, 70) };
            cboStrutMaterialLeft = new ComboBox { Location = new Point(70, 67), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboStrutMaterialRight = new ComboBox { Location = new Point(230, 67), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            grpStrut.Controls.AddRange(new Control[] { chkStrutLeft, chkStrutRight, lblStrutSection, cboStrutSectionLeft, cboStrutSectionRight, lblStrutMaterial, cboStrutMaterialLeft, cboStrutMaterialRight });

            // Preview Panel
            previewPanel = new Panel
            {
                Location = new Point(470, 10),
                Size = new Size(220, 500),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Populate combo boxes
            PopulateSections();

            // Event handlers
            AttachEventHandlers();

            this.Controls.AddRange(new Control[] { chkPurlins, grpDefinition, grpDimensions, grpTypes, grpBraces, grpPurlins, grpRidgeBeam, grpStrut, previewPanel });

            UpdatePreview();
        }

        private void PopulateSections()
        {
            string[] sections = { "HEA200", "HEA250", "HEA300", "IPE300", "IPE400", "IPE500", "W310X38", "W360X39", "C200x75", "C250x80" };
            string[] materials = { "S235", "S275", "S355", "Metal Stud Layer" };

            foreach (var section in sections)
            {
                cboPurlinSection.Items.Add(section);
                cboRidgeBeamSection.Items.Add(section);
                cboStrutSectionLeft.Items.Add(section);
                cboStrutSectionRight.Items.Add(section);
            }

            foreach (var material in materials)
            {
                cboPurlinMaterial.Items.Add(material);
                cboRidgeBeamMaterial.Items.Add(material);
                cboStrutMaterialLeft.Items.Add(material);
                cboStrutMaterialRight.Items.Add(material);
            }

            cboPurlinSection.SelectedIndex = 6;
            cboRidgeBeamSection.SelectedIndex = 6;
            cboStrutSectionLeft.SelectedIndex = 6;
            cboStrutSectionRight.SelectedIndex = 6;

            cboPurlinMaterial.SelectedIndex = 3;
            cboRidgeBeamMaterial.SelectedIndex = 3;
            cboStrutMaterialLeft.SelectedIndex = 3;
            cboStrutMaterialRight.SelectedIndex = 3;
        }

        private void AttachEventHandlers()
        {
            EventHandler onChanged = (s, e) => OnDataChanged();

            chkPurlins.CheckedChanged += onChanged;
            radioNumberOfPurlins.CheckedChanged += onChanged;
            radioPurlinSpacing.CheckedChanged += onChanged;
            radioSinglePurlins.CheckedChanged += onChanged;
            radioContinuousPurlins.CheckedChanged += onChanged;
            chkRidgeBeam.CheckedChanged += onChanged;
            chkStrutLeft.CheckedChanged += onChanged;
            chkStrutRight.CheckedChanged += onChanged;

            ItemCheckEventHandler onListChanged = (s, e) => OnDataChanged();
            lstPurlinBraces.ItemCheck += onListChanged;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox textBox)
                    textBox.TextChanged += onChanged;
                else if (ctrl is GroupBox groupBox)
                    AttachTextBoxHandlers(groupBox, onChanged);
            }
        }

        private void AttachTextBoxHandlers(GroupBox groupBox, EventHandler handler)
        {
            foreach (Control ctrl in groupBox.Controls)
            {
                if (ctrl is TextBox)
                    ctrl.TextChanged += handler;
                else if (ctrl is GroupBox innerGroup)
                    AttachTextBoxHandlers(innerGroup, handler);
            }
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

                Pen framePen = new Pen(Color.Gray, 2);
                Pen purlinPen = new Pen(Color.Orange, 2);
                Pen bracePen = new Pen(Color.Cyan, 1);

                int centerX = previewPanel.Width / 2;
                int baseY = 450;

                // Draw frame
                g.DrawLine(framePen, centerX - 80, baseY, centerX - 80, 150);
                g.DrawLine(framePen, centerX + 80, baseY, centerX + 80, 150);
                g.DrawLine(framePen, centerX - 80, 150, centerX, 80);
                g.DrawLine(framePen, centerX, 80, centerX + 80, 150);

                // Draw purlins
                int numberOfPurlins = int.TryParse(txtNpl.Text, out var n) ? n : 4;
                double spacing = 140.0 / (numberOfPurlins + 1);

                for (int i = 1; i <= numberOfPurlins; i++)
                {
                    int yPos = (int)(150 + i * spacing * 2);
                    
                    // Left slope
                    g.DrawLine(purlinPen, centerX - 80 + (int)(i * 10), yPos, centerX - 70 + (int)(i * 10), yPos);
                    
                    // Right slope
                    g.DrawLine(purlinPen, centerX + 70 - (int)(i * 10), yPos, centerX + 80 - (int)(i * 10), yPos);

                    // Draw braces if checked
                    if (i <= lstPurlinBraces.Items.Count && lstPurlinBraces.GetItemChecked(i - 1))
                    {
                        g.DrawLine(bracePen, centerX - 75 + (int)(i * 10), yPos + 5, centerX - 65 + (int)(i * 10), yPos - 5);
                    }
                }
            }
        }

        public PurlinsData GetData()
        {
            var selectedBraces = new List<int>();
            for (int i = 0; i < lstPurlinBraces.Items.Count; i++)
            {
                if (lstPurlinBraces.GetItemChecked(i))
                    selectedBraces.Add(i + 1);
            }

            return new PurlinsData
            {
                IsEnabled = chkPurlins.Checked,
                UseNumberOfPurlins = radioNumberOfPurlins.Checked,
                NumberOfPurlinsLeft = int.TryParse(txtNpl.Text, out var npl) ? npl : 4,
                NumberOfPurlinsRight = int.TryParse(txtNpr.Text, out var npr) ? npr : 4,
                PurlinSpacingLeft = double.TryParse(txtDpl.Text, out var dpl) ? dpl : 1200,
                PurlinSpacingRight = double.TryParse(txtDpr.Text, out var dpr) ? dpr : 1200,
                PurlinDepthLeft = double.TryParse(txtDprl.Text, out var dprl) ? dprl : 200,
                PurlinDepthRight = double.TryParse(txtDprr.Text, out var dprr) ? dprr : 200,
                PurlinFlangeWidthLeft = double.TryParse(txtDpcl.Text, out var dpcl) ? dpcl : 200,
                PurlinFlangeWidthRight = double.TryParse(txtDpcr.Text, out var dpcr) ? dpcr : 200,
                IsSinglePurlin = radioSinglePurlins.Checked,
                PurlinBraces = selectedBraces,
                PurlinSection = cboPurlinSection.SelectedItem?.ToString() ?? "W310X38",
                PurlinMaterial = cboPurlinMaterial.SelectedItem?.ToString() ?? "Metal Stud Layer",
                HasRidgeBeam = chkRidgeBeam.Checked,
                RidgeBeamSection = cboRidgeBeamSection.SelectedItem?.ToString() ?? "W310X38",
                RidgeBeamMaterial = cboRidgeBeamMaterial.SelectedItem?.ToString() ?? "Metal Stud Layer",
                StrutLeft = chkStrutLeft.Checked,
                StrutRight = chkStrutRight.Checked,
                StrutSectionLeft = cboStrutSectionLeft.SelectedItem?.ToString() ?? "W310X38",
                StrutSectionRight = cboStrutSectionRight.SelectedItem?.ToString() ?? "W310X38",
                StrutMaterialLeft = cboStrutMaterialLeft.SelectedItem?.ToString() ?? "Metal Stud Layer",
                StrutMaterialRight = cboStrutMaterialRight.SelectedItem?.ToString() ?? "Metal Stud Layer"
            };
        }
    }

    public class PurlinsData
    {
        public bool IsEnabled { get; set; }
        public bool UseNumberOfPurlins { get; set; }
        public int NumberOfPurlinsLeft { get; set; }
        public int NumberOfPurlinsRight { get; set; }
        public double PurlinSpacingLeft { get; set; }
        public double PurlinSpacingRight { get; set; }
        public double PurlinDepthLeft { get; set; }
        public double PurlinDepthRight { get; set; }
        public double PurlinFlangeWidthLeft { get; set; }
        public double PurlinFlangeWidthRight { get; set; }
        public bool IsSinglePurlin { get; set; }
        public List<int> PurlinBraces { get; set; } = new List<int>();
        public string PurlinSection { get; set; }
        public string PurlinMaterial { get; set; }
        public bool HasRidgeBeam { get; set; }
        public string RidgeBeamSection { get; set; }
        public string RidgeBeamMaterial { get; set; }
        public bool StrutLeft { get; set; }
        public bool StrutRight { get; set; }
        public string StrutSectionLeft { get; set; }
        public string StrutSectionRight { get; set; }
        public string StrutMaterialLeft { get; set; }
        public string StrutMaterialRight { get; set; }
    }
}
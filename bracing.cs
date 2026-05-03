using System;
using System.Windows.Forms;
using System.Drawing;

namespace TeklaFrameGenerator.UI
{
    public class BracingsControl : UserControl
    {
        private RadioButton radioNumberOfBracings, radioNumberOfJoinedPurlins;
        private TextBox txtNumberOfBracings, txtNumberOfJoinedPurlins;
        private DataGridView bracingGrid;
        private TextBox txtHwl, txtHwr;
        private ComboBox cboWallBracingSectionLeft, cboWallBracingSectionRight;
        private ComboBox cboWallBracingMaterialLeft, cboWallBracingMaterialRight;
        private ComboBox cboRoofBracingSection, cboRoofBracingMaterial;
        private Panel previewPanel;

        public event EventHandler DataChanged;

        public BracingsControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 650);

            // Definition Group
            GroupBox grpDefinition = new GroupBox
            {
                Text = "Definition",
                Location = new Point(10, 10),
                Size = new Size(450, 80)
            };

            radioNumberOfBracings = new RadioButton { Text = "Number of bracings on roof slope", Location = new Point(15, 20), Checked = true };
            txtNumberOfBracings = new TextBox { Location = new Point(250, 17), Width = 60, Text = "3" };

            radioNumberOfJoinedPurlins = new RadioButton { Text = "Number of joined purlins", Location = new Point(15, 45) };
            txtNumberOfJoinedPurlins = new TextBox { Location = new Point(250, 42), Width = 60, Text = "2" };

            grpDefinition.Controls.AddRange(new Control[] { radioNumberOfBracings, txtNumberOfBracings, radioNumberOfJoinedPurlins, txtNumberOfJoinedPurlins });

            // Bracing Grid
            bracingGrid = new DataGridView
            {
                Location = new Point(10, 100),
                Size = new Size(450, 200),
                AutoGenerateColumns = false,
                AllowUserToAddRows = false
            };

            bracingGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Span", Name = "Span", Width = 60 });
            
            var leftWallCol = new DataGridViewComboBoxColumn { HeaderText = "Left wall", Name = "LeftWall", Width = 120 };
            leftWallCol.Items.AddRange("None", "Single X", "Double X", "K-Bracing", "V-Bracing");
            
            var roofCol = new DataGridViewComboBoxColumn { HeaderText = "Roof", Name = "Roof", Width = 120 };
            roofCol.Items.AddRange("None", "Single X", "Double X", "Diagonal");
            
            var rightWallCol = new DataGridViewComboBoxColumn { HeaderText = "Right wall", Name = "RightWall", Width = 120 };
            rightWallCol.Items.AddRange("None", "Single X", "Double X", "K-Bracing", "V-Bracing");

            bracingGrid.Columns.AddRange(leftWallCol, roofCol, rightWallCol);

            // Add rows
            for (int i = 4; i >= 1; i--)
            {
                int rowIndex = bracingGrid.Rows.Add();
                bracingGrid.Rows[rowIndex].Cells["Span"].Value = i.ToString();
                bracingGrid.Rows[rowIndex].Cells["LeftWall"].Value = i == 1 || i == 4 ? "Single X" : "None";
                bracingGrid.Rows[rowIndex].Cells["Roof"].Value = i == 1 || i == 4 ? "Single X" : "None";
                bracingGrid.Rows[rowIndex].Cells["RightWall"].Value = i == 1 || i == 4 ? "Single X" : "None";
            }

            // Dimensions
            GroupBox grpDimensions = new GroupBox
            {
                Text = "Dimensions",
                Location = new Point(10, 310),
                Size = new Size(450, 50)
            };

            var lblHwl = new Label { Text = "h_wl:", Location = new Point(15, 22) };
            txtHwl = new TextBox { Location = new Point(60, 19), Width = 80, Text = "3100" };
            var lblHwr = new Label { Text = "h_wr:", Location = new Point(210, 22) };
            txtHwr = new TextBox { Location = new Point(255, 19), Width = 80, Text = "3100" };

            grpDimensions.Controls.AddRange(new Control[] { lblHwl, txtHwl, lblHwr, txtHwr });

            // Wall Bracing
            GroupBox grpWallBracing = new GroupBox
            {
                Text = "Wall bracing",
                Location = new Point(10, 370),
                Size = new Size(450, 90)
            };

            var lblWallSection = new Label { Text = "Section:", Location = new Point(15, 25) };
            cboWallBracingSectionLeft = new ComboBox { Location = new Point(70, 22), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboWallBracingSectionRight = new ComboBox { Location = new Point(230, 22), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblWallMaterial = new Label { Text = "Material:", Location = new Point(15, 55) };
            cboWallBracingMaterialLeft = new ComboBox { Location = new Point(70, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboWallBracingMaterialRight = new ComboBox { Location = new Point(230, 52), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblLeft = new Label { Text = "Left:", Location = new Point(15, 0) };
            var lblRight = new Label { Text = "Right:", Location = new Point(175, 0) };

            grpWallBracing.Controls.AddRange(new Control[] { lblLeft, lblRight, lblWallSection, cboWallBracingSectionLeft, cboWallBracingSectionRight, lblWallMaterial, cboWallBracingMaterialLeft, cboWallBracingMaterialRight });

            // Roof Bracing
            GroupBox grpRoofBracing = new GroupBox
            {
                Text = "Roof bracing",
                Location = new Point(10, 470),
                Size = new Size(450, 70)
            };

            var lblRoofSection = new Label { Text = "Section:", Location = new Point(15, 25) };
            cboRoofBracingSection = new ComboBox { Location = new Point(70, 22), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            var lblRoofMaterial = new Label { Text = "Material:", Location = new Point(15, 50) };
            cboRoofBracingMaterial = new ComboBox { Location = new Point(70, 47), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            grpRoofBracing.Controls.AddRange(new Control[] { lblRoofSection, cboRoofBracingSection, lblRoofMaterial, cboRoofBracingMaterial });

            // Preview Panel
            previewPanel = new Panel
            {
                Location = new Point(470, 10),
                Size = new Size(220, 550),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Populate sections
            PopulateSections();

            // Event handlers
            AttachEventHandlers();

            this.Controls.AddRange(new Control[] { grpDefinition, bracingGrid, grpDimensions, grpWallBracing, grpRoofBracing, previewPanel });

            UpdatePreview();
        }

        private void PopulateSections()
        {
            string[] sections = { "L80x8", "L100x10", "L120x12", "W310X38", "CHS60.3x3.2", "CHS88.9x4" };
            string[] materials = { "S235", "S275", "S355", "Metal Stud Layer" };

            foreach (var section in sections)
            {
                cboWallBracingSectionLeft.Items.Add(section);
                cboWallBracingSectionRight.Items.Add(section);
                cboRoofBracingSection.Items.Add(section);
            }

            foreach (var material in materials)
            {
                cboWallBracingMaterialLeft.Items.Add(material);
                cboWallBracingMaterialRight.Items.Add(material);
                cboRoofBracingMaterial.Items.Add(material);
            }

            cboWallBracingSectionLeft.SelectedIndex = 0;
            cboWallBracingSectionRight.SelectedIndex = 0;
            cboRoofBracingSection.SelectedIndex = 0;

            cboWallBracingMaterialLeft.SelectedIndex = 0;
            cboWallBracingMaterialRight.SelectedIndex = 0;
            cboRoofBracingMaterial.SelectedIndex = 0;
        }

        private void AttachEventHandlers()
        {
            EventHandler onChanged = (s, e) => OnDataChanged();

            radioNumberOfBracings.CheckedChanged += onChanged;
            radioNumberOfJoinedPurlins.CheckedChanged += onChanged;
            txtNumberOfBracings.TextChanged += onChanged;
            txtNumberOfJoinedPurlins.TextChanged += onChanged;
            txtHwl.TextChanged += onChanged;
            txtHwr.TextChanged += onChanged;

            bracingGrid.CellValueChanged += (s, e) => OnDataChanged();
            
            cboWallBracingSectionLeft.SelectedIndexChanged += onChanged;
            cboWallBracingSectionRight.SelectedIndexChanged += onChanged;
            cboRoofBracingSection.SelectedIndexChanged += onChanged;
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
                Pen bracingPen = new Pen(Color.Yellow, 2);

                int centerX = previewPanel.Width / 2;
                int baseY = 500;

                // Draw frame structure
                g.DrawLine(framePen, centerX - 80, baseY, centerX - 80, 200);
                g.DrawLine(framePen, centerX + 80, baseY, centerX + 80, 200);
                g.DrawLine(framePen, centerX - 80, 200, centerX, 120);
                g.DrawLine(framePen, centerX, 120, centerX + 80, 200);

                // Draw bracings based on grid selection
                for (int i = 0; i < bracingGrid.Rows.Count; i++)
                {
                    if (bracingGrid.Rows[i].IsNewRow) continue;

                    int span = 4 - i;
                    int yPos = baseY - (span * 70);

                    var leftWall = bracingGrid.Rows[i].Cells["LeftWall"].Value?.ToString();
                    var roof = bracingGrid.Rows[i].Cells["Roof"].Value?.ToString();
                    var rightWall = bracingGrid.Rows[i].Cells["RightWall"].Value?.ToString();

                    // Draw left wall bracing
                    if (leftWall != "None" && leftWall != null)
                    {
                        g.DrawLine(bracingPen, centerX - 80, yPos, centerX - 70, yPos - 30);
                        g.DrawLine(bracingPen, centerX - 70, yPos - 30, centerX - 80, yPos - 60);
                    }

                    // Draw roof bracing
                    if (roof != "None" && roof != null)
                    {
                        int roofX = centerX - 40 + (span * 20);
                        int roofY = 150 + (span * 20);
                        g.DrawLine(bracingPen, roofX, roofY, roofX + 15, roofY - 20);
                        g.DrawLine(bracingPen, roofX + 15, roofY - 20, roofX + 30, roofY);
                    }

                    // Draw right wall bracing
                    if (rightWall != "None" && rightWall != null)
                    {
                        g.DrawLine(bracingPen, centerX + 80, yPos, centerX + 70, yPos - 30);
                        g.DrawLine(bracingPen, centerX + 70, yPos - 30, centerX + 80, yPos - 60);
                    }
                }
            }
        }

        public BracingsData GetData()
        {
            var gridData = new System.Collections.Generic.List<BracingSpanData>();

            foreach (DataGridViewRow row in bracingGrid.Rows)
            {
                if (row.IsNewRow) continue;

                gridData.Add(new BracingSpanData
                {
                    Span = Convert.ToInt32(row.Cells["Span"].Value ?? 0),
                    LeftWall = row.Cells["LeftWall"].Value?.ToString() ?? "None",
                    Roof = row.Cells["Roof"].Value?.ToString() ?? "None",
                    RightWall = row.Cells["RightWall"].Value?.ToString() ?? "None"
                });
            }

            return new BracingsData
            {
                UseNumberOfBracings = radioNumberOfBracings.Checked,
                NumberOfBracings = int.TryParse(txtNumberOfBracings.Text, out var nob) ? nob : 3,
                NumberOfJoinedPurlins = int.TryParse(txtNumberOfJoinedPurlins.Text, out var njp) ? njp : 2,
                BracingSpans = gridData,
                WallHeightLeft = double.TryParse(txtHwl.Text, out var hwl) ? hwl : 3100,
                WallHeightRight = double.TryParse(txtHwr.Text, out var hwr) ? hwr : 3100,
                WallBracingSectionLeft = cboWallBracingSectionLeft.SelectedItem?.ToString() ?? "L80x8",
                WallBracingSectionRight = cboWallBracingSectionRight.SelectedItem?.ToString() ?? "L80x8",
                WallBracingMaterialLeft = cboWallBracingMaterialLeft.SelectedItem?.ToString() ?? "S235",
                WallBracingMaterialRight = cboWallBracingMaterialRight.SelectedItem?.ToString() ?? "S235",
                RoofBracingSection = cboRoofBracingSection.SelectedItem?.ToString() ?? "L80x8",
                RoofBracingMaterial = cboRoofBracingMaterial.SelectedItem?.ToString() ?? "S235"
            };
        }
    }

    public class BracingSpanData
    {
        public int Span { get; set; }
        public string LeftWall { get; set; }
        public string Roof { get; set; }
        public string RightWall { get; set; }
    }

    public class BracingsData
    {
        public bool UseNumberOfBracings { get; set; }
        public int NumberOfBracings { get; set; }
        public int NumberOfJoinedPurlins { get; set; }
        public System.Collections.Generic.List<BracingSpanData> BracingSpans { get; set; }
        public double WallHeightLeft { get; set; }
        public double WallHeightRight { get; set; }
        public string WallBracingSectionLeft { get; set; }
        public string WallBracingSectionRight { get; set; }
        public string WallBracingMaterialLeft { get; set; }
        public string WallBracingMaterialRight { get; set; }
        public string RoofBracingSection { get; set; }
        public string RoofBracingMaterial { get; set; }
    }
}
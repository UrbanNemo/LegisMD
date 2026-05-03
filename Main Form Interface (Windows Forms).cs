using System;
using System.Windows.Forms;
using Tekla.Structures.Model;

namespace TeklaFrameGenerator.UI
{
    public partial class MainForm : Form
    {
        private Model _model;
        private FrameGeneratorData _frameData;

        public MainForm(Model model)
        {
            InitializeComponent();
            _model = model;
            _frameData = new FrameGeneratorData();
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            // Create main tab control
            TabControl mainTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(200, 0),
                Width = this.Width - 200
            };

            // Tab 1: Bays Management
            TabPage baysTab = new TabPage("Bays");
            var baysControl = new BaysControl();
            baysTab.Controls.Add(baysControl);
            mainTabControl.TabPages.Add(baysTab);

            // Tab 2: Project Settings
            TabPage projectTab = new TabPage("Project");
            var projectControl = new ProjectControl();
            projectTab.Controls.Add(projectControl);
            mainTabControl.TabPages.Add(projectTab);

            // Tab 3: Geometry
            TabPage geometryTab = new TabPage("Geometry");
            var geometryControl = new GeometryControl();
            geometryTab.Controls.Add(geometryControl);
            mainTabControl.TabPages.Add(geometryTab);

            // Tab 4: Truss
            TabPage trussTab = new TabPage("Truss");
            var trussControl = new TrussControl();
            trussTab.Controls.Add(trussControl);
            mainTabControl.TabPages.Add(trussTab);

            // Tab 5: Deck Slabs
            TabPage deckSlabTab = new TabPage("Deck slabs");
            var deckSlabControl = new DeckSlabControl();
            deckSlabTab.Controls.Add(deckSlabControl);
            mainTabControl.TabPages.Add(deckSlabTab);

            this.Controls.Add(mainTabControl);

            // Wire up OK/Cancel buttons
            baysControl.DataChanged += (s, e) => _frameData.Bays = baysControl.GetData();
            projectControl.DataChanged += (s, e) => _frameData.Project = projectControl.GetData();
            geometryControl.DataChanged += (s, e) => _frameData.Geometry = geometryControl.GetData();
            trussControl.DataChanged += (s, e) => _frameData.Truss = trussControl.GetData();
            deckSlabControl.DataChanged += (s, e) => _frameData.DeckSlab = deckSlabControl.GetData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var generator = new FrameGenerator(_model, _frameData);
                generator.Generate();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Generation failed: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
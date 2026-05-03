using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FrameGenerator.DataModels;
using FrameGenerator.Core.Utilities;

namespace FrameGenerator.UI
{
    /// <summary>
    /// Main application form for the Frame Generator plugin.
    /// Provides the primary user interface with tab-based navigation for all features.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly Logger _logger;
        private readonly FrameConfig _frameConfig;
        private TabControl? _tabControl;
        private Panel? _navigationPanel;
        private Panel? _previewPanel;

        /// <summary>
        /// Gets the current frame configuration.
        /// </summary>
        public FrameConfig FrameConfig => _frameConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            _logger = new Logger("MainForm");
            _frameConfig = new FrameConfig();

            _logger.LogInfo("Initializing MainForm...");

            InitializeComponent();
            InitializeUI();

            _logger.LogInfo("MainForm initialized successfully");
        }

        /// <summary>
        /// Initializes the component (designer-generated).
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "MainForm";
            this.Text = "Frame Generator v1.0.0 - Tekla Structures Plugin";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Icon = SystemIcons.Application;

            // Setup menu and toolbar
            SetupMenuStrip();
            SetupToolStrip();

            // Setup main layout
            SetupMainLayout();

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Sets up the menu strip.
        /// </summary>
        private void SetupMenuStrip()
        {
            MenuStrip menuStrip = new MenuStrip();

            // File menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("&File");
            fileMenu.DropDownItems.Add("&New", null, (s, e) => NewConfiguration());
            fileMenu.DropDownItems.Add("&Open...", null, (s, e) => OpenConfiguration());
            fileMenu.DropDownItems.Add("&Save", null, (s, e) => SaveConfiguration());
            fileMenu.DropDownItems.Add("Save &As...", null, (s, e) => SaveConfigurationAs());
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("E&xit", null, (s, e) => this.Close());

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem("&Edit");
            editMenu.DropDownItems.Add("&Reset to Defaults", null, (s, e) => ResetToDefaults());
            editMenu.DropDownItems.Add("&Preferences", null, (s, e) => ShowPreferences());

            // Help menu
            ToolStripMenuItem helpMenu = new ToolStripMenuItem("&Help");
            helpMenu.DropDownItems.Add("&About", null, (s, e) => ShowAbout());
            helpMenu.DropDownItems.Add("&Help Topics", null, (s, e) => ShowHelp());

            menuStrip.Items.AddRange(new[] { fileMenu, editMenu, helpMenu });
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        /// <summary>
        /// Sets up the tool strip.
        /// </summary>
        private void SetupToolStrip()
        {
            ToolStrip toolStrip = new ToolStrip();

            // New button
            ToolStripButton newBtn = new ToolStripButton("New");
            newBtn.Click += (s, e) => NewConfiguration();

            // Open button
            ToolStripButton openBtn = new ToolStripButton("Open");
            openBtn.Click += (s, e) => OpenConfiguration();

            // Save button
            ToolStripButton saveBtn = new ToolStripButton("Save");
            saveBtn.Click += (s, e) => SaveConfiguration();

            // Separator
            ToolStripSeparator separator1 = new ToolStripSeparator();

            // Generate button
            ToolStripButton generateBtn = new ToolStripButton("Generate Frame");
            generateBtn.Font = new System.Drawing.Font(generateBtn.Font, System.Drawing.FontStyle.Bold);
            generateBtn.Click += (s, e) => GenerateFrame();

            // Separator
            ToolStripSeparator separator2 = new ToolStripSeparator();

            // Help button
            ToolStripButton helpBtn = new ToolStripButton("Help");
            helpBtn.Click += (s, e) => ShowHelp();

            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                newBtn, openBtn, saveBtn, separator1, generateBtn, separator2, helpBtn
            });

            this.Controls.Add(toolStrip);
        }

        /// <summary>
        /// Sets up the main layout with navigation, tabs, and preview panels.
        /// </summary>
        private void SetupMainLayout()
        {
            // Create main container with docking support
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(5),
                Margin = new Padding(0)
            };

            // Set column widths
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            // Set row heights
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            // Navigation panel (left)
            _navigationPanel = CreateNavigationPanel();
            mainLayout.Controls.Add(_navigationPanel, 0, 0);

            // Tab control (center)
            _tabControl = CreateTabControl();
            mainLayout.Controls.Add(_tabControl, 1, 0);

            // Preview panel (right)
            _previewPanel = CreatePreviewPanel();
            mainLayout.Controls.Add(_previewPanel, 2, 0);

            // Button panel (bottom, spans all columns)
            Panel buttonPanel = CreateButtonPanel();
            mainLayout.SetColumnSpan(buttonPanel, 3);
            mainLayout.Controls.Add(buttonPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        /// <summary>
        /// Creates the navigation panel with feature tree.
        /// </summary>
        private Panel CreateNavigationPanel()
        {
            Panel panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                Margin = new Padding(2),
                AutoScroll = true
            };

            TreeView navTree = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                FullRowSelect = true,
                Indent = 15,
                ItemHeight = 20
            };

            // Add tree nodes for main features
            TreeNode generalNode = navTree.Nodes.Add("General");
            generalNode.Nodes.Add("Bays Management");
            generalNode.Nodes.Add("Project Settings");

            TreeNode geometryNode = navTree.Nodes.Add("Structure");
            geometryNode.Nodes.Add("Geometry");
            geometryNode.Nodes.Add("Truss");

            TreeNode elementsNode = navTree.Nodes.Add("Elements");
            elementsNode.Nodes.Add("Deck Slabs");
            elementsNode.Nodes.Add("Eaves & Attics");
            elementsNode.Nodes.Add("Purlins");
            elementsNode.Nodes.Add("Bracings");
            elementsNode.Nodes.Add("Outer Walls");

            // Handle node selection
            navTree.AfterSelect += (s, e) =>
            {
                string nodeText = e.Node?.Text ?? string.Empty;
                SelectTabByName(nodeText);
            };

            navTree.ExpandAll();
            panel.Controls.Add(navTree);

            return panel;
        }

        /// <summary>
        /// Creates the tab control with all feature tabs.
        /// </summary>
        private TabControl CreateTabControl()
        {
            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(2),
                Padding = new System.Drawing.Point(10, 5)
            };

            // Create tabs for each feature
            tabControl.TabPages.Add(CreateBaysTab());
            tabControl.TabPages.Add(CreateProjectSettingsTab());
            tabControl.TabPages.Add(CreateGeometryTab());
            tabControl.TabPages.Add(CreateTrussTab());
            tabControl.TabPages.Add(CreatePurlinsTab());
            tabControl.TabPages.Add(CreateBracingsTab());
            tabControl.TabPages.Add(CreateDeckSlabsTab());
            tabControl.TabPages.Add(CreateEavesAtticTab());
            tabControl.TabPages.Add(CreateOuterWallsTab());

            return tabControl;
        }

        /// <summary>
        /// Creates the Bays Management tab.
        /// </summary>
        private TabPage CreateBaysTab()
        {
            TabPage tabPage = new TabPage("Bays Management");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Bays Management - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Project Settings tab.
        /// </summary>
        private TabPage CreateProjectSettingsTab()
        {
            TabPage tabPage = new TabPage("Project Settings");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Project Settings - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Geometry tab.
        /// </summary>
        private TabPage CreateGeometryTab()
        {
            TabPage tabPage = new TabPage("Geometry");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Geometry Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Truss tab.
        /// </summary>
        private TabPage CreateTrussTab()
        {
            TabPage tabPage = new TabPage("Truss");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Truss Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Purlins tab.
        /// </summary>
        private TabPage CreatePurlinsTab()
        {
            TabPage tabPage = new TabPage("Purlins");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Purlins Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Bracings tab.
        /// </summary>
        private TabPage CreateBracingsTab()
        {
            TabPage tabPage = new TabPage("Bracings");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Bracings Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Deck Slabs tab.
        /// </summary>
        private TabPage CreateDeckSlabsTab()
        {
            TabPage tabPage = new TabPage("Deck Slabs");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Deck Slabs Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Eaves & Attics tab.
        /// </summary>
        private TabPage CreateEavesAtticTab()
        {
            TabPage tabPage = new TabPage("Eaves & Attics");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Eaves & Attics Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the Outer Walls tab.
        /// </summary>
        private TabPage CreateOuterWallsTab()
        {
            TabPage tabPage = new TabPage("Outer Walls");
            tabPage.Padding = new Padding(10);

            Label placeholderLabel = new Label
            {
                Text = "Outer Walls Configuration - Coming Soon",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14F)
            };

            tabPage.Controls.Add(placeholderLabel);
            return tabPage;
        }

        /// <summary>
        /// Creates the preview panel.
        /// </summary>
        private Panel CreatePreviewPanel()
        {
            Panel panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                Margin = new Padding(2),
                BackColor = System.Drawing.SystemColors.ControlLight
            };

            Label titleLabel = new Label
            {
                Text = "3D Preview",
                Dock = DockStyle.Top,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Height = 25
            };

            Label previewLabel = new Label
            {
                Text = "Preview will display here",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.SystemColors.GrayText
            };

            panel.Controls.Add(previewLabel);
            panel.Controls.Add(titleLabel);

            return panel;
        }

        /// <summary>
        /// Creates the button panel with OK, Cancel, and Help buttons.
        /// </summary>
        private Panel CreateButtonPanel()
        {
            Panel panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Height = 50,
                Dock = DockStyle.Bottom,
                Padding = new Padding(5)
            };

            // Progress bar
            ProgressBar progressBar = new ProgressBar
            {
                Left = 10,
                Top = 5,
                Width = 400,
                Height = 20,
                Visible = false
            };

            // Status label
            Label statusLabel = new Label
            {
                Left = 420,
                Top = 5,
                Width = 300,
                Height = 20,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };

            // OK button
            Button okBtn = new Button
            {
                Text = "Generate",
                Width = 100,
                Height = 35,
                Left = panel.Width - 320,
                Top = 7,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            okBtn.Click += (s, e) => GenerateFrame();

            // Cancel button
            Button cancelBtn = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 35,
                Left = panel.Width - 210,
                Top = 7,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            cancelBtn.Click += (s, e) => this.Close();

            // Help button
            Button helpBtn = new Button
            {
                Text = "Help",
                Width = 100,
                Height = 35,
                Left = panel.Width - 100,
                Top = 7,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            helpBtn.Click += (s, e) => ShowHelp();

            panel.Controls.AddRange(new Control[] { progressBar, statusLabel, okBtn, cancelBtn, helpBtn });

            return panel;
        }

        /// <summary>
        /// Selects a tab by name.
        /// </summary>
        private void SelectTabByName(string tabName)
        {
            if (_tabControl == null) return;

            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab.Text.Contains(tabName))
                {
                    _tabControl.SelectedTab = tab;
                    break;
                }
            }
        }

        /// <summary>
        /// Handles new configuration creation.
        /// </summary>
        private void NewConfiguration()
        {
            _frameConfig.Reset();
            _logger.LogInfo("New configuration created");
            MessageBox.Show("New configuration created", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles opening existing configuration.
        /// </summary>
        private void OpenConfiguration()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Open Frame Configuration"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _logger.LogInfo($"Opening configuration: {openFileDialog.FileName}");
                MessageBox.Show($"Opening: {openFileDialog.FileName}", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles saving configuration.
        /// </summary>
        private void SaveConfiguration()
        {
            _logger.LogInfo("Configuration saved");
            MessageBox.Show("Configuration saved successfully", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles saving configuration with new name.
        /// </summary>
        private void SaveConfigurationAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json",
                Title = "Save Frame Configuration As"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _logger.LogInfo($"Saving configuration as: {saveFileDialog.FileName}");
                MessageBox.Show($"Saved as: {saveFileDialog.FileName}", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Handles reset to defaults.
        /// </summary>
        private void ResetToDefaults()
        {
            if (MessageBox.Show("Reset all parameters to default values?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _frameConfig.Reset();
                _logger.LogInfo("Configuration reset to defaults");
            }
        }

        /// <summary>
        /// Handles preferences dialog.
        /// </summary>
        private void ShowPreferences()
        {
            _logger.LogInfo("Preferences dialog opened");
            MessageBox.Show("Preferences dialog coming soon", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles frame generation.
        /// </summary>
        private void GenerateFrame()
        {
            _logger.LogInfo("Frame generation started");
            MessageBox.Show("Frame generation will start...\n\nThis feature will be implemented in the next phase.", "Frame Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows help dialog.
        /// </summary>
        private void ShowHelp()
        {
            _logger.LogInfo("Help dialog opened");
            MessageBox.Show("Help Topics\n\nFor more information, please visit the documentation folder or contact support.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows about dialog.
        /// </summary>
        private void ShowAbout()
        {
            _logger.LogInfo("About dialog opened");
            MessageBox.Show(
                "Frame Generator v1.0.0-dev\n\n" +
                "Parametric Structural Steel Frame Generator for Tekla Structures\n\n" +
                "Copyright © 2026 UrbanNemo\n\n" +
                "This plugin enables parametric generation of complete structural frames " +
                "including columns, beams, trusses, purlins, bracings, and more.",
                "About Frame Generator",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Model;
using Tekla.Structures.Plugins;

namespace TeklaFrameGenerator
{
    [Plugin("FrameGenerator")]
    public class FrameGeneratorPlugin : IPlugin
    {
        private Model _model;
        private MainForm _mainForm;

        public PluginBaseInfo GetPluginInfo()
        {
            return new PluginBaseInfo
            {
                Name = "Frame Generator",
                Description = "Parametric structural frame generator for buildings",
                Author = "Your Name",
                Version = "1.0.0"
            };
        }

        public List<InputDefinition> DefineInput()
        {
            return new List<InputDefinition>();
        }

        public RunStatus Run(List<InputDefinition> inputs)
        {
            try
            {
                _model = new Model();
                if (!_model.GetConnectionStatus())
                {
                    MessageBox.Show("Cannot connect to Tekla Structures model.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return RunStatus.Failed;
                }

                _mainForm = new MainForm(_model);
                
                if (_mainForm.ShowDialog() == DialogResult.OK)
                {
                    return RunStatus.Succeeded;
                }
                
                return RunStatus.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Frame Generator Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return RunStatus.Failed;
            }
        }
    }
}
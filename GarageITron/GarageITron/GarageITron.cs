using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GarageModel;


namespace GarageITron
{
    public partial class GarageITron : Form
    {
        private GarageRepository _repo;
        private GarageMediator.GarageMediator _mediator;
        
        public GarageITron()
        {
            InitializeComponent();
            _repo = GarageRepository.Instance;
            _mediator = new GarageMediator.GarageMediator();
            _mediator.IDScanned += _mediator_IDScanned;
        }

        private void _mediator_IDScanned(object sender, GarageAssignment assignment, VehicleInformation information)
        {
            if (assignment == null || information == null)
            {
                _mediator.RequestClearID();
                return;
            }

            vehicleInformationUI.Items.Clear();
            vehicleInformationUI.Items.AddRange(new object[]
            {
                assignment, information
            });

            rescanUI.Enabled = true;
            processVehicleUI.Enabled = true;
            vehicleProcessStatusUI.Items.Add($"Scanned: {assignment.ID}");
        }

        private void UpdateGaragePopulation()
        {
            garagePopulationUI.Text = $"Garage Population: {_repo.GetGaragePopulation()} / 23";
        }

        private void UpdateSystemStatus()
        {
            systemStatusUI.Text = $"System Status: {_mediator.GetStatus()}";
        }

        private void startServersUI_Click(object sender, EventArgs e)
        {
            try
            {
                _mediator.Request();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            startServersUI.Enabled = false;
            killServersUI.Enabled = true;
            UpdateSystemStatus();
        }

        private void killServersUI_Click(object sender, EventArgs e)
        {
            try
            {
                _mediator.Kill();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            killServersUI.Enabled = false;
            processVehicleUI.Enabled = false;
            rescanUI.Enabled = false;
            startServersUI.Enabled = true;
            UpdateSystemStatus();
        }

        private void GarageITron_Load(object sender, EventArgs e)
        {
            UpdateGaragePopulation();
            UpdateSystemStatus();
        }

        private void rescanUI_Click(object sender, EventArgs e)
        {
            _mediator.RequestClearID();
        }
    }
}
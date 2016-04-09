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
        private GarageAssignment _scanned;

        public GarageITron()
        {
            InitializeComponent();
            _repo = GarageRepository.Instance;
            _mediator = new GarageMediator.GarageMediator();
            _mediator.IDScanned += _mediator_IDScanned;
            _mediator.VehicleInstructionsStarted += _mediator_VehicleInstructionsStarted;
            _mediator.VehicleProcessingStarted += _mediator_VehicleProcessingStarted;
            _mediator.VehicleProcessed += _mediator_VehicleProcessed;

        }

        private void _mediator_VehicleProcessed()
        {
            vehicleProcessStatusUI.Items.Add("Garage has finished processing vehicle...");
            UpdateGaragePopulation();
            UpdateSystemStatus();
        }

        private void _mediator_VehicleProcessingStarted()
        {
            vehicleProcessStatusUI.Items.Add("Garage has started processing vehicle...");
            killServersUI.Enabled = true;
        }
        
        private void _mediator_VehicleInstructionsStarted()
        {
            vehicleProcessStatusUI.Items.Add("Instructions being sent to Garage...");
            processVehicleUI.Enabled = false;
            rescanUI.Enabled = false;
            killServersUI.Enabled = false;
        }

        private void _mediator_IDScanned(object sender, GarageAssignment assignment, VehicleInformation information)
        {
            if (assignment == null || information == null)
            {
                _mediator.RequestClearID();
                return;
            }

            _scanned = assignment;
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

            _scanned = null;
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
            _scanned = null;
            _mediator.RequestClearID();
            UpdateSystemStatus();
        }

        private void processVehicleUI_Click(object sender, EventArgs e)
        {
            if (_scanned == null)
                return;
            _mediator.RequestProcessVehicle(_scanned);
            UpdateSystemStatus();
        }
    }
}
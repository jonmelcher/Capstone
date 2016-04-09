namespace GarageITron
{
    partial class GarageITron
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.garageMenuUI = new System.Windows.Forms.TabControl();
            this.garageMenu1 = new System.Windows.Forms.TabPage();
            this.killServersUI = new System.Windows.Forms.Button();
            this.startServersUI = new System.Windows.Forms.Button();
            this.vehicleProcessStatusUI = new System.Windows.Forms.ListBox();
            this.garagePopulationUI = new System.Windows.Forms.Label();
            this.systemStatusUI = new System.Windows.Forms.Label();
            this.processVehicleUI = new System.Windows.Forms.Button();
            this.garageMenu2 = new System.Windows.Forms.TabPage();
            this.vehicleInformationUI = new System.Windows.Forms.ListBox();
            this.rescanUI = new System.Windows.Forms.Button();
            this.garageMenuUI.SuspendLayout();
            this.garageMenu1.SuspendLayout();
            this.garageMenu2.SuspendLayout();
            this.SuspendLayout();
            // 
            // garageMenuUI
            // 
            this.garageMenuUI.Controls.Add(this.garageMenu1);
            this.garageMenuUI.Controls.Add(this.garageMenu2);
            this.garageMenuUI.Location = new System.Drawing.Point(12, 12);
            this.garageMenuUI.Name = "garageMenuUI";
            this.garageMenuUI.SelectedIndex = 0;
            this.garageMenuUI.Size = new System.Drawing.Size(260, 237);
            this.garageMenuUI.TabIndex = 0;
            // 
            // garageMenu1
            // 
            this.garageMenu1.Controls.Add(this.rescanUI);
            this.garageMenu1.Controls.Add(this.killServersUI);
            this.garageMenu1.Controls.Add(this.startServersUI);
            this.garageMenu1.Controls.Add(this.vehicleProcessStatusUI);
            this.garageMenu1.Controls.Add(this.garagePopulationUI);
            this.garageMenu1.Controls.Add(this.systemStatusUI);
            this.garageMenu1.Controls.Add(this.processVehicleUI);
            this.garageMenu1.Location = new System.Drawing.Point(4, 22);
            this.garageMenu1.Name = "garageMenu1";
            this.garageMenu1.Padding = new System.Windows.Forms.Padding(3);
            this.garageMenu1.Size = new System.Drawing.Size(252, 211);
            this.garageMenu1.TabIndex = 0;
            this.garageMenu1.Text = "Garage Controls";
            this.garageMenu1.UseVisualStyleBackColor = true;
            // 
            // killServersUI
            // 
            this.killServersUI.Enabled = false;
            this.killServersUI.Location = new System.Drawing.Point(130, 6);
            this.killServersUI.Name = "killServersUI";
            this.killServersUI.Size = new System.Drawing.Size(116, 23);
            this.killServersUI.TabIndex = 6;
            this.killServersUI.Text = "Kill Servers";
            this.killServersUI.UseVisualStyleBackColor = true;
            this.killServersUI.Click += new System.EventHandler(this.killServersUI_Click);
            // 
            // startServersUI
            // 
            this.startServersUI.Location = new System.Drawing.Point(6, 6);
            this.startServersUI.Name = "startServersUI";
            this.startServersUI.Size = new System.Drawing.Size(116, 23);
            this.startServersUI.TabIndex = 5;
            this.startServersUI.Text = "Start Servers";
            this.startServersUI.UseVisualStyleBackColor = true;
            this.startServersUI.Click += new System.EventHandler(this.startServersUI_Click);
            // 
            // vehicleProcessStatusUI
            // 
            this.vehicleProcessStatusUI.FormattingEnabled = true;
            this.vehicleProcessStatusUI.Location = new System.Drawing.Point(6, 96);
            this.vehicleProcessStatusUI.Name = "vehicleProcessStatusUI";
            this.vehicleProcessStatusUI.Size = new System.Drawing.Size(240, 108);
            this.vehicleProcessStatusUI.TabIndex = 4;
            // 
            // garagePopulationUI
            // 
            this.garagePopulationUI.AutoSize = true;
            this.garagePopulationUI.Location = new System.Drawing.Point(6, 45);
            this.garagePopulationUI.MaximumSize = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.MinimumSize = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.Name = "garagePopulationUI";
            this.garagePopulationUI.Size = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.TabIndex = 3;
            this.garagePopulationUI.Text = "Garage Population: _ / 23";
            this.garagePopulationUI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // systemStatusUI
            // 
            this.systemStatusUI.AutoSize = true;
            this.systemStatusUI.Location = new System.Drawing.Point(6, 32);
            this.systemStatusUI.MaximumSize = new System.Drawing.Size(240, 13);
            this.systemStatusUI.MinimumSize = new System.Drawing.Size(240, 13);
            this.systemStatusUI.Name = "systemStatusUI";
            this.systemStatusUI.Size = new System.Drawing.Size(240, 13);
            this.systemStatusUI.TabIndex = 2;
            this.systemStatusUI.Text = "System Status";
            this.systemStatusUI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processVehicleUI
            // 
            this.processVehicleUI.Enabled = false;
            this.processVehicleUI.Location = new System.Drawing.Point(6, 61);
            this.processVehicleUI.Name = "processVehicleUI";
            this.processVehicleUI.Size = new System.Drawing.Size(116, 23);
            this.processVehicleUI.TabIndex = 0;
            this.processVehicleUI.Text = "Process!";
            this.processVehicleUI.UseVisualStyleBackColor = true;
            // 
            // garageMenu2
            // 
            this.garageMenu2.Controls.Add(this.vehicleInformationUI);
            this.garageMenu2.Location = new System.Drawing.Point(4, 22);
            this.garageMenu2.Name = "garageMenu2";
            this.garageMenu2.Padding = new System.Windows.Forms.Padding(3);
            this.garageMenu2.Size = new System.Drawing.Size(252, 211);
            this.garageMenu2.TabIndex = 1;
            this.garageMenu2.Text = "Scanned Vehicle Information";
            this.garageMenu2.UseVisualStyleBackColor = true;
            // 
            // vehicleInformationUI
            // 
            this.vehicleInformationUI.FormattingEnabled = true;
            this.vehicleInformationUI.Location = new System.Drawing.Point(6, 14);
            this.vehicleInformationUI.Name = "vehicleInformationUI";
            this.vehicleInformationUI.Size = new System.Drawing.Size(240, 186);
            this.vehicleInformationUI.TabIndex = 0;
            // 
            // rescanUI
            // 
            this.rescanUI.Enabled = false;
            this.rescanUI.Location = new System.Drawing.Point(130, 61);
            this.rescanUI.Name = "rescanUI";
            this.rescanUI.Size = new System.Drawing.Size(116, 23);
            this.rescanUI.TabIndex = 7;
            this.rescanUI.Text = "Rescan!";
            this.rescanUI.UseVisualStyleBackColor = true;
            this.rescanUI.Click += new System.EventHandler(this.rescanUI_Click);
            // 
            // GarageITron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.garageMenuUI);
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "GarageITron";
            this.Text = "Garage-ITron v1.1";
            this.Load += new System.EventHandler(this.GarageITron_Load);
            this.garageMenuUI.ResumeLayout(false);
            this.garageMenu1.ResumeLayout(false);
            this.garageMenu1.PerformLayout();
            this.garageMenu2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl garageMenuUI;
        private System.Windows.Forms.TabPage garageMenu1;
        private System.Windows.Forms.ListBox vehicleProcessStatusUI;
        private System.Windows.Forms.Label garagePopulationUI;
        private System.Windows.Forms.Label systemStatusUI;
        private System.Windows.Forms.Button processVehicleUI;
        private System.Windows.Forms.TabPage garageMenu2;
        private System.Windows.Forms.ListBox vehicleInformationUI;
        private System.Windows.Forms.Button killServersUI;
        private System.Windows.Forms.Button startServersUI;
        private System.Windows.Forms.Button rescanUI;
    }
}


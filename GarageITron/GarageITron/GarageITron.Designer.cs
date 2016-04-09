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
            this.garageMenu2 = new System.Windows.Forms.TabPage();
            this.addCarUI = new System.Windows.Forms.Button();
            this.removeCarUI = new System.Windows.Forms.Button();
            this.systemStatusUI = new System.Windows.Forms.Label();
            this.garagePopulationUI = new System.Windows.Forms.Label();
            this.vehicleProcessStatusUI = new System.Windows.Forms.ListBox();
            this.vehicleInformationUI = new System.Windows.Forms.ListBox();
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
            this.garageMenu1.Controls.Add(this.vehicleProcessStatusUI);
            this.garageMenu1.Controls.Add(this.garagePopulationUI);
            this.garageMenu1.Controls.Add(this.systemStatusUI);
            this.garageMenu1.Controls.Add(this.removeCarUI);
            this.garageMenu1.Controls.Add(this.addCarUI);
            this.garageMenu1.Location = new System.Drawing.Point(4, 22);
            this.garageMenu1.Name = "garageMenu1";
            this.garageMenu1.Padding = new System.Windows.Forms.Padding(3);
            this.garageMenu1.Size = new System.Drawing.Size(252, 211);
            this.garageMenu1.TabIndex = 0;
            this.garageMenu1.Text = "Garage Controls";
            this.garageMenu1.UseVisualStyleBackColor = true;
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
            // addCarUI
            // 
            this.addCarUI.Enabled = false;
            this.addCarUI.Location = new System.Drawing.Point(6, 6);
            this.addCarUI.Name = "addCarUI";
            this.addCarUI.Size = new System.Drawing.Size(116, 23);
            this.addCarUI.TabIndex = 0;
            this.addCarUI.Text = "Enter Garage!";
            this.addCarUI.UseVisualStyleBackColor = true;
            // 
            // removeCarUI
            // 
            this.removeCarUI.Enabled = false;
            this.removeCarUI.Location = new System.Drawing.Point(130, 6);
            this.removeCarUI.Name = "removeCarUI";
            this.removeCarUI.Size = new System.Drawing.Size(116, 23);
            this.removeCarUI.TabIndex = 1;
            this.removeCarUI.Text = "Leave Garage!";
            this.removeCarUI.UseVisualStyleBackColor = true;
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
            // garagePopulationUI
            // 
            this.garagePopulationUI.AutoSize = true;
            this.garagePopulationUI.Location = new System.Drawing.Point(6, 45);
            this.garagePopulationUI.MaximumSize = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.MinimumSize = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.Name = "garagePopulationUI";
            this.garagePopulationUI.Size = new System.Drawing.Size(240, 13);
            this.garagePopulationUI.TabIndex = 3;
            this.garagePopulationUI.Text = "Garage Population: 0 / 23";
            this.garagePopulationUI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vehicleProcessStatusUI
            // 
            this.vehicleProcessStatusUI.FormattingEnabled = true;
            this.vehicleProcessStatusUI.Location = new System.Drawing.Point(9, 70);
            this.vehicleProcessStatusUI.Name = "vehicleProcessStatusUI";
            this.vehicleProcessStatusUI.Size = new System.Drawing.Size(237, 134);
            this.vehicleProcessStatusUI.TabIndex = 4;
            // 
            // vehicleInformationUI
            // 
            this.vehicleInformationUI.FormattingEnabled = true;
            this.vehicleInformationUI.Location = new System.Drawing.Point(6, 14);
            this.vehicleInformationUI.Name = "vehicleInformationUI";
            this.vehicleInformationUI.Size = new System.Drawing.Size(240, 186);
            this.vehicleInformationUI.TabIndex = 0;
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
        private System.Windows.Forms.Button removeCarUI;
        private System.Windows.Forms.Button addCarUI;
        private System.Windows.Forms.TabPage garageMenu2;
        private System.Windows.Forms.ListBox vehicleInformationUI;
    }
}


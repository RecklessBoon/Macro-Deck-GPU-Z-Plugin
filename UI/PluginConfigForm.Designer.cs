namespace RecklessBoon.MacroDeck.GPUZ.UI
{
    partial class PluginConfigForm
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
            this.components = new System.ComponentModel.Container();
            this.pollingFrequencyErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.pollingFrequency = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_OK = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.lblPollingFrequency = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pollingFrequencyErrorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pollingFrequency)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pollingFrequencyErrorProvider
            // 
            this.pollingFrequencyErrorProvider.ContainerControl = this;
            // 
            // pollingFrequency
            // 
            this.pollingFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pollingFrequency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.pollingFrequency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pollingFrequency.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pollingFrequency.ForeColor = System.Drawing.Color.White;
            this.pollingFrequencyErrorProvider.SetIconAlignment(this.pollingFrequency, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.pollingFrequency.Location = new System.Drawing.Point(143, 10);
            this.pollingFrequency.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.pollingFrequency.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.pollingFrequency.Name = "pollingFrequency";
            this.pollingFrequency.Size = new System.Drawing.Size(110, 22);
            this.pollingFrequency.TabIndex = 16;
            this.pollingFrequency.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.pollingFrequency.Validating += new System.ComponentModel.CancelEventHandler(this.PollingFrequency_Validating);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Controls.Add(this.pollingFrequency);
            this.panel1.Controls.Add(this.lblPollingFrequency);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 68);
            this.panel1.TabIndex = 2;
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.BorderRadius = 8;
            this.btn_OK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_OK.FlatAppearance.BorderSize = 0;
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.HoverColor = System.Drawing.Color.Empty;
            this.btn_OK.Icon = null;
            this.btn_OK.Location = new System.Drawing.Point(183, 38);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Progress = 0;
            this.btn_OK.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(225)))));
            this.btn_OK.Size = new System.Drawing.Size(70, 25);
            this.btn_OK.TabIndex = 17;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.UseWindowsAccentColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // lblPollingFrequency
            // 
            this.lblPollingFrequency.AutoSize = true;
            this.lblPollingFrequency.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPollingFrequency.Location = new System.Drawing.Point(3, 10);
            this.lblPollingFrequency.Name = "lblPollingFrequency";
            this.lblPollingFrequency.Size = new System.Drawing.Size(124, 18);
            this.lblPollingFrequency.TabIndex = 14;
            this.lblPollingFrequency.Text = "Polling Frequency:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "GPU-Z Plugin Configuration";
            // 
            // PluginConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 104);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "PluginConfigForm";
            this.Padding = new System.Windows.Forms.Padding(6, 30, 6, 6);
            this.Text = "PluginConfigForm";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pollingFrequencyErrorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pollingFrequency)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider pollingFrequencyErrorProvider;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown pollingFrequency;
        private System.Windows.Forms.Label lblPollingFrequency;
        private System.Windows.Forms.Label label1;
        private SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary btn_OK;
    }
}
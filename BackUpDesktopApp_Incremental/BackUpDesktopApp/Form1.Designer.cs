namespace BackUpDesktopApp
{
    partial class Form1
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
            this.lblDBRestorePath = new System.Windows.Forms.Label();
            this.txtdbname = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lstBoxLog = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDEstination = new System.Windows.Forms.TextBox();
            this.lblDBbckupName = new System.Windows.Forms.Label();
            this.txtBckfileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.rdBackUp = new System.Windows.Forms.RadioButton();
            this.rdRestore = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBDEstination = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDBRestorePath
            // 
            this.lblDBRestorePath.AutoSize = true;
            this.lblDBRestorePath.Location = new System.Drawing.Point(77, 145);
            this.lblDBRestorePath.Name = "lblDBRestorePath";
            this.lblDBRestorePath.Size = new System.Drawing.Size(112, 13);
            this.lblDBRestorePath.TabIndex = 0;
            this.lblDBRestorePath.Text = "Enter Database Name";
            // 
            // txtdbname
            // 
            this.txtdbname.Location = new System.Drawing.Point(240, 138);
            this.txtdbname.Name = "txtdbname";
            this.txtdbname.Size = new System.Drawing.Size(191, 20);
            this.txtdbname.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(240, 209);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(155, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Start BackUp";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lstBoxLog
            // 
            this.lstBoxLog.FormattingEnabled = true;
            this.lstBoxLog.Location = new System.Drawing.Point(223, 252);
            this.lstBoxLog.Name = "lstBoxLog";
            this.lstBoxLog.Size = new System.Drawing.Size(410, 186);
            this.lstBoxLog.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Server Host Name";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(240, 55);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(191, 20);
            this.txtServer.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(240, 81);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(191, 20);
            this.txtUsername.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(240, 110);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(191, 20);
            this.txtPassword.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(459, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Destination App Path";
            // 
            // txtDEstination
            // 
            this.txtDEstination.Location = new System.Drawing.Point(597, 138);
            this.txtDEstination.Name = "txtDEstination";
            this.txtDEstination.Size = new System.Drawing.Size(191, 20);
            this.txtDEstination.TabIndex = 13;
            // 
            // lblDBbckupName
            // 
            this.lblDBbckupName.AutoSize = true;
            this.lblDBbckupName.Location = new System.Drawing.Point(80, 178);
            this.lblDBbckupName.Name = "lblDBbckupName";
            this.lblDBbckupName.Size = new System.Drawing.Size(114, 13);
            this.lblDBbckupName.TabIndex = 14;
            this.lblDBbckupName.Text = "DB BackUp File Name";
            // 
            // txtBckfileName
            // 
            this.txtBckfileName.Location = new System.Drawing.Point(240, 171);
            this.txtBckfileName.Name = "txtBckfileName";
            this.txtBckfileName.Size = new System.Drawing.Size(191, 20);
            this.txtBckfileName.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(459, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Source Application Path";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(597, 110);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(191, 20);
            this.txtSourcePath.TabIndex = 17;
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Location = new System.Drawing.Point(462, 209);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(100, 23);
            this.btnSettingsSave.TabIndex = 19;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // rdBackUp
            // 
            this.rdBackUp.AutoSize = true;
            this.rdBackUp.Checked = true;
            this.rdBackUp.Location = new System.Drawing.Point(80, 12);
            this.rdBackUp.Name = "rdBackUp";
            this.rdBackUp.Size = new System.Drawing.Size(64, 17);
            this.rdBackUp.TabIndex = 21;
            this.rdBackUp.TabStop = true;
            this.rdBackUp.Text = "BackUp";
            this.rdBackUp.UseVisualStyleBackColor = true;
            this.rdBackUp.CheckedChanged += new System.EventHandler(this.rdBackUp_CheckedChanged);
            // 
            // rdRestore
            // 
            this.rdRestore.AutoSize = true;
            this.rdRestore.Enabled = false;
            this.rdRestore.Location = new System.Drawing.Point(214, 12);
            this.rdRestore.Name = "rdRestore";
            this.rdRestore.Size = new System.Drawing.Size(62, 17);
            this.rdRestore.TabIndex = 22;
            this.rdRestore.Text = "Restore";
            this.rdRestore.UseVisualStyleBackColor = true;
            this.rdRestore.CheckedChanged += new System.EventHandler(this.rdRestore_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(459, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Destination DB Path";
            // 
            // txtDBDEstination
            // 
            this.txtDBDEstination.Location = new System.Drawing.Point(597, 171);
            this.txtDBDEstination.Name = "txtDBDEstination";
            this.txtDBDEstination.Size = new System.Drawing.Size(191, 20);
            this.txtDBDEstination.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtDBDEstination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdRestore);
            this.Controls.Add(this.rdBackUp);
            this.Controls.Add(this.btnSettingsSave);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtBckfileName);
            this.Controls.Add(this.lblDBbckupName);
            this.Controls.Add(this.txtDEstination);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstBoxLog);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtdbname);
            this.Controls.Add(this.lblDBRestorePath);
            this.Name = "Form1";
            this.Text = "BackUp Application Incremental";
            this.Shown += new System.EventHandler(this.Form1_Shown_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDBRestorePath;
        private System.Windows.Forms.TextBox txtdbname;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ListBox lstBoxLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDEstination;
        private System.Windows.Forms.Label lblDBbckupName;
        private System.Windows.Forms.TextBox txtBckfileName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnSettingsSave;
        private System.Windows.Forms.RadioButton rdBackUp;
        private System.Windows.Forms.RadioButton rdRestore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBDEstination;
    }
}


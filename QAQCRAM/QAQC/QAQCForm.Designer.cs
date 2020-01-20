namespace QAQCRAM
{
    partial class QAQCForm
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
            this.RAMStoryText = new System.Windows.Forms.TextBox();
            this.FilenameText = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.ElementIDText = new System.Windows.Forms.TextBox();
            this.RAMIDText = new System.Windows.Forms.TextBox();
            this.FilenamePanel = new System.Windows.Forms.Panel();
            this.BeamNumberPanel = new System.Windows.Forms.Panel();
            this.StoryNamePanel = new System.Windows.Forms.Panel();
            this.ElementIDPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OFDButton = new System.Windows.Forms.Button();
            this.SelectElementButton = new System.Windows.Forms.Button();
            this.RotText = new System.Windows.Forms.TextBox();
            this.RotationPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ToleranceText = new System.Windows.Forms.TextBox();
            this.TolerancePanel = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ClearToggle = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.JoistToggle = new System.Windows.Forms.CheckBox();
            this.BeamToggle = new System.Windows.Forms.CheckBox();
            this.ColumnToggle = new System.Windows.Forms.CheckBox();
            this.VBToggle = new System.Windows.Forms.CheckBox();
            this.SelectionSetCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ElementIDPanel.SuspendLayout();
            this.RotationPanel.SuspendLayout();
            this.TolerancePanel.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // RAMStoryText
            // 
            this.RAMStoryText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.RAMStoryText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RAMStoryText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RAMStoryText.ForeColor = System.Drawing.SystemColors.Window;
            this.RAMStoryText.Location = new System.Drawing.Point(72, 196);
            this.RAMStoryText.Name = "RAMStoryText";
            this.RAMStoryText.Size = new System.Drawing.Size(213, 18);
            this.RAMStoryText.TabIndex = 3;
            this.RAMStoryText.Tag = "RAM Story Name";
            this.RAMStoryText.Text = "RAM Story Name";
            this.RAMStoryText.Click += new System.EventHandler(this.RAMStoryText_Click);
            this.RAMStoryText.TextChanged += new System.EventHandler(this.RAMStoryText_TextChanged);
            // 
            // FilenameText
            // 
            this.FilenameText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.FilenameText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FilenameText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilenameText.ForeColor = System.Drawing.SystemColors.Window;
            this.FilenameText.Location = new System.Drawing.Point(72, 105);
            this.FilenameText.Name = "FilenameText";
            this.FilenameText.ReadOnly = true;
            this.FilenameText.Size = new System.Drawing.Size(213, 18);
            this.FilenameText.TabIndex = 5;
            this.FilenameText.Text = "Filename";
            this.FilenameText.TextChanged += new System.EventHandler(this.FilenameText_TextChanged);
            // 
            // RunButton
            // 
            this.RunButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.RunButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RunButton.FlatAppearance.BorderSize = 0;
            this.RunButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RunButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunButton.ForeColor = System.Drawing.Color.White;
            this.RunButton.Location = new System.Drawing.Point(35, 593);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(250, 35);
            this.RunButton.TabIndex = 6;
            this.RunButton.Text = "COMPARE MODELS";
            this.RunButton.UseVisualStyleBackColor = false;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // ElementIDText
            // 
            this.ElementIDText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.ElementIDText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ElementIDText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElementIDText.ForeColor = System.Drawing.SystemColors.Window;
            this.ElementIDText.Location = new System.Drawing.Point(72, 327);
            this.ElementIDText.Name = "ElementIDText";
            this.ElementIDText.ReadOnly = true;
            this.ElementIDText.Size = new System.Drawing.Size(213, 18);
            this.ElementIDText.TabIndex = 2;
            this.ElementIDText.Text = "Element ID";
            this.ElementIDText.TextChanged += new System.EventHandler(this.ElementIDText_TextChanged);
            // 
            // RAMIDText
            // 
            this.RAMIDText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.RAMIDText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RAMIDText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RAMIDText.ForeColor = System.Drawing.Color.White;
            this.RAMIDText.Location = new System.Drawing.Point(72, 150);
            this.RAMIDText.Name = "RAMIDText";
            this.RAMIDText.Size = new System.Drawing.Size(213, 18);
            this.RAMIDText.TabIndex = 4;
            this.RAMIDText.Tag = "RAM Beam Number";
            this.RAMIDText.Text = "RAM Beam Number";
            this.RAMIDText.Click += new System.EventHandler(this.RAMIDText_Click);
            this.RAMIDText.TextChanged += new System.EventHandler(this.RAMIDText_TextChanged);
            this.RAMIDText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Integer_KeyPress);
            // 
            // FilenamePanel
            // 
            this.FilenamePanel.BackColor = System.Drawing.Color.White;
            this.FilenamePanel.Location = new System.Drawing.Point(35, 126);
            this.FilenamePanel.Name = "FilenamePanel";
            this.FilenamePanel.Size = new System.Drawing.Size(250, 1);
            this.FilenamePanel.TabIndex = 8;
            // 
            // BeamNumberPanel
            // 
            this.BeamNumberPanel.BackColor = System.Drawing.Color.White;
            this.BeamNumberPanel.Location = new System.Drawing.Point(35, 170);
            this.BeamNumberPanel.Name = "BeamNumberPanel";
            this.BeamNumberPanel.Size = new System.Drawing.Size(250, 1);
            this.BeamNumberPanel.TabIndex = 9;
            // 
            // StoryNamePanel
            // 
            this.StoryNamePanel.BackColor = System.Drawing.Color.White;
            this.StoryNamePanel.Location = new System.Drawing.Point(35, 215);
            this.StoryNamePanel.Name = "StoryNamePanel";
            this.StoryNamePanel.Size = new System.Drawing.Size(250, 1);
            this.StoryNamePanel.TabIndex = 10;
            // 
            // ElementIDPanel
            // 
            this.ElementIDPanel.BackColor = System.Drawing.Color.White;
            this.ElementIDPanel.Controls.Add(this.panel1);
            this.ElementIDPanel.Location = new System.Drawing.Point(35, 349);
            this.ElementIDPanel.Name = "ElementIDPanel";
            this.ElementIDPanel.Size = new System.Drawing.Size(250, 1);
            this.ElementIDPanel.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 1);
            this.panel1.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(87, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 52);
            this.label2.TabIndex = 13;
            this.label2.Text = "QA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(172, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 52);
            this.label3.TabIndex = 14;
            this.label3.Text = "QC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(149, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 52);
            this.label4.TabIndex = 15;
            this.label4.Text = "|";
            // 
            // CloseButton
            // 
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.SystemColors.Window;
            this.CloseButton.Location = new System.Drawing.Point(305, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(23, 25);
            this.CloseButton.TabIndex = 17;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OFDButton
            // 
            this.OFDButton.BackgroundImage = global::QAQCRAM.Properties.Resources.folderResized;
            this.OFDButton.FlatAppearance.BorderSize = 0;
            this.OFDButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OFDButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OFDButton.Location = new System.Drawing.Point(38, 106);
            this.OFDButton.Name = "OFDButton";
            this.OFDButton.Size = new System.Drawing.Size(20, 17);
            this.OFDButton.TabIndex = 1;
            this.OFDButton.UseVisualStyleBackColor = true;
            this.OFDButton.Click += new System.EventHandler(this.OFDButton_Click);
            // 
            // SelectElementButton
            // 
            this.SelectElementButton.BackgroundImage = global::QAQCRAM.Properties.Resources.beamResized;
            this.SelectElementButton.FlatAppearance.BorderSize = 0;
            this.SelectElementButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectElementButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SelectElementButton.Location = new System.Drawing.Point(39, 328);
            this.SelectElementButton.Name = "SelectElementButton";
            this.SelectElementButton.Size = new System.Drawing.Size(19, 19);
            this.SelectElementButton.TabIndex = 1;
            this.SelectElementButton.UseVisualStyleBackColor = true;
            this.SelectElementButton.Click += new System.EventHandler(this.SelectElementButton_Click);
            // 
            // RotText
            // 
            this.RotText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.RotText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RotText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RotText.ForeColor = System.Drawing.SystemColors.Window;
            this.RotText.Location = new System.Drawing.Point(72, 238);
            this.RotText.Name = "RotText";
            this.RotText.Size = new System.Drawing.Size(213, 18);
            this.RotText.TabIndex = 18;
            this.RotText.Tag = "Rotation Input (deg.)";
            this.RotText.Text = "Rotation Input (deg.)";
            this.RotText.Click += new System.EventHandler(this.RotText_Click);
            this.RotText.TextChanged += new System.EventHandler(this.RotText_TextChanged);
            // 
            // RotationPanel
            // 
            this.RotationPanel.BackColor = System.Drawing.Color.White;
            this.RotationPanel.Controls.Add(this.panel2);
            this.RotationPanel.Location = new System.Drawing.Point(38, 258);
            this.RotationPanel.Name = "RotationPanel";
            this.RotationPanel.Size = new System.Drawing.Size(250, 1);
            this.RotationPanel.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(250, 1);
            this.panel2.TabIndex = 20;
            // 
            // ToleranceText
            // 
            this.ToleranceText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.ToleranceText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ToleranceText.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToleranceText.ForeColor = System.Drawing.SystemColors.Window;
            this.ToleranceText.Location = new System.Drawing.Point(72, 283);
            this.ToleranceText.Name = "ToleranceText";
            this.ToleranceText.Size = new System.Drawing.Size(213, 18);
            this.ToleranceText.TabIndex = 20;
            this.ToleranceText.Tag = "Tolerance Input (ft.)";
            this.ToleranceText.Text = "Tolerance Input (ft.)";
            this.ToleranceText.Click += new System.EventHandler(this.ToleranceText_Click);
            this.ToleranceText.TextChanged += new System.EventHandler(this.ToleranceText_TextChanged);
            this.ToleranceText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Integer_KeyPress);
            // 
            // TolerancePanel
            // 
            this.TolerancePanel.BackColor = System.Drawing.Color.White;
            this.TolerancePanel.Controls.Add(this.panel4);
            this.TolerancePanel.Location = new System.Drawing.Point(40, 303);
            this.TolerancePanel.Name = "TolerancePanel";
            this.TolerancePanel.Size = new System.Drawing.Size(250, 1);
            this.TolerancePanel.TabIndex = 21;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(0, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(250, 1);
            this.panel4.TabIndex = 20;
            // 
            // ClearToggle
            // 
            this.ClearToggle.AutoSize = true;
            this.ClearToggle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.ClearToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.ClearToggle.Location = new System.Drawing.Point(40, 372);
            this.ClearToggle.Name = "ClearToggle";
            this.ClearToggle.Size = new System.Drawing.Size(201, 21);
            this.ClearToggle.TabIndex = 22;
            this.ClearToggle.Text = "    Clear Previous Results?";
            this.ClearToggle.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Location = new System.Drawing.Point(40, 395);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(250, 1);
            this.panel5.TabIndex = 23;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Location = new System.Drawing.Point(0, 11);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(250, 1);
            this.panel6.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Location = new System.Drawing.Point(40, 511);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(250, 1);
            this.panel3.TabIndex = 24;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.Location = new System.Drawing.Point(0, 11);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(250, 1);
            this.panel7.TabIndex = 13;
            // 
            // JoistToggle
            // 
            this.JoistToggle.AutoSize = true;
            this.JoistToggle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.JoistToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.JoistToggle.Location = new System.Drawing.Point(41, 404);
            this.JoistToggle.Name = "JoistToggle";
            this.JoistToggle.Size = new System.Drawing.Size(179, 21);
            this.JoistToggle.TabIndex = 25;
            this.JoistToggle.Text = "    Consider RAM Joists";
            this.JoistToggle.UseVisualStyleBackColor = true;
            // 
            // BeamToggle
            // 
            this.BeamToggle.AutoSize = true;
            this.BeamToggle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.BeamToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.BeamToggle.Location = new System.Drawing.Point(41, 430);
            this.BeamToggle.Name = "BeamToggle";
            this.BeamToggle.Size = new System.Drawing.Size(188, 21);
            this.BeamToggle.TabIndex = 26;
            this.BeamToggle.Text = "    Consider RAM Beams";
            this.BeamToggle.UseVisualStyleBackColor = true;
            // 
            // ColumnToggle
            // 
            this.ColumnToggle.AutoSize = true;
            this.ColumnToggle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.ColumnToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.ColumnToggle.Location = new System.Drawing.Point(41, 457);
            this.ColumnToggle.Name = "ColumnToggle";
            this.ColumnToggle.Size = new System.Drawing.Size(200, 21);
            this.ColumnToggle.TabIndex = 27;
            this.ColumnToggle.Text = "    Consider RAM Columns";
            this.ColumnToggle.UseVisualStyleBackColor = true;
            // 
            // VBToggle
            // 
            this.VBToggle.AutoSize = true;
            this.VBToggle.Font = new System.Drawing.Font("Arial", 11.25F);
            this.VBToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.VBToggle.Location = new System.Drawing.Point(41, 484);
            this.VBToggle.Name = "VBToggle";
            this.VBToggle.Size = new System.Drawing.Size(239, 21);
            this.VBToggle.TabIndex = 28;
            this.VBToggle.Text = "    Consider RAM Vertical Braces";
            this.VBToggle.UseVisualStyleBackColor = true;
            // 
            // SelectionSetCombo
            // 
            this.SelectionSetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectionSetCombo.Font = new System.Drawing.Font("Arial", 11.25F);
            this.SelectionSetCombo.FormattingEnabled = true;
            this.SelectionSetCombo.Location = new System.Drawing.Point(41, 553);
            this.SelectionSetCombo.Name = "SelectionSetCombo";
            this.SelectionSetCombo.Size = new System.Drawing.Size(247, 25);
            this.SelectionSetCombo.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(78, 524);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 17);
            this.label1.TabIndex = 30;
            this.label1.Text = "Exclude Selection Set?";
            // 
            // QAQCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(331, 650);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectionSetCombo);
            this.Controls.Add(this.VBToggle);
            this.Controls.Add(this.ColumnToggle);
            this.Controls.Add(this.BeamToggle);
            this.Controls.Add(this.JoistToggle);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.ClearToggle);
            this.Controls.Add(this.TolerancePanel);
            this.Controls.Add(this.ToleranceText);
            this.Controls.Add(this.RotationPanel);
            this.Controls.Add(this.RotText);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OFDButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ElementIDPanel);
            this.Controls.Add(this.StoryNamePanel);
            this.Controls.Add(this.BeamNumberPanel);
            this.Controls.Add(this.FilenamePanel);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.FilenameText);
            this.Controls.Add(this.RAMIDText);
            this.Controls.Add(this.RAMStoryText);
            this.Controls.Add(this.ElementIDText);
            this.Controls.Add(this.SelectElementButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QAQCForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QAQCForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QAQCForm_Load);
            this.Click += new System.EventHandler(this.QAQCForm_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QAQCForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QAQCForm_MouseMove);
            this.ElementIDPanel.ResumeLayout(false);
            this.RotationPanel.ResumeLayout(false);
            this.TolerancePanel.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button RunButton;
        public System.Windows.Forms.TextBox ElementIDText;
        private System.Windows.Forms.Panel FilenamePanel;
        private System.Windows.Forms.Panel BeamNumberPanel;
        private System.Windows.Forms.Panel StoryNamePanel;
        private System.Windows.Forms.Panel ElementIDPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OFDButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button SelectElementButton;
        public System.Windows.Forms.TextBox FilenameText;
        public System.Windows.Forms.TextBox RAMStoryText;
        public System.Windows.Forms.TextBox RAMIDText;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox RotText;
        private System.Windows.Forms.Panel RotationPanel;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TextBox ToleranceText;
        private System.Windows.Forms.Panel TolerancePanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.CheckBox ClearToggle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        public System.Windows.Forms.CheckBox JoistToggle;
        public System.Windows.Forms.CheckBox BeamToggle;
        public System.Windows.Forms.CheckBox ColumnToggle;
        public System.Windows.Forms.CheckBox VBToggle;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox SelectionSetCombo;
    }
}
namespace QAQCRAM
{
    partial class QAQCEdit
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.LVDataList = new System.Windows.Forms.ListView();
            this.MemberTypeLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ElementIDLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionSizeLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConcernLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RevitParamLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RAMParamLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RAMStoryLV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UpdateButton = new System.Windows.Forms.Button();
            this.IgnoreButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SearchBar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(338, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 52);
            this.label4.TabIndex = 18;
            this.label4.Text = "|";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(363, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 52);
            this.label3.TabIndex = 17;
            this.label3.Text = "QC";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(271, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 52);
            this.label2.TabIndex = 16;
            this.label2.Text = "QA";
            // 
            // CloseButton
            // 
            this.CloseButton.FlatAppearance.BorderSize = 0;
            this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.SystemColors.Window;
            this.CloseButton.Location = new System.Drawing.Point(640, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(23, 25);
            this.CloseButton.TabIndex = 19;
            this.CloseButton.Text = "X";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LVDataList
            // 
            this.LVDataList.AllowDrop = true;
            this.LVDataList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.LVDataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LVDataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MemberTypeLV,
            this.ElementIDLV,
            this.SectionSizeLV,
            this.ConcernLV,
            this.RevitParamLV,
            this.RAMParamLV,
            this.RAMStoryLV});
            this.LVDataList.ForeColor = System.Drawing.SystemColors.Window;
            this.LVDataList.FullRowSelect = true;
            this.LVDataList.GridLines = true;
            this.LVDataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LVDataList.HideSelection = false;
            this.LVDataList.Location = new System.Drawing.Point(5, 83);
            this.LVDataList.Name = "LVDataList";
            this.LVDataList.OwnerDraw = true;
            this.LVDataList.Size = new System.Drawing.Size(658, 381);
            this.LVDataList.TabIndex = 21;
            this.LVDataList.UseCompatibleStateImageBehavior = false;
            this.LVDataList.View = System.Windows.Forms.View.Details;
            this.LVDataList.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.LVDataList_ColumnWidthChanging);
            this.LVDataList.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.LVDataList_DrawColumnHeader);
            this.LVDataList.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.LVDataList_DrawItem);
            this.LVDataList.Click += new System.EventHandler(this.HighlightRow_Click);
            // 
            // MemberTypeLV
            // 
            this.MemberTypeLV.Text = "   Member_Type";
            this.MemberTypeLV.Width = 101;
            // 
            // ElementIDLV
            // 
            this.ElementIDLV.Text = "   Element_ID";
            this.ElementIDLV.Width = 92;
            // 
            // SectionSizeLV
            // 
            this.SectionSizeLV.Text = "    Section_Size";
            this.SectionSizeLV.Width = 108;
            // 
            // ConcernLV
            // 
            this.ConcernLV.Text = "  Concern";
            this.ConcernLV.Width = 65;
            // 
            // RevitParamLV
            // 
            this.RevitParamLV.Text = "     Revit_Value";
            this.RevitParamLV.Width = 108;
            // 
            // RAMParamLV
            // 
            this.RAMParamLV.Text = "     RAM_Value";
            this.RAMParamLV.Width = 106;
            // 
            // RAMStoryLV
            // 
            this.RAMStoryLV.Text = "RAM_Story";
            this.RAMStoryLV.Width = 72;
            // 
            // UpdateButton
            // 
            this.UpdateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.UpdateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UpdateButton.FlatAppearance.BorderSize = 0;
            this.UpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateButton.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateButton.ForeColor = System.Drawing.Color.White;
            this.UpdateButton.Location = new System.Drawing.Point(144, 588);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(115, 39);
            this.UpdateButton.TabIndex = 22;
            this.UpdateButton.Text = "UPDATE";
            this.UpdateButton.UseVisualStyleBackColor = false;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // IgnoreButton
            // 
            this.IgnoreButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.IgnoreButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.IgnoreButton.FlatAppearance.BorderSize = 2;
            this.IgnoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IgnoreButton.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IgnoreButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.IgnoreButton.Location = new System.Drawing.Point(415, 588);
            this.IgnoreButton.Name = "IgnoreButton";
            this.IgnoreButton.Size = new System.Drawing.Size(115, 39);
            this.IgnoreButton.TabIndex = 23;
            this.IgnoreButton.Text = "IGNORE";
            this.IgnoreButton.UseVisualStyleBackColor = false;
            this.IgnoreButton.Click += new System.EventHandler(this.IgnoreButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 536);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(645, 23);
            this.progressBar1.TabIndex = 24;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(280, 588);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 39);
            this.button1.TabIndex = 25;
            this.button1.Text = "ISOLATE";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Isolate_Click);
            // 
            // SearchBar
            // 
            this.SearchBar.Location = new System.Drawing.Point(72, 488);
            this.SearchBar.Name = "SearchBar";
            this.SearchBar.Size = new System.Drawing.Size(148, 20);
            this.SearchBar.TabIndex = 26;
            this.SearchBar.TextChanged += new System.EventHandler(this.SearchBar_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(12, 491);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "SEARCH:";
            // 
            // QAQCEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(669, 649);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchBar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.IgnoreButton);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.LVDataList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QAQCEdit";
            this.Text = "QAQCEdit";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QAQCEdit_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.QAQCEdit_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.QAQCEdit_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ColumnHeader MemberTypeLV;
        private System.Windows.Forms.ColumnHeader ElementIDLV;
        private System.Windows.Forms.ColumnHeader SectionSizeLV;
        private System.Windows.Forms.ColumnHeader ConcernLV;
        private System.Windows.Forms.ColumnHeader RevitParamLV;
        private System.Windows.Forms.ColumnHeader RAMParamLV;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button IgnoreButton;
        public System.Windows.Forms.ListView LVDataList;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ColumnHeader RAMStoryLV;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SearchBar;
        private System.Windows.Forms.Label label1;
    }
}
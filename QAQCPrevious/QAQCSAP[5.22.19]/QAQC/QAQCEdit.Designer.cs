namespace QAQCSAP
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
            this.UpdateButton = new System.Windows.Forms.Button();
            this.IgnoreButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(277, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 52);
            this.label4.TabIndex = 18;
            this.label4.Text = "|";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(302, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 52);
            this.label3.TabIndex = 17;
            this.label3.Text = "QC";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(210, 14);
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
            this.CloseButton.Location = new System.Drawing.Point(580, 3);
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
            this.LVDataList.BackColor = System.Drawing.Color.Gold;
            this.LVDataList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LVDataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MemberTypeLV,
            this.ElementIDLV,
            this.SectionSizeLV,
            this.ConcernLV,
            this.RevitParamLV,
            this.RAMParamLV});
            this.LVDataList.ForeColor = System.Drawing.Color.Black;
            this.LVDataList.FullRowSelect = true;
            this.LVDataList.GridLines = true;
            this.LVDataList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LVDataList.Location = new System.Drawing.Point(5, 83);
            this.LVDataList.Name = "LVDataList";
            this.LVDataList.OwnerDraw = true;
            this.LVDataList.Size = new System.Drawing.Size(588, 381);
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
            this.RAMParamLV.Text = "     SAP_Value";
            this.RAMParamLV.Width = 106;
            // 
            // UpdateButton
            // 
            this.UpdateButton.BackColor = System.Drawing.Color.Red;
            this.UpdateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UpdateButton.FlatAppearance.BorderSize = 0;
            this.UpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateButton.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateButton.ForeColor = System.Drawing.Color.White;
            this.UpdateButton.Location = new System.Drawing.Point(127, 490);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(115, 39);
            this.UpdateButton.TabIndex = 22;
            this.UpdateButton.Text = "UPDATE";
            this.UpdateButton.UseVisualStyleBackColor = false;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // IgnoreButton
            // 
            this.IgnoreButton.BackColor = System.Drawing.Color.Gold;
            this.IgnoreButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.IgnoreButton.FlatAppearance.BorderSize = 2;
            this.IgnoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IgnoreButton.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IgnoreButton.ForeColor = System.Drawing.Color.Red;
            this.IgnoreButton.Location = new System.Drawing.Point(322, 490);
            this.IgnoreButton.Name = "IgnoreButton";
            this.IgnoreButton.Size = new System.Drawing.Size(115, 39);
            this.IgnoreButton.TabIndex = 23;
            this.IgnoreButton.Text = "IGNORE";
            this.IgnoreButton.UseVisualStyleBackColor = false;
            this.IgnoreButton.Click += new System.EventHandler(this.IgnoreButton_Click);
            // 
            // QAQCEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(605, 545);
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
    }
}
namespace SNESOT_Sync
{
    partial class Rom
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedRom = new System.Windows.Forms.CheckBox();
            this.pic = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedRom
            // 
            this.checkedRom.AutoSize = true;
            this.checkedRom.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedRom.Location = new System.Drawing.Point(3, 4);
            this.checkedRom.Name = "checkedRom";
            this.checkedRom.Size = new System.Drawing.Size(15, 14);
            this.checkedRom.TabIndex = 0;
            this.checkedRom.UseVisualStyleBackColor = true;
            // 
            // pic
            // 
            this.pic.Image = global::SNESOT_Sync.Properties.Resources.no_download;
            this.pic.Location = new System.Drawing.Point(3, 21);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(566, 79);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 1;
            this.pic.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Quartz MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Lime;
            this.lblName.Location = new System.Drawing.Point(24, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(66, 19);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "label1";
            // 
            // Rom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.checkedRom);
            this.Name = "Rom";
            this.Size = new System.Drawing.Size(572, 100);
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        public System.Windows.Forms.CheckBox checkedRom;
        public System.Windows.Forms.PictureBox pic;
    }
}

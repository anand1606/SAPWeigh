namespace SAPWeigh
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
            this.txtOutPut = new System.Windows.Forms.RichTextBox();
            this.btnComConfig = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnToggle = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOutPut
            // 
            this.txtOutPut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOutPut.Font = new System.Drawing.Font("Arial Unicode MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPut.Location = new System.Drawing.Point(12, 12);
            this.txtOutPut.Multiline = false;
            this.txtOutPut.Name = "txtOutPut";
            this.txtOutPut.ReadOnly = true;
            this.txtOutPut.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtOutPut.Size = new System.Drawing.Size(329, 29);
            this.txtOutPut.TabIndex = 1;
            this.txtOutPut.Text = "1bH00Œ1233";
            // 
            // btnComConfig
            // 
            this.btnComConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComConfig.Location = new System.Drawing.Point(12, 45);
            this.btnComConfig.Name = "btnComConfig";
            this.btnComConfig.Size = new System.Drawing.Size(62, 29);
            this.btnComConfig.TabIndex = 2;
            this.btnComConfig.Text = "Config";
            this.btnComConfig.UseVisualStyleBackColor = true;
            this.btnComConfig.Click += new System.EventHandler(this.btnComConfig_Click);
            // 
            // btnReload
            // 
            this.btnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.Location = new System.Drawing.Point(80, 45);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(62, 29);
            this.btnReload.TabIndex = 3;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnToggle
            // 
            this.btnToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggle.Location = new System.Drawing.Point(148, 45);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(62, 29);
            this.btnToggle.TabIndex = 4;
            this.btnToggle.Text = "Start";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Location = new System.Drawing.Point(290, 45);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(62, 29);
            this.btn_Close.TabIndex = 5;
            this.btn_Close.Text = "Stop";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 81);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btnToggle);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnComConfig);
            this.Controls.Add(this.txtOutPut);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SAP Weight Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox txtOutPut;
        private System.Windows.Forms.Button btnComConfig;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.Button btn_Close;
    }
}


namespace CPU_GPU_TEMP_MINI_DISPLAY
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnTOGGLESERIAL = new System.Windows.Forms.Button();
            this.btnTESTDISPLAY = new System.Windows.Forms.Button();
            this.txtTESTINPUT = new System.Windows.Forms.TextBox();
            this.cbSERIALPORTS = new System.Windows.Forms.ComboBox();
            this.btnFULLINFO = new System.Windows.Forms.Button();
            this.btnEXIT = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // btnTOGGLESERIAL
            // 
            this.btnTOGGLESERIAL.Location = new System.Drawing.Point(12, 38);
            this.btnTOGGLESERIAL.Name = "btnTOGGLESERIAL";
            this.btnTOGGLESERIAL.Size = new System.Drawing.Size(121, 23);
            this.btnTOGGLESERIAL.TabIndex = 0;
            this.btnTOGGLESERIAL.Text = "TOGGLE";
            this.btnTOGGLESERIAL.UseVisualStyleBackColor = true;
            this.btnTOGGLESERIAL.Click += new System.EventHandler(this.btnTOGGLESERIAL_Click);
            // 
            // btnTESTDISPLAY
            // 
            this.btnTESTDISPLAY.Location = new System.Drawing.Point(407, 38);
            this.btnTESTDISPLAY.Name = "btnTESTDISPLAY";
            this.btnTESTDISPLAY.Size = new System.Drawing.Size(75, 23);
            this.btnTESTDISPLAY.TabIndex = 1;
            this.btnTESTDISPLAY.Text = "TEST DISPLAY";
            this.btnTESTDISPLAY.UseVisualStyleBackColor = true;
            this.btnTESTDISPLAY.Click += new System.EventHandler(this.btnTESTDISPLAY_Click);
            // 
            // txtTESTINPUT
            // 
            this.txtTESTINPUT.Location = new System.Drawing.Point(301, 40);
            this.txtTESTINPUT.Name = "txtTESTINPUT";
            this.txtTESTINPUT.Size = new System.Drawing.Size(100, 20);
            this.txtTESTINPUT.TabIndex = 3;
            this.txtTESTINPUT.Text = "<C##G##>";
            // 
            // cbSERIALPORTS
            // 
            this.cbSERIALPORTS.FormattingEnabled = true;
            this.cbSERIALPORTS.Location = new System.Drawing.Point(12, 11);
            this.cbSERIALPORTS.Name = "cbSERIALPORTS";
            this.cbSERIALPORTS.Size = new System.Drawing.Size(470, 21);
            this.cbSERIALPORTS.TabIndex = 5;
            this.cbSERIALPORTS.SelectedIndexChanged += new System.EventHandler(this.cbSERIALPORTS_SelectedIndexChanged);
            // 
            // btnFULLINFO
            // 
            this.btnFULLINFO.Location = new System.Drawing.Point(158, 38);
            this.btnFULLINFO.Name = "btnFULLINFO";
            this.btnFULLINFO.Size = new System.Drawing.Size(75, 23);
            this.btnFULLINFO.TabIndex = 7;
            this.btnFULLINFO.Text = "FULL INFO";
            this.btnFULLINFO.UseVisualStyleBackColor = true;
            this.btnFULLINFO.Click += new System.EventHandler(this.btnFULLINFO_Click);
            // 
            // btnEXIT
            // 
            this.btnEXIT.Location = new System.Drawing.Point(488, 11);
            this.btnEXIT.Name = "btnEXIT";
            this.btnEXIT.Size = new System.Drawing.Size(68, 50);
            this.btnEXIT.TabIndex = 8;
            this.btnEXIT.Text = "EXIT APP";
            this.btnEXIT.UseVisualStyleBackColor = true;
            this.btnEXIT.Click += new System.EventHandler(this.btnEXIT_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "click to open cpu and gpu mini display control panel";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 72);
            this.Controls.Add(this.btnEXIT);
            this.Controls.Add(this.btnFULLINFO);
            this.Controls.Add(this.cbSERIALPORTS);
            this.Controls.Add(this.txtTESTINPUT);
            this.Controls.Add(this.btnTESTDISPLAY);
            this.Controls.Add(this.btnTOGGLESERIAL);
            this.Name = "Form1";
            this.Text = "CPU and GPU Mini Display";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTOGGLESERIAL;
        private System.Windows.Forms.Button btnTESTDISPLAY;
        private System.Windows.Forms.TextBox txtTESTINPUT;
        private System.Windows.Forms.ComboBox cbSERIALPORTS;
        private System.Windows.Forms.Button btnFULLINFO;
        private System.Windows.Forms.Button btnEXIT;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}


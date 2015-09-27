namespace DownLoadMachine
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.lblAllTime = new System.Windows.Forms.Label();
            this.lblDownLoad = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblUseTime = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "下载文件地址：";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(158, 21);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(798, 25);
            this.txtUrl.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "保存文件路径：";
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Location = new System.Drawing.Point(159, 58);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.Size = new System.Drawing.Size(798, 25);
            this.txtLocalPath.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(503, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(637, 89);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 3;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(781, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblFileSize
            // 
            this.lblFileSize.AutoSize = true;
            this.lblFileSize.Location = new System.Drawing.Point(30, 127);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(23, 15);
            this.lblFileSize.TabIndex = 5;
            this.lblFileSize.Text = "00";
            // 
            // lblAllTime
            // 
            this.lblAllTime.AutoSize = true;
            this.lblAllTime.Location = new System.Drawing.Point(30, 164);
            this.lblAllTime.Name = "lblAllTime";
            this.lblAllTime.Size = new System.Drawing.Size(23, 15);
            this.lblAllTime.TabIndex = 5;
            this.lblAllTime.Text = "00";
            // 
            // lblDownLoad
            // 
            this.lblDownLoad.AutoSize = true;
            this.lblDownLoad.Location = new System.Drawing.Point(30, 201);
            this.lblDownLoad.Name = "lblDownLoad";
            this.lblDownLoad.Size = new System.Drawing.Size(23, 15);
            this.lblDownLoad.TabIndex = 5;
            this.lblDownLoad.Text = "00";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(30, 236);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(23, 15);
            this.lblSpeed.TabIndex = 5;
            this.lblSpeed.Text = "00";
            // 
            // lblUseTime
            // 
            this.lblUseTime.AutoSize = true;
            this.lblUseTime.Location = new System.Drawing.Point(30, 273);
            this.lblUseTime.Name = "lblUseTime";
            this.lblUseTime.Size = new System.Drawing.Size(23, 15);
            this.lblUseTime.TabIndex = 5;
            this.lblUseTime.Text = "00";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(37, 311);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(920, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 372);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblUseTime);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblDownLoad);
            this.Controls.Add(this.lblAllTime);
            this.Controls.Add(this.lblFileSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtLocalPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Label lblAllTime;
        private System.Windows.Forms.Label lblDownLoad;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblUseTime;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}


namespace WFARoboticDrawing
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
            this.originalPictureBox = new System.Windows.Forms.PictureBox();
            this.convertedPictureBox = new System.Windows.Forms.PictureBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.convertButton = new System.Windows.Forms.Button();
            this.processButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // originalPictureBox
            // 
            this.originalPictureBox.Location = new System.Drawing.Point(8, 8);
            this.originalPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.originalPictureBox.Name = "originalPictureBox";
            this.originalPictureBox.Size = new System.Drawing.Size(333, 390);
            this.originalPictureBox.TabIndex = 0;
            this.originalPictureBox.TabStop = false;
            // 
            // convertedPictureBox
            // 
            this.convertedPictureBox.Location = new System.Drawing.Point(444, 8);
            this.convertedPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.convertedPictureBox.Name = "convertedPictureBox";
            this.convertedPictureBox.Size = new System.Drawing.Size(333, 390);
            this.convertedPictureBox.TabIndex = 1;
            this.convertedPictureBox.TabStop = false;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(8, 402);
            this.loadButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(333, 32);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Load Image";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // convertButton
            // 
            this.convertButton.Location = new System.Drawing.Point(8, 438);
            this.convertButton.Margin = new System.Windows.Forms.Padding(2);
            this.convertButton.Name = "convertButton";
            this.convertButton.Size = new System.Drawing.Size(333, 32);
            this.convertButton.TabIndex = 3;
            this.convertButton.Text = "Convert Image";
            this.convertButton.UseVisualStyleBackColor = true;
            this.convertButton.Click += new System.EventHandler(this.convertButton_Click);
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(444, 411);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(333, 59);
            this.processButton.TabIndex = 4;
            this.processButton.Text = "Command It";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 484);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.convertButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.convertedPictureBox);
            this.Controls.Add(this.originalPictureBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button processButton;
    }
}


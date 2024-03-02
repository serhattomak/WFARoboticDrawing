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
            this.originalLabel = new System.Windows.Forms.Label();
            this.convertedLabel = new System.Windows.Forms.Label();
            this.processButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // originalPictureBox
            // 
            this.originalPictureBox.Location = new System.Drawing.Point(12, 46);
            this.originalPictureBox.Name = "originalPictureBox";
            this.originalPictureBox.Size = new System.Drawing.Size(550, 470);
            this.originalPictureBox.TabIndex = 0;
            this.originalPictureBox.TabStop = false;
            // 
            // convertedPictureBox
            // 
            this.convertedPictureBox.Location = new System.Drawing.Point(565, 46);
            this.convertedPictureBox.Name = "convertedPictureBox";
            this.convertedPictureBox.Size = new System.Drawing.Size(550, 470);
            this.convertedPictureBox.TabIndex = 1;
            this.convertedPictureBox.TabStop = false;
            // 
            // loadButton
            // 
            this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.loadButton.Location = new System.Drawing.Point(12, 521);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(550, 45);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "GÖRSEL YÜKLE";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // convertButton
            // 
            this.convertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.convertButton.Location = new System.Drawing.Point(12, 572);
            this.convertButton.Name = "convertButton";
            this.convertButton.Size = new System.Drawing.Size(550, 45);
            this.convertButton.TabIndex = 3;
            this.convertButton.Text = "GÖRSELİ DÖNÜŞTÜR";
            this.convertButton.UseVisualStyleBackColor = true;
            this.convertButton.Click += new System.EventHandler(this.convertButton_Click);
            // 
            // originalLabel
            // 
            this.originalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.originalLabel.Location = new System.Drawing.Point(12, 21);
            this.originalLabel.Name = "originalLabel";
            this.originalLabel.Size = new System.Drawing.Size(550, 20);
            this.originalLabel.TabIndex = 4;
            this.originalLabel.Text = "YÜKLENEN GÖRSEL";
            this.originalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // convertedLabel
            // 
            this.convertedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.convertedLabel.Location = new System.Drawing.Point(565, 21);
            this.convertedLabel.Name = "convertedLabel";
            this.convertedLabel.Size = new System.Drawing.Size(550, 20);
            this.convertedLabel.TabIndex = 5;
            this.convertedLabel.Text = "DÖNÜŞTÜRÜLEN GÖRSEL";
            this.convertedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // processButton
            // 
            this.processButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.processButton.Location = new System.Drawing.Point(565, 522);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(550, 90);
            this.processButton.TabIndex = 6;
            this.processButton.Text = "KOMUT ÇIKTISI AL";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 621);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.convertedLabel);
            this.Controls.Add(this.originalLabel);
            this.Controls.Add(this.convertButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.convertedPictureBox);
            this.Controls.Add(this.originalPictureBox);
            this.Name = "Form1";
            this.Text = "Staubli TX90L Görseli Komuta Dönüştürme Programı";
            ((System.ComponentModel.ISupportInitialize)(this.originalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertedPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox originalPictureBox;
        private System.Windows.Forms.PictureBox convertedPictureBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button convertButton;
        private System.Windows.Forms.Label originalLabel;
        private System.Windows.Forms.Label convertedLabel;
        private System.Windows.Forms.Button processButton;
    }
}


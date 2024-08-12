namespace OddSlider {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            flatSlider1 = new FlatSlider();
            updateTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // flatSlider1
            // 
            flatSlider1.BackColor = Color.White;
            flatSlider1.Location = new Point(15, 32);
            flatSlider1.Margin = new Padding(6);
            flatSlider1.Maximum = 100;
            flatSlider1.Minimum = 0;
            flatSlider1.Name = "flatSlider1";
            flatSlider1.Size = new Size(362, 101);
            flatSlider1.TabIndex = 0;
            flatSlider1.Text = "flatSlider1";
            flatSlider1.ThumbColor = Color.Blue;
            flatSlider1.ThumbHoverColor = Color.DarkBlue;
            flatSlider1.ThumbImage = (Image)resources.GetObject("flatSlider1.ThumbImage");
            flatSlider1.ThumbImageMode = PictureBoxSizeMode.Zoom;
            flatSlider1.ThumbOffsetY = -35;
            flatSlider1.ThumbSize = 30;
            flatSlider1.TrackColor = Color.White;
            flatSlider1.TrackHeight = 35;
            flatSlider1.TrackImage = (Image)resources.GetObject("flatSlider1.TrackImage");
            flatSlider1.TrackImageMode = PictureBoxSizeMode.StretchImage;
            flatSlider1.Value = 0;
            flatSlider1.ValueChanged = null;
            // 
            // updateTimer
            // 
            updateTimer.Enabled = true;
            updateTimer.Tick += updateTimer_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundColor = Color.White;
            ClientSize = new Size(392, 121);
            CloseButtonColor = Color.LightSteelBlue;
            CloseButtonHoverColor = Color.CadetBlue;
            Controls.Add(flatSlider1);
            CornerRadius = 25;
            DotButtonColor = Color.LightSteelBlue;
            DotButtonHoverColor = Color.CadetBlue;
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private FlatSlider flatSlider1;
        private System.Windows.Forms.Timer updateTimer;
    }
}

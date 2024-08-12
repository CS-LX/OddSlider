namespace OddSlider {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            listBox1 = new ListBox();
            topMostCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(12, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(191, 124);
            listBox1.TabIndex = 0;
            // 
            // topMostCheckbox
            // 
            topMostCheckbox.AutoSize = true;
            topMostCheckbox.Location = new Point(209, 12);
            topMostCheckbox.Name = "topMostCheckbox";
            topMostCheckbox.Size = new Size(61, 24);
            topMostCheckbox.TabIndex = 1;
            topMostCheckbox.Text = "置顶";
            topMostCheckbox.UseVisualStyleBackColor = true;
            topMostCheckbox.CheckedChanged += topMostCheckbox_CheckedChanged;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 150);
            Controls.Add(topMostCheckbox);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "设置";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private CheckBox topMostCheckbox;
    }
}
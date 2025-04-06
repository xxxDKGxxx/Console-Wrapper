namespace JetBrains_rekrutacja
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InputTextBox = new TextBox();
            OutputTextBox = new RichTextBox();
            consoleChooser = new ToolStrip();
            consoleChooserCMD = new ToolStripButton();
            consoleChooserPowerShell = new ToolStripButton();
            label1 = new Label();
            label2 = new Label();
            consoleChooser.SuspendLayout();
            SuspendLayout();
            // 
            // InputTextBox
            // 
            InputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            InputTextBox.BackColor = Color.Black;
            InputTextBox.ForeColor = Color.FromArgb(192, 0, 192);
            InputTextBox.Location = new Point(76, 105);
            InputTextBox.Margin = new Padding(4);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new Size(867, 27);
            InputTextBox.TabIndex = 0;
            InputTextBox.KeyPress += textBox1_KeyPress;
            // 
            // OutputTextBox
            // 
            OutputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OutputTextBox.BackColor = Color.Black;
            OutputTextBox.BorderStyle = BorderStyle.FixedSingle;
            OutputTextBox.ForeColor = Color.White;
            OutputTextBox.Location = new Point(76, 183);
            OutputTextBox.Margin = new Padding(4);
            OutputTextBox.Name = "OutputTextBox";
            OutputTextBox.ReadOnly = true;
            OutputTextBox.Size = new Size(867, 363);
            OutputTextBox.TabIndex = 1;
            OutputTextBox.TabStop = false;
            OutputTextBox.Text = "";
            // 
            // consoleChooser
            // 
            consoleChooser.BackColor = SystemColors.ControlDarkDark;
            consoleChooser.Items.AddRange(new ToolStripItem[] { consoleChooserCMD, consoleChooserPowerShell });
            consoleChooser.Location = new Point(0, 0);
            consoleChooser.Name = "consoleChooser";
            consoleChooser.RenderMode = ToolStripRenderMode.Professional;
            consoleChooser.Size = new Size(1029, 25);
            consoleChooser.TabIndex = 2;
            consoleChooser.Text = "toolStrip1";
            // 
            // consoleChooserCMD
            // 
            consoleChooserCMD.DisplayStyle = ToolStripItemDisplayStyle.Image;
            consoleChooserCMD.Image = Resource1.cmd;
            consoleChooserCMD.ImageTransparentColor = Color.Magenta;
            consoleChooserCMD.Name = "consoleChooserCMD";
            consoleChooserCMD.Size = new Size(23, 22);
            consoleChooserCMD.Text = "consoleChooserCMD";
            consoleChooserCMD.Click += consoleChooserCMD_Click;
            // 
            // consoleChooserPowerShell
            // 
            consoleChooserPowerShell.DisplayStyle = ToolStripItemDisplayStyle.Image;
            consoleChooserPowerShell.Image = Resource1.PowerShell_51;
            consoleChooserPowerShell.ImageTransparentColor = Color.Magenta;
            consoleChooserPowerShell.Name = "consoleChooserPowerShell";
            consoleChooserPowerShell.Size = new Size(23, 22);
            consoleChooserPowerShell.Text = "consoleChooserPowerShell";
            consoleChooserPowerShell.Click += consoleChooserPowerShell_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(77, 79);
            label1.Name = "label1";
            label1.Size = new Size(127, 20);
            label1.TabIndex = 3;
            label1.Text = "Command Input:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(77, 160);
            label2.Name = "label2";
            label2.Size = new Size(113, 20);
            label2.TabIndex = 4;
            label2.Text = "Output Stream";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1029, 600);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(consoleChooser);
            Controls.Add(OutputTextBox);
            Controls.Add(InputTextBox);
            Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "Form1";
            Opacity = 0.95D;
            Text = "Console Emulator";
            FormClosing += Form1_FormClosing;
            KeyPress += Form1_KeyPress;
            consoleChooser.ResumeLayout(false);
            consoleChooser.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputTextBox;
        private RichTextBox OutputTextBox;
        private ToolStrip consoleChooser;
        private ToolStripButton consoleChooserCMD;
        private ToolStripButton consoleChooserPowerShell;
        private Label label1;
        private Label label2;
    }
}

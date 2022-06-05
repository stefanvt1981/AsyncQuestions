namespace Voorbeeld_5
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

        private void ConfigureTrueButton_Click(object sender, EventArgs e)
        {
            AsyncTest(true);
        }
        private void ConfigureFalseButton_Click(object sender, EventArgs e)
        {
            AsyncTest(false);
        }
        private async void AsyncTest(bool configureAwait)
        {
            this.ResultsListBox.Items.Clear();
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-EG");
                this.ResultsListBox.Items.Add("Async test started");
                this.ResultsListBox.Items.Add(string.Format("configureAwait = {0}", configureAwait));
                this.ResultsListBox.Items.Add(string.Format("Current thread ID = {0}", Thread.CurrentThread.ManagedThreadId));
                this.ResultsListBox.Items.Add(string.Format("Current culture = {0}", Thread.CurrentThread.CurrentCulture));
                this.ResultsListBox.Items.Add("Awaiting a task...");
                await Task.Delay(500).ConfigureAwait(configureAwait);
                this.ResultsListBox.Items.Add("Task completed");
                this.ResultsListBox.Items.Add(string.Format("Current thread ID: {0}", Thread.CurrentThread.ManagedThreadId));
                this.ResultsListBox.Items.Add(string.Format("Current culture: {0}", Thread.CurrentThread.CurrentCulture));
            }
            catch (InvalidOperationException ex)
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                this.ResultsListBox.BeginInvoke((Action)(() => {
                    this.ResultsListBox.Items.Add($"{ex.GetType().Name} caught from thread {threadId}");
                }));
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.ConfigureTrueButton = new System.Windows.Forms.Button();
            this.ConfigureFalseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // ResultsListBox
            //
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 12);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(517, 342);
            this.ResultsListBox.TabIndex = 0;
            //
            // ConfigureTrueButton
            //
            this.ConfigureTrueButton.Location = new System.Drawing.Point(12, 357);
            this.ConfigureTrueButton.Name = "ConfigureTrueButton";
            this.ConfigureTrueButton.Size = new System.Drawing.Size(516, 23);
            this.ConfigureTrueButton.TabIndex = 1;
            this.ConfigureTrueButton.Text = "Task.ConfigureAwait(true) Test";
            this.ConfigureTrueButton.UseVisualStyleBackColor = true;
            this.ConfigureTrueButton.Click += new System.EventHandler(this.ConfigureTrueButton_Click);
            //
            // ConfigureFalseButton
            //
            this.ConfigureFalseButton.Location = new System.Drawing.Point(12, 386);
            this.ConfigureFalseButton.Name = "ConfigureFalseButton";
            this.ConfigureFalseButton.Size = new System.Drawing.Size(516, 23);
            this.ConfigureFalseButton.TabIndex = 2;
            this.ConfigureFalseButton.Text = "Task.ConfigureAwait(false) Test";
            this.ConfigureFalseButton.UseVisualStyleBackColor = true;
            this.ConfigureFalseButton.Click += new System.EventHandler(this.ConfigureFalseButton_Click);
            //
            // ConfigureAwaitForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 421);
            this.Controls.Add(this.ConfigureFalseButton);
            this.Controls.Add(this.ConfigureTrueButton);
            this.Controls.Add(this.ResultsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ConfigureAwaitForm";
            this.Text = "Task.ConfigureAwait Sample";
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.Button ConfigureTrueButton;
        private System.Windows.Forms.Button ConfigureFalseButton;


        #endregion
    }
}
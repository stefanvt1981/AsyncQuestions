namespace Voorbeeld_4
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

        private void NoContextButton_Click(object sender, EventArgs e)
        {
            RunTask(null);
        }
        private void UIContextButton_Click(object sender, EventArgs e)
        {
            RunTask(SynchronizationContext.Current);
        }
        private void RunTask(SynchronizationContext context)
        {
            this.ResultsListBox.Items.Clear();
            this.ResultsListBox.Items.Add($"UI Thread {Thread.CurrentThread.ManagedThreadId}");
            Task.Run(async () => {
                if (null != context) SynchronizationContext.SetSynchronizationContext(context);
                LogMessage($"Task started");
                if (null == SynchronizationContext.Current) LogMessage($"Task synchronization context is null");
                else LogMessage($"Task synchronization context is {SynchronizationContext.Current.GetType().Name}");
                await Task.Delay(1000);
                LogMessage($"Task thread is {Thread.CurrentThread.ManagedThreadId}");
                LogMessage($"Control.InvokeRequired = {this.ResultsListBox.InvokeRequired}");
                LogMessage($"Trying to manipulate UI...");
                try
                {
                    this.ResultsListBox.Items.Add("Successfully accessed UI directly!");
                }
                catch (InvalidOperationException)
                {
                    LogMessage($"Failed!");
                }
                LogMessage($"Task finished");
            });
        }
        private void LogMessage(string msg)
        {
            this.ResultsListBox.Invoke((Action)(() => {
                this.ResultsListBox.Items.Add(msg);
            }));
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.NoContextButton = new System.Windows.Forms.Button();
            this.UIContextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // ResultsListBox
            //
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(13, 13);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(429, 264);
            this.ResultsListBox.TabIndex = 0;
            //
            // NoContextButton
            //
            this.NoContextButton.Location = new System.Drawing.Point(13, 284);
            this.NoContextButton.Name = "NoContextButton";
            this.NoContextButton.Size = new System.Drawing.Size(429, 23);
            this.NoContextButton.TabIndex = 1;
            this.NoContextButton.Text = "Task without Synchronization Context";
            this.NoContextButton.UseVisualStyleBackColor = true;
            this.NoContextButton.Click += new System.EventHandler(this.NoContextButton_Click);
            //
            // UIContextButton
            //
            this.UIContextButton.Location = new System.Drawing.Point(13, 313);
            this.UIContextButton.Name = "UIContextButton";
            this.UIContextButton.Size = new System.Drawing.Size(429, 23);
            this.UIContextButton.TabIndex = 2;
            this.UIContextButton.Text = "Task with UI Synchronization Context";
            this.UIContextButton.UseVisualStyleBackColor = true;
            this.UIContextButton.Click += new System.EventHandler(this.UIContextButton_Click);
            //
            // TasksForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 345);
            this.Controls.Add(this.UIContextButton);
            this.Controls.Add(this.NoContextButton);
            this.Controls.Add(this.ResultsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TasksForm";
            this.Text = "TasksForm";
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.Button NoContextButton;
        private System.Windows.Forms.Button UIContextButton;


        #endregion
    }
}
namespace Voorbeeld_3._1
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

        private void RegularThreadsButton_Click(object sender, EventArgs e)
        {
            RunThreads(null);
        }
        private void UIThreadTest_Click(object sender, EventArgs e)
        {
            // SynchronizationContext.Current will return a reference to WindowsFormsSynchronizationContext
            RunThreads(SynchronizationContext.Current);
        }
        private void RunThreads(SynchronizationContext context)
        {
            this.ResultsListBox.Items.Clear();
            this.ResultsListBox.Items.Add($"UI Thread {Thread.CurrentThread.ManagedThreadId}");
            this.ResultsListBox.Items.Clear();
            int maxThreads = 3;
            for (int i = 0; i < maxThreads; i++)
            {
                Thread t = new Thread(UpdateListBox);
                t.IsBackground = true;
                t.Start(context); // passing context to thread proc
            }
        }
        private void UpdateListBox(object state)
        {
            // fetching passed SynchrnozationContext
            SynchronizationContext syncContext = state as SynchronizationContext;
            // get thread ID
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (null == syncContext) // no SynchronizationContext provided
                this.ResultsListBox.Items.Add($"Hello from thread {threadId}, currently executing thread is {Thread.CurrentThread.ManagedThreadId}");
            else syncContext.Send((obj) => this.ResultsListBox.Items.Add($"Hello from thread {threadId}, currently executing thread is {Thread.CurrentThread.ManagedThreadId}"), null);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.RegularThreadsButton = new System.Windows.Forms.Button();
            this.UIThreadTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // ResultsListBox
            //
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 12);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(516, 212);
            this.ResultsListBox.TabIndex = 1;
            //
            // RegularThreadsButton
            //
            this.RegularThreadsButton.Location = new System.Drawing.Point(12, 232);
            this.RegularThreadsButton.Name = "RegularThreadsButton";
            this.RegularThreadsButton.Size = new System.Drawing.Size(516, 23);
            this.RegularThreadsButton.TabIndex = 2;
            this.RegularThreadsButton.Text = "Regular Thread Test";
            this.RegularThreadsButton.UseVisualStyleBackColor = true;
            this.RegularThreadsButton.Click += new System.EventHandler(this.RegularThreadsButton_Click);
            //
            // UIThreadTest
            //
            this.UIThreadTest.Location = new System.Drawing.Point(12, 261);
            this.UIThreadTest.Name = "UIThreadTest";
            this.UIThreadTest.Size = new System.Drawing.Size(516, 23);
            this.UIThreadTest.TabIndex = 3;
            this.UIThreadTest.Text = "UI-Context Thread Test";
            this.UIThreadTest.UseVisualStyleBackColor = true;
            this.UIThreadTest.Click += new System.EventHandler(this.UIThreadTest_Click);
            //
            // ThreadsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 294);
            this.Controls.Add(this.UIThreadTest);
            this.Controls.Add(this.RegularThreadsButton);
            this.Controls.Add(this.ResultsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ThreadsForm";
            this.Text = "SynchronizationContext Sample";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.Button RegularThreadsButton;
        private System.Windows.Forms.Button UIThreadTest;

        #endregion
    }
}
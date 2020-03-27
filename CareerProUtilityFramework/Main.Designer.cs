namespace CareerProUtilityFramework
{
	partial class Main
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
			this.pbPickLoadDir = new System.Windows.Forms.Button();
			this.txtLoadPath = new System.Windows.Forms.TextBox();
			this.pbButtonLoad = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pbPickLoadDir
			// 
			this.pbPickLoadDir.Location = new System.Drawing.Point(36, 42);
			this.pbPickLoadDir.Name = "pbPickLoadDir";
			this.pbPickLoadDir.Size = new System.Drawing.Size(195, 23);
			this.pbPickLoadDir.TabIndex = 0;
			this.pbPickLoadDir.Text = "&Pick Load Dir..";
			this.pbPickLoadDir.UseVisualStyleBackColor = true;
			this.pbPickLoadDir.Click += new System.EventHandler(this.pbPickLoadDir_Click);
			// 
			// txtLoadPath
			// 
			this.txtLoadPath.Location = new System.Drawing.Point(257, 42);
			this.txtLoadPath.Name = "txtLoadPath";
			this.txtLoadPath.Size = new System.Drawing.Size(235, 22);
			this.txtLoadPath.TabIndex = 1;
			// 
			// pbButtonLoad
			// 
			this.pbButtonLoad.Location = new System.Drawing.Point(520, 40);
			this.pbButtonLoad.Name = "pbButtonLoad";
			this.pbButtonLoad.Size = new System.Drawing.Size(129, 23);
			this.pbButtonLoad.TabIndex = 2;
			this.pbButtonLoad.Text = "Load Resumes!";
			this.pbButtonLoad.UseVisualStyleBackColor = true;
			this.pbButtonLoad.Click += new System.EventHandler(this.pbButtonLoad_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(36, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pbButtonLoad);
			this.Controls.Add(this.txtLoadPath);
			this.Controls.Add(this.pbPickLoadDir);
			this.Name = "Main";
			this.Text = "CarrerProUtility";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button pbPickLoadDir;
		private System.Windows.Forms.TextBox txtLoadPath;
		private System.Windows.Forms.Button pbButtonLoad;
		private System.Windows.Forms.Label label1;
	}
}


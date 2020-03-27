using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Npgsql;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;


namespace CareerProUtilityFramework
{
	public partial class Main : Form
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public Main()
		{
			InitializeComponent();
			txtLoadPath.Text = Properties.CareerProUtil.Default.LoadLocation;
		}

		private void pbPickLoadDir_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.SelectedPath = Properties.CareerProUtil.Default.LoadLocation;
				// Do not allow the user to create new files via the FolderBrowserDialog.
				fbd.ShowNewFolderButton = false;

				// Default to the My Documents folder.
				//fbd.RootFolder = Environment.SpecialFolder.Personal;

				DialogResult result = fbd.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					txtLoadPath.Text = fbd.SelectedPath;
					Properties.CareerProUtil.Default.LoadLocation = fbd.SelectedPath;
					Properties.CareerProUtil.Default.Save();

					//string[] files = Directory.GetFiles(fbd.SelectedPath);

					//System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
				}
			}
		}

		private void pbButtonLoad_Click(object sender, EventArgs e)
		{
			string[] files = Directory.GetFiles(txtLoadPath.Text,@"*.doc*");
		
			string connstring = String.Format($"Server={Properties.CareerProUtil.Default.dbHost};Port={Properties.CareerProUtil.Default.port};" +
				$"User Id={Properties.CareerProUtil.Default.dbUser};Password={Properties.CareerProUtil.Default.dbPass};Database={Properties.CareerProUtil.Default.dbName};");

			// Making connection with Npgsql provider
			using (NpgsqlConnection conn = new NpgsqlConnection(connstring))
			{
				try
				{
					conn.Open();
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "Error");
				}
				int totalLoaded = 0;

				Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
				foreach (var file in files)
				{
					using (var transaction = conn.BeginTransaction())
					{
						var ri = new ResumeInfo(file);
						Guid personId = Guid.NewGuid();
						string insertStatement = $"Insert into person ( person_id, last_name) values ('{{{personId}}}', '{ri.Name}')";
						NpgsqlCommand cmd = new NpgsqlCommand(insertStatement);
						cmd.Connection = conn;
						logger.Info(insertStatement);


						var rowsAffected = cmd.ExecuteNonQuery();  //cmd.ExecuteReader(); 
						if (rowsAffected != 1)
						{
							//MessageBox.Show(insertStatement, "InsertStatement Failed");
							logger.Error("InsertStatement Failed");
							logger.Error(insertStatement);
							break;
						}
						Guid resumeId = Guid.NewGuid();
						//sring resume = File.ReadAllText(file);
						//var resume = File.ReadAllBytes(file);
						//string InsertResumeStatement = $"Insert into resumes_raw (resume_id, resume_name, date, resume_desc, orig_file_name, resume)" +
						//												$"'{{{resumeId}}}','{ri.Name}', '{DateTime.Now.ToString()}', '{ri.Desc}','{ri.FileName}','{resume}')";

						try
						{
							Document document = application.Documents.Open(file);

							var wordContentRange = document.Content;
							var resumeTxt = wordContentRange.Text;
							var emails = ExtractEmails(resumeTxt);
							if (emails != null)
							{
								foreach (var email in emails)
								{
									Guid emailId = Guid.NewGuid();
									string insertEmails = $"Insert into email values ('{{{emailId}}}','{{{personId}}}','{email}')";
									NpgsqlCommand cmdInsertEmails = new NpgsqlCommand(insertEmails);
									cmdInsertEmails.Connection = conn;
									logger.Info(insertEmails);
									int nRowsAffected = cmdInsertEmails.ExecuteNonQuery();
									if (nRowsAffected == 0)
									{
										logger.Error("Could not Insert Emails");
										logger.Error(insertEmails);
									}
								}
								transaction.Commit();
								totalLoaded++;
							}
							else
							{
								logger.Error($"No Emails for {ri.FileName}");
								transaction.Rollback();
							}
							document.Close();
						}
						catch (Exception ex3)
						{
							logger.Error(" Problems with file: " + file);
							transaction.Rollback();
							logger.Error(ex3.Message);
							break;

						}
					
						
						
					}
					
				}
				MessageBox.Show(string.Format($"{totalLoaded} items loaded"), "info");
			}
		}

		public List<string> ExtractEmails( string input)
		{
			List<string> emails = null;
			Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
			//find items that matches with our pattern
			MatchCollection emailMatches = emailRegex.Matches(input);
			foreach(Match emailMatch in emailMatches)
			{
				if (emails == null)
					emails = new List<string>();
				emails.Add(emailMatch.Value);
			}

			return emails;
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}



	public class ResumeInfo
			{

			
			public ResumeInfo(string fileName)
			{
				FileName = Path.GetFileNameWithoutExtension(fileName);
				var info = FileName.Split('-');
				if (info.Length == 2)
				{
				Name = info[0].Trim().Replace("'", "") ;
					Desc = info[1].Trim().Replace("'", "");
				}
				else
				{
				info = FileName.Split(' ');
					Name = info[0].Trim().Replace("'", ""); ;
					if ( info.Length == 2 )
						Desc = info[1].Trim().Replace("'", "");

			}

			}
			public string Name { get; set; }
			public string Desc { get; set; }

			public string FileName { get; set; }

		

		}

	
	}


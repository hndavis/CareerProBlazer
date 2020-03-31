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
using NpgsqlTypes;
using System.Threading;

namespace CareerProUtilityFramework
{
	public partial class Main : Form
	{
		private CancellationTokenSource source = new CancellationTokenSource();
		private CancellationToken token;
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public Main()
		{
			InitializeComponent();
			txtLoadPath.Text = Properties.CareerProUtil.Default.LoadLocation;
			token = source.Token;
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
			string[] files = Directory.GetFiles(txtLoadPath.Text, @"*.doc*");
			using (Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application())
			{
				string connstring = String.Format($"Server={Properties.CareerProUtil.Default.dbHost};Port={Properties.CareerProUtil.Default.port};" +
					$"User Id={Properties.CareerProUtil.Default.dbUser};Password={Properties.CareerProUtil.Default.dbPass};Database={Properties.CareerProUtil.Default.dbName};");
				Task<int> t = System.Threading.Tasks.Task.Run(() =>
				{
					int totalLoaded = 0;
					using (NpgsqlConnection conn = new NpgsqlConnection(connstring))
					{
						try
						{
							conn.Open();
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message, "Error");
						}

						string startingFile = "";
						bool inTheMiddle = false;
						if (File.Exists("LastFile.txt"))
						{
							var previousFile = File.OpenText("LastFile.txt");
							startingFile = previousFile.ReadToEnd();
							previousFile.Close();
							inTheMiddle = true;

						}

						foreach (var file in files)
						{
							if (token.IsCancellationRequested)
								return totalLoaded;

							if (inTheMiddle)
							{
								if (file == startingFile)
								{
									inTheMiddle = false;
								}
								else
								{
									continue;
								}
							}

							label1.BeginInvoke((Action)(() =>
							{
								label1.Text = file;
							}));

							using (var transaction = conn.BeginTransaction())
							{
								var ri = new ResumeInfo(file);
								Guid personId = Guid.NewGuid();
								string insertStatement = $"Insert into person ( person_id, last_name, first_name, orig_file_name ) values ('{{{personId}}}', '{ri.LName}','{ri.FName}', '{ri.FileName}')";

								logger.Info(insertStatement);
								NpgsqlCommand cmd = new NpgsqlCommand(insertStatement);
								cmd.Connection = conn;



								var rowsAffected = cmd.ExecuteNonQuery();  //cmd.ExecuteReader(); 
							if (rowsAffected != 1)
								{
								//MessageBox.Show(insertStatement, "InsertStatement Failed");
								logger.Error("InsertStatement Failed");
								//logger.Error(insertStatement);
								continue;
								}
								Guid resumeId = Guid.NewGuid();

							//try
							//{
							//var resume = File.ReadAllBytes(file);
							//string InsertResumeStatement = $"Insert into resumes_raw (resume_id, resume_name, date, resume_desc, orig_file_name, resume)" +
							//												$"'{{{resumeId}}}','{ri.Name}', '{DateTime.Now.ToString()}', '{ri.Desc}','{ri.FileName}',:datResume)";
							//NpgsqlCommand cmdInsertResumesRaw = new NpgsqlCommand(InsertResumeStatement);
							//cmdInsertResumesRaw.Connection = conn;

							// cmdInsertResumesRaw.Parameters.Add(new NpgsqlParameter("datResume", NpgsqlDbType.Integer) { Value = resume });


							//NpgsqlParameter param = new NpgsqlParameter("dataParam", NpgsqlDbType.Integer);
							//param.Value = resume;
							//cmdInsertResumesRaw.Parameters.Add(param);
							//int rawRowsAffected = cmdInsertResumesRaw.ExecuteNonQuery();
							//if (rawRowsAffected == 0)
							//{
							//	logger.Error($"Could not insert {ri.FileName} to resumes_raw)");
							//}
							//}
							//catch (Exception exRaw)
							//{
							//	logger.Error($"Could not insert {ri.FileName} to resumes_raw)");
							//	logger.Error(exRaw.Message);
							//	transaction.Rollback();
							//	break;
							//}



							try
								{
									Document document = application.Documents.Open(file);
								// need to turn document into byte array

								// need to turn document into searchable text
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
									continue;

								}

								var lastFile = File.CreateText("LastFile.txt");
								lastFile.Write(file);
								lastFile.Close();

							}

						}
					}
					return totalLoaded;
				}, token
					);
				t.Wait();
				File.Delete("LastFile.txt");
			}
			MessageBox.Show(string.Format($"{t.Result} items loaded"), "info");
			// Making connection with Npgsql provider
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
				FileName = Path.GetFileNameWithoutExtension(fileName).Trim().Replace("'", "");
				var info = FileName.Split('-');
			switch (info.Length)
			{
				case 2: //sdasd - aworker
					Name = info[0].Trim().Replace("'", "");
					Desc = info[1].Trim().Replace("'", "");
					break;
				case 1: // adads aworker
					info = FileName.Split(' ');
					Name = info[0].Trim().Replace("'", ""); ;
					if (info.Length == 2)
						Desc = info[1].Trim().Replace("'", "");
					break;
				case 3: // hyfenated-lastname, firstname - job desc
					Name = info[0].Trim().Replace("'", "") + info[1].Trim().Replace("'", "");
					Desc = info[2].Trim().Replace("'", "");
					break;

				case 4:
				default:
					Name = info[0].Trim().Replace("'", "");
					Desc = info[2].Trim().Replace("'", "");
					break;
			}
			
			char[] seperators = new char[] { ',',' ' };
			var name = Name.Split(seperators);
			if ( name.Length > 1)
			{
				LName = name[0].Trim();
				FName = name[1].Trim();
			}
			else
			{
				LName = Name;
			}
		}
		public string Name { get; set; }
		public string FName { get; set; }
		public string  LName { get; set; }
		public string Desc { get; set; }

		public string FileName { get; set; }

		

		}

	
	}


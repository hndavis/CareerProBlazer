using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CareerProUtil
{
	public class ResumeLoader
	{
		String StartingDir;
		int count;
		int total;
		public ResumeLoader( string StartDirectory)
		{
			StartingDir = StartDirectory;

		}

		async Task<FileInfo[]> Start()
		{
			DirectoryInfo di = new DirectoryInfo(StartingDir);
			FileInfo[] resumes = di.GetFiles("*.doc*");
			foreach(var resume in resumes )
			{

			}
			return resumes;
		}

	}
}

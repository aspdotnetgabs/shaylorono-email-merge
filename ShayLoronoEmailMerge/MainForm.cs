/*
 * Created by SharpDevelop.
 * User: Gabs
 * Date: 12/9/2020
 * Time: 2:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SimpleExcelImport;
using System.Linq;

namespace ShayLoronoEmailMerge
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		List<Student> epStudentMasterlist;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void BtnLoadExcelDataClick(object sender, EventArgs e)
		{
			string epFolderPath = @"C:\Users\DRIVE_D\My Documents\_GIT_VCS\AspDotNetGabs\shaylorono-email-merge\ShayLoronoEmailMerge\";
			
			var data = System.IO.File.ReadAllBytes(Path.Combine(epFolderPath, "EP_Student.xlsx"));
            var import = new ImportFromExcel();
            import.LoadXlsx(data);            
            //first parameter it's the sheet number in the excel workbook
            //second parameter it's the number of rows to skip at the start(we have an header in the file)
            epStudentMasterlist = import.ExcelToList<Student>(0, 1);   
            epStudentMasterlist = epStudentMasterlist.Where(x => !string.IsNullOrWhiteSpace(x.IDNumber)).ToList();
//            epStudentMasterlist.ForEach(x => {
//            	                            	
//            });
                        
            bindingSourceEPStudents.DataSource = epStudentMasterlist;
            
            var roomLookup = epStudentMasterlist.Select(s => s.Room).Distinct().ToArray();
            comboBoxRooms.Items.AddRange(roomLookup);
		}
		void BtnSearchClick(object sender, EventArgs e)
		{
			if(string.IsNullOrWhiteSpace(txtSearch.Text)) return;
			
			string search = txtSearch.Text.Trim();
			int n;
			if(int.TryParse(search, out n))
			{
				bindingSourceEPStudents.DataSource = epStudentMasterlist.Where(x => x.IDNumber == search).ToList();
			}
			else
			{
				bindingSourceEPStudents.DataSource = epStudentMasterlist.Where(x => x.StudentName.ToLower().Contains(search.ToLower())).ToList();
			}
		}
		void ComboBoxRoomsSelectedIndexChanged(object sender, EventArgs e)
		{
			bindingSourceEPStudents.DataSource = epStudentMasterlist.Where(x => x.Room == comboBoxRooms.Text).ToList();
		}
	}
}

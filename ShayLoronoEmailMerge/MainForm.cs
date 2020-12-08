﻿/*
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
		string epFolderPath = @"C:\Users\DRIVE_D\My Documents\_GIT_VCS\AspDotNetGabs\shaylorono-email-merge\ShayLoronoEmailMerge\";
		List<Student> epStudentMasterlist;
		
		string message = @"<p>Dear EP Parents,</p><p>It’s very important that a child’s progress is carefully monitored throughout their school career, as educators and parents we need to know that our students/children are learning at the intended rate of development. </p>" 
			+ "<p>The Cambridge University CEFR language development scale, which ranges from Pre A1 to C2, allows us to measure your child’s English language development from Grade 1 to Grade 12. This system identifies high performers together with students who require extra support. It’s essential, in an English based curriculum like EP, that our students have the language ability to follow their English and English based subject lessons. </p>" 
			+ "<p>We also want to share this information with you through report cards. Attached to this email you will find two documents. The first, a PDF that explains the report card, what each section means and where the information came from. The second, your actual child’s report card. </p>" 
			+ "<p>If you would like to know more about the CEFR scale please visit the Cambridge University website (https://www.cambridgeenglish.org/exams-and-tests/cefr/), if you have any questions or concerns please contact the EP office. </p><p>Kind regards,</p><p><br></p><p>Bob Jordan<br> Director of Studies <br>Bell Language Centre Assumption College Thonburi<p>";
		
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
		
			var data = System.IO.File.ReadAllBytes(Path.Combine(epFolderPath, "EP_Student.xlsx"));
            var import = new ImportFromExcel();
            import.LoadXlsx(data);            
            //first parameter it's the sheet number in the excel workbook
            //second parameter it's the number of rows to skip at the start(we have an header in the file)
            epStudentMasterlist = import.ExcelToList<Student>(0, 1);   
            epStudentMasterlist = epStudentMasterlist.Where(x => !string.IsNullOrWhiteSpace(x.IDNumber)).ToList();
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
		
		void BtnSendAllClick(object sender, EventArgs e)
		{
			if(bindingSourceEPStudents.DataSource == null) return;
			btnSendAll.Enabled = false;
						
			bool success = true;
			var sendList = (List<Student>)bindingSourceEPStudents.List;
			
			sendList.ToList().ForEach(x => {
			    sendingStatusStrip.Text = "Sending email to " + x.StudentName + "...";
			    string attachFile = Path.Combine(epFolderPath, "BELL REPORT", x.Room.Replace("Room ", string.Empty), x.PdfFilename.Trim() + ".pdf");
			    string sendTo = x.Email; // "hewbertgabon@gmail.com";
             	string subject = "Bell Report";
             	string ccopy = "sensie325@gmail.com"; 
             	success = EmailService.SendEmail(sendTo, subject, message, attachFile, ccopy);
             	sendingStatusStrip.Text = success ? "Successfully sent." : "Sending failed.";
             });
			
			if(success)
				MessageBox.Show("Emails sent successfully.");
			else
				MessageBox.Show("Emails sending failed.");
			
			btnSendAll.Enabled = true;
		}
		
		void BtnSendOneClick(object sender, EventArgs e)
		{	
			if(bindingSourceEPStudents.DataSource == null) return;			
			
			btnSendOne.Enabled = false;
			
			var epStudent = (Student)bindingSourceEPStudents.Current; 
			
		    string attachFile = Path.Combine(epFolderPath, "BELL REPORT", epStudent.Room.Replace("Room ", string.Empty), epStudent.PdfFilename.Trim() + ".pdf");
		    string sendTo = epStudent.Email;
         	string subject = "Bell Report";
         	string ccopy = "sensie325@gmail.com"; 
         	var success = EmailService.SendEmail(sendTo, subject, message, attachFile, ccopy);	
         	
			if(success)
				MessageBox.Show("Emails sent successfully.");
			else
				MessageBox.Show("Emails sending failed.");
			
			btnSendOne.Enabled = true;         	
		}
	}
}

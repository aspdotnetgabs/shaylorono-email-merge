using SimpleExcelImport;

public class Student
{
	[ExcelImport("IDNumber", order = 6)]
	public string IDNumber { get; set; }
	
	[ExcelImport("StudentName", order = 5)]
	public string StudentName { get; set; }
	
	[ExcelImport("Gender", order = 4)]
	public string Gender { get; set; }
	
	[ExcelImport("Email", order = 7)]
	public string Email { get; set; }
	
	[ExcelImport("ClassRecordNo", order = 1)]
	public string ClassRecordNo { get; set; }
	
	[ExcelImport("GradeLevel", order = 2)]
	public string GradeLevel { get; set; }
	
	[ExcelImport("Room", order = 3)]
	public string Room { get; set; }
	
	[ExcelImport("PdfFilename", order = 8)]
	public string PdfFilename { get; set; }
	
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikWeb_WCF.BusinessLogic
{
	[Serializable]
	public class Employee
	{
		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string Gender { get; set; }
		public decimal Salary { get; set; }
		public int Createdby { get; set; }
		public int TotalCount { get; set; }
	}
}
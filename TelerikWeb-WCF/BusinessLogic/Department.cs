using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikWeb_WCF.BusinessLogic
{
	[Serializable]
	public class Department
	{
		public int DepartmentID { get; set; }
		public string DepartmentName { get; set; }
	}
}
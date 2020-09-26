using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelerikWeb_WCF.ServiceAdapter;

namespace TelerikWeb_WCF.BusinessLogic
{
	public class EmployeeManager
	{
		public static List<Employee> GetEmployees(int pageIndex, int pageSize, int? departemntId, string keyword, string columnName, bool orderASC)
		{
			return EmployeeServiceAdapter.GetEmployees(pageIndex, pageSize, departemntId, keyword, columnName, orderASC);
		}

		public static int UpdateEmployee(Employee employee)
		{
			return EmployeeServiceAdapter.UpdateEmployee(employee);
		}

		public static void DeleteEmployee(int employeeId, int deletedBy)
		{
			EmployeeServiceAdapter.DeleteEmployee(employeeId, deletedBy);
		}

		public static List<Department> GetDepartment()
		{
			return EmployeeServiceAdapter.GetDepartment();
		}
	}
}
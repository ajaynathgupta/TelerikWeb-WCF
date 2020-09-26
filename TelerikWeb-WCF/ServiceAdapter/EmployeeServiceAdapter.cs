using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TelerikWeb_WCF.BusinessLogic;
using TelerikWeb_WCF.EmployeeServiceReference;

namespace TelerikWeb_WCF.ServiceAdapter
{

	public class EmployeeServiceAdapter
	{
		#region GetEmployees
		public static List<Employee> GetEmployees(int pageIndex, int pageSize, int? departemntId, string keyword, string columnName, bool orderASC)
		{
			var empList = new List<Employee>();

			using (var serviceClient = new EmployeeServicesClient())
			{
				var empDTO = serviceClient.GetEmployees(pageIndex, pageSize, departemntId, keyword, columnName, orderASC).ToList();
				empDTO.ForEach(dto =>

				empList.Add(ConvertEmployeeDTOToEntity(dto))
				);
			}

			return empList;
		}

		private static Employee ConvertEmployeeDTOToEntity(EmployeeDTO dto)
		{
			return new Employee()
			{
				EmployeeId = dto.EmployeeId,
				EmployeeName = dto.EmployeeName,
				DepartmentId = dto.DepartmentId,
				DepartmentName = dto.DepartmentName,
				Email = dto.Email,
				Address = dto.Address,
				Gender = dto.Gender,
				Salary = dto.Salary,
				TotalCount = dto.TotalCount,
			};
		}
		#endregion


		#region GetEmployees
		public static int UpdateEmployee(Employee employee)
		{
			using (var serviceClient = new EmployeeServicesClient())
			{
				var employeeId = serviceClient.UpdateEmployee(ConvertEmployeeEntityToDTO(employee));
				return employeeId;
			}
		}

		private static EmployeeDTO ConvertEmployeeEntityToDTO(Employee entity)
		{
			return new EmployeeDTO()
			{
				EmployeeId = entity.EmployeeId,
				EmployeeName = entity.EmployeeName,
				DepartmentId = entity.DepartmentId,
				Email = entity.Email,
				Address = entity.Address,
				Gender = entity.Gender,
				Salary = entity.Salary,
				CreatedBy = entity.Createdby
			};
		}
		#endregion

		#region DeleteEmployee
		public static void DeleteEmployee(int employeeId, int deletedBy)
		{
			using (var serviceClient = new EmployeeServicesClient())
			{
				serviceClient.DeleteEmployee(employeeId, deletedBy);
			}
		}
		#endregion

		#region DeleteEmployee
		public static List<Department> GetDepartment()
		{
			using (var serviceClient = new EmployeeServicesClient())
			{
				var departmentList = new List<Department>();
				var departmentDto = serviceClient.GetDepartment().ToList();
				if (departmentDto != null)
				{
					departmentDto.ForEach(x =>
					{
						departmentList.Add(new Department() { DepartmentID = x.DepartmentID, DepartmentName = x.DepartmentName });
					});
				}
				return departmentList;
			}
		}
		#endregion
	}
}
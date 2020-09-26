using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using TelerikWeb_WCF.BusinessLogic;

public partial class EmployeeDetail : System.Web.UI.Page
{

	int LoginUserId = 1; //demo userid 

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			FillDepartment(cmbDepartment);
		}
		lblError.Text = string.Empty;
		lblError.ForeColor = System.Drawing.Color.Red;
	}

	#region grdEmployee_NeedDataSource
	protected void grdEmployee_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
	{
		try
		{
			int? departmentId = null;
			string employeeName = null;
			if (!string.IsNullOrWhiteSpace(cmbDepartment.SelectedValue))
			{
				departmentId = Convert.ToInt32(cmbDepartment.SelectedValue);
			}
			if (!string.IsNullOrWhiteSpace(cmbEmployee.SelectedValue))
			{
				employeeName = cmbEmployee.Text;
			}

			string columnName = null; bool orderASC = true;
			// Get Sort expression if Any
			if (grdEmployee.MasterTableView.SortExpressions.Count > 0)
			{
				GridSortExpression sortExpression = grdEmployee.MasterTableView.SortExpressions[0];
				columnName = sortExpression.FieldName;
				orderASC = sortExpression.SortOrderAsString() == "ASC";
			}

			var employeeList = EmployeeManager.GetEmployees(grdEmployee.CurrentPageIndex, grdEmployee.PageSize, departmentId, employeeName, columnName, orderASC);
			grdEmployee.DataSource = employeeList;
			if (employeeList.Count > 0)
			{
				grdEmployee.VirtualItemCount = employeeList[0].TotalCount;
			}
		}
		catch (Exception ex)
		{
			grdEmployee.DataSource = new List<Employee>();
			lblError.Text = ex.Message;
			lblError.ForeColor = System.Drawing.Color.Green;
		}
	}
	#endregion

	#region grdEmployee_ItemCommand
	protected void grdEmployee_ItemCommand(object sender, GridCommandEventArgs e)
	{

		try
		{
			if (e.Item == null)
				return;

			switch (e.CommandName)
			{
				case RadGrid.InitInsertCommandName:
				case RadGrid.EditCommandName:
					CloseDetailFormIfExpanded();
					break;
				case RadGrid.PerformInsertCommandName:
					if (AddUpdateEmployee(e, true))
					{
						CloseDetailFormIfExpanded();
					}
					else
					{
						e.Canceled = true;
					}
					break;
				case RadGrid.UpdateCommandName:
					if (AddUpdateEmployee(e, false))
					{
						CloseDetailFormIfExpanded();
					}
					else
					{
						e.Canceled = true;
					}
					break;
				case RadGrid.DeleteCommandName:
					CloseDetailFormIfExpanded();
					var id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EmployeeId"];
					if (id != null)
					{
						EmployeeManager.DeleteEmployee(Convert.ToInt32(id), LoginUserId);
					}
					break;
			}
		}
		catch (Exception ex)
		{
			lblError.Text = ex.Message;
			lblError.ForeColor = System.Drawing.Color.Green;
		}
	}
	#endregion


	#region grdEmployee_ItemDataBound
	protected void grdEmployee_ItemDataBound(object sender, GridItemEventArgs e)
	{
		try
		{
			if (e.Item.IsInEditMode && e.Item is GridEditFormItem)
			{
				var editForm = (GridEditFormItem)e.Item;
				var btnAdd = (Button)editForm.FindControl("btnAdd");

				var cmbGender = (RadComboBox)editForm.FindControl("cmbGender");
				cmbGender.Items.Add(new RadComboBoxItem() { Value = "M", Text = "Male" });
				cmbGender.Items.Add(new RadComboBoxItem() { Value = "F", Text = "Female" });

				var cmbDepartmentEdit = (RadComboBox)editForm.FindControl("cmbDepartmentEdit");

				var employee = e.Item.DataItem as Employee;
				if (employee != null)
				{
					var txtName = (RadTextBox)editForm.FindControl("txtName");
					var txtEmail = (RadTextBox)editForm.FindControl("txtEmail");
					var txtSalary = (RadNumericTextBox)editForm.FindControl("txtSalary");
					var txtAddress = (RadTextBox)editForm.FindControl("txtAddress");

					txtName.Text = employee.EmployeeName;
					txtEmail.Text = employee.Email;
					txtSalary.Text = Convert.ToString(employee.Salary);
					txtAddress.Text = employee.Address;


					FillDepartment(cmbDepartmentEdit, employee.DepartmentId);

					if (employee.Gender != null)
					{
						cmbGender.SelectedValue = employee.Gender;
					}

					btnAdd.Text = "Update";
					btnAdd.CommandName = "Update";
				}
				else
				{
					btnAdd.Text = "Add";
					btnAdd.CommandName = "PerformInsert";
					FillDepartment(cmbDepartmentEdit);
				}

			}

		}
		catch (Exception ex)
		{
			lblError.Text = ex.Message;
			lblError.ForeColor = System.Drawing.Color.Green;
		}
	}
	#endregion

	private void CloseDetailFormIfExpanded()
	{
		foreach (GridItem item in grdEmployee.EditItems)
		{
			item.Edit = false;
		}

		if (grdEmployee.MasterTableView.IsItemInserted)
		{
			var insertItem = grdEmployee.MasterTableView.GetInsertItem();
			if (insertItem != null)
			{
				insertItem.Edit = false;
			}
		}

	}

	#region AddUpdateEmployee
	private bool AddUpdateEmployee(GridCommandEventArgs e, bool isInsert)
	{

		var errors = new StringBuilder();

		try
		{
			var txtName = (RadTextBox)e.Item.FindControl("txtName");
			var txtEmail = (RadTextBox)e.Item.FindControl("txtEmail");
			var txtSalary = (RadNumericTextBox)e.Item.FindControl("txtSalary");
			var cmbGender = (RadComboBox)e.Item.FindControl("cmbGender");
			var txtAddress = (RadTextBox)e.Item.FindControl("txtAddress");
			var cmbDepartmentEdit = (RadComboBox)e.Item.FindControl("cmbDepartmentEdit");

			if (string.IsNullOrWhiteSpace(txtName.Text))
			{
				errors.Append("* Please Enter Employee Name</br>");
			}

			if (string.IsNullOrWhiteSpace(txtSalary.Text))
			{
				errors.Append("* Please Enter Salary</br>");
			}

			if (string.IsNullOrWhiteSpace(cmbDepartmentEdit.SelectedValue))
			{
				errors.Append("* Please Select Department</br>");
			}

			if (errors.Length > 0)
			{
				lblError.Text = errors.ToString();
				return false;
			}


			var employee = new Employee();
			if (!isInsert)
			{
				var id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["EmployeeId"];
				employee.EmployeeId = Convert.ToInt32(id);
			}

			employee.EmployeeName = txtName.Text;
			employee.Email = txtEmail.Text;
			employee.Salary = Convert.ToInt32(txtSalary.Text);
			employee.DepartmentId = Convert.ToInt32(cmbDepartmentEdit.SelectedValue);
			employee.Address = txtAddress.Text;
			if (!string.IsNullOrWhiteSpace(cmbGender.SelectedValue))
			{
				employee.Gender = cmbGender.SelectedValue;
			}
			employee.Createdby = LoginUserId;

			EmployeeManager.UpdateEmployee(employee);

			lblError.Text = (isInsert ? "Inserted" : "Updated") + " Successfully";
			lblError.ForeColor = System.Drawing.Color.Green;
			return true;
		}
		catch (Exception ex)
		{
			lblError.Text = ex.Message;
		}
		return false;
	}
	#endregion


	#region FillDepartment
	private void FillDepartment(RadComboBox comboBox, int? selectedDepartmentId = null)
	{
		//Fill Department
		var departments = EmployeeManager.GetDepartment();
		foreach (var item in departments)
		{
			comboBox.Items.Add(new RadComboBoxItem()
			{
				Value = item.DepartmentID.ToString(),
				Text = item.DepartmentName,
				Selected = selectedDepartmentId.HasValue && selectedDepartmentId == item.DepartmentID
			});
		}
	}


	#endregion

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		grdEmployee.Rebind();
	}


	[WebMethod]
	public static RadComboBoxData GetEmployeeList(RadComboBoxContext context)
	{
		RadComboBoxData comboData = new RadComboBoxData();
		try
		{

			string keyword = context.Text;
			int pageFrom = context.NumberOfItems;
			int pageSize = 10;

			int? departmentId = null;
			if (context.ContainsKey("DepartmentId") && context["DepartmentId"] != null)
			{
				departmentId = Convert.ToInt32(context["DepartmentId"]);
			}

			var employees = EmployeeManager.GetEmployees(pageFrom, pageSize, departmentId, context.Text, null, true);

			if (employees != null)
			{
				List<RadComboBoxItemData> result = new List<RadComboBoxItemData>();

				foreach (var item in employees)
				{
					RadComboBoxItemData itemData = new RadComboBoxItemData();
					itemData.Text = item.EmployeeName;
					itemData.Value = Convert.ToString(item.EmployeeId);
					result.Add(itemData);
				}

				int totalCount = employees[0].TotalCount;
				int itemOffset = context.NumberOfItems;
				int endOffset = itemOffset + totalCount;
				comboData.EndOfItems = totalCount < pageSize;
				//comboData.Message = GetStatusMessagecustom(endOffset);
				comboData.Items = result.ToArray();
			}
			return comboData;
		}
		catch (Exception ex)
		{
			return comboData;
		}
	}



}
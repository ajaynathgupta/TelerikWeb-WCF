<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeDetail.aspx.cs" Inherits="EmployeeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <script type="text/javascript">
            
            var employeeControl = function () {
                return {
                    cmbDepartment: "<%= cmbDepartment.ClientID%>",
                    cmbEmployee: "<%= cmbEmployee.ClientID%>"
                }
            }();

            function cmbEmployee_OnClientItemsRequesting(sender, eventArgs) {

                var departmentId = null;
                var cmbDepartment = $find(employeeControl.cmbDepartment);
                if (cmbDepartment.get_value()) {
                    departmentId = cmbDepartment.get_value();
                }

                var context = eventArgs.get_context();
                context["DepartmentId"] = departmentId;
            }

            function btnClear_OnClientClicking(sender,eventArgs) {

                var cmbDepartment = $find(employeeControl.cmbDepartment);
                cmbDepartment.trackChanges();
                cmbDepartment.clearSelection();
                cmbDepartment.commitChanges();

                var cmbEmployee = $find(employeeControl.cmbEmployee);
                cmbEmployee.trackChanges();
                cmbEmployee.clearSelection();
                cmbEmployee.commitChanges();

                eventArgs.set_cancel(true);
            }

            function validateEmployee() {

                debugger;
                var tblEmployeeEdit = $('table #tblEmployeeEdit');
                var errors =[];

                var txtName = tblEmployeeEdit.find('[id$=txtName]');
                var txtSalary = tblEmployeeEdit.find('[id$=txtSalary]');
                var cmbDepartmentEdit = $find(tblEmployeeEdit.find('[id$=cmbDepartmentEdit]')[0].id);

                if (!txtName.val()) {
                    errors.push('Please Enter Name');
                }

                if (!txtSalary.val()) {
                    errors.push('Please Enter Salary');
                }

                if (!cmbDepartmentEdit.get_value()) {
                    errors.push('Please Select Department');
                }

                var ulEmployeeEditError = $('#ulEmployeeEditError')
                ulEmployeeEditError.empty();
                if (errors.length) {

                    errors.forEach(function (e) {

                        ulEmployeeEditError.append('<li>' + e + '</li>')
                    });
                    return false;
                }

                return true;
            }

        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <div id="divheader">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <table cellpadding="3" cellspacing="3" width="50%">
                    <colgroup>
                        <col width="30%" />
                        <col width="70%" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Department Name :"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbDepartment" EmptyMessage="Select" runat="server" Width="200px">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Employee Name :"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cmbEmployee" runat="server" Width="200px" AutoPostBack="false" EnableAutomaticLoadOnDemand="true"
                                OnClientItemsRequesting="cmbEmployee_OnClientItemsRequesting" EmptyMessage="Select" AllowCustomText="true">
                                <WebServiceSettings Method="GetEmployeeList" Path="EmployeeDetail.aspx"></WebServiceSettings>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            <telerik:RadButton ID="btnSearch" runat ="server" Text="Search" OnClick="btnSearch_Click"></telerik:RadButton>
                            <telerik:RadButton ID="btnClear" runat ="server" Text="Clear" OnClientClicking ="btnClear_OnClientClicking"></telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </div>

            <telerik:RadGrid ID="grdEmployee" runat="server" OnNeedDataSource="grdEmployee_NeedDataSource" AllowPaging="True"
                AutoGenerateColumns="False" OnItemCommand="grdEmployee_ItemCommand" OnItemDataBound="grdEmployee_ItemDataBound"
                CellSpacing="-1" GridLines="Both" AllowSorting="true" >
                <MasterTableView CommandItemDisplay="Top" DataKeyNames="EmployeeId,EmployeeName,DepartmentId,DepartmentName,Email,Address,Gender,Salary">
                    <CommandItemTemplate>
                        <div>
                            <asp:LinkButton ID="lnkAdd" runat="server" CommandName="InitInsert" Text="+" Font-Size="XX-Large" Height="30px"></asp:LinkButton>
                        </div>
                    </CommandItemTemplate>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <ul id="ulEmployeeEditError" style="color:red"></ul>
                            <table id="tblEmployeeEdit" cellspacing="3" cellpadding="3">
                                <colgroup>
                                    <col width="50%" />
                                    <col width="50%" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtName" runat="server" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Department :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cmbDepartmentEdit" EmptyMessage="Select" runat="server" Width="200px">
                                         </telerik:RadComboBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Email :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Gender :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cmbGender" EmptyMessage="Select" runat="server" Width="200px">
                                         </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Salary :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadNumericTextBox ID="txtSalary" NumberFormat-DecimalDigits="0" runat="server" Width="200px"></telerik:RadNumericTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Address :"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtAddress" runat="server" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="PerformInsert" OnClientClick="return validateEmployee();" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" Text="Cancel" CommandName="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </FormTemplate>
                    </EditFormSettings>
                    <Columns>
                        <telerik:GridEditCommandColumn EditText="Edit" UniqueName="EditCommandColumn">
                        </telerik:GridEditCommandColumn>
                        <telerik:GridBoundColumn DataField="EmployeeName" HeaderText="EmployeeName" UniqueName="EmployeeName" SortExpression="EmployeeName"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DepartmentName" HeaderText="DepartmentName" UniqueName="DepartmentName" SortExpression="DepartmentName"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Email" HeaderText="Email" UniqueName="Email" SortExpression="Email"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Gender" HeaderText="Gender" UniqueName="Gender" SortExpression="Gender"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Salary" HeaderText="Salary" UniqueName="Salary" SortExpression="Salary"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Address" HeaderText="Address" UniqueName="Address" SortExpression="Address"></telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" ConfirmText="Are you sure you want to delete this record?"></telerik:GridButtonColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>


        </div>
    </form>
</body>
</html>

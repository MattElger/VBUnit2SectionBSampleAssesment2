Imports System.IO

Public Class frmCustomers
    Private Structure Customers
        Public CustomerID As String
        Public FirstName As String
        Public LastName As String
        Public Address1 As String
        Public Address2 As String
        Public Postcode As String                  'Creating the structure that will hold the data.
        Public DOB As String
    End Structure
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click


        Dim MissingData As Integer 'if you haven't entered any data into the textboxes it will be added to missing data variable to later do a presence check.
        If txtFirstName.Text.Replace(" ", "") = "" Then MissingData = MissingData + 1
        If txtAddress1.Text.Replace(" ", "") = "" Then MissingData = MissingData + 1
        If txtAddress2.Text = "" Then MissingData = MissingData + 1
        If CStr(txtPostcode.Text.Replace(" ", "")) = "" Then MissingData = MissingData + 1
        If txtLastName.Text.Replace(" ", "") = "" Then MissingData = MissingData + 1
        If MissingData >= 1 Then 'presence check on the data input by the user
            MsgBox("You are missing " & MissingData & " items of data.")
            Exit Sub
        End If

        If txtAddress1.Text.Length > 30 Then
            MsgBox("The Address Line 1 you have entered is too long!")
            Exit Sub
        End If
        If txtAddress2.Text.Length > 30 Then
            MsgBox("The Address Line 2 you have entered is too long!")
            Exit Sub
        End If
        If CStr(txtPostcode.Text.Replace(" ", "")).Length > 7 Then
            MsgBox("The postcode you have entered is too long!")
            Exit Sub
        ElseIf CStr(txtPostcode.Text.Replace(" ", "")).Length < 7 Then
            MsgBox("The postcode you have entered is too short!")
            Exit Sub
        End If
        If txtFirstName.Text.Length > 30 Then
            MsgBox("The first name you have entered is too long!")
            Exit Sub
        End If
        If txtLastName.Text.Length > 30 Then
            MsgBox("The last name you have entered is too long!")
            Exit Sub
        End If









        Dim CustomerDataCheck() As String = File.ReadAllLines(Dir$("Customers.txt"))
        'checks if the data entered by the user is already a record in the database.
        For I = 0 To UBound(CustomerDataCheck)
            Dim CountGot As Integer
            If Trim(Mid(CustomerDataCheck(I), 6, 30)) = txtFirstName.Text Then CountGot = CountGot + 1 'Checks eack item of data entered by the user.
            If Trim(Mid(CustomerDataCheck(I), 36, 30)) = txtLastName.Text Then CountGot = CountGot + 1
            If Trim(Mid(CustomerDataCheck(I), 66, 30)) = txtAddress1.Text Then CountGot = CountGot + 1
            If Trim(Mid(CustomerDataCheck(I), 96, 30)) = txtAddress2.Text Then CountGot = CountGot + 1
            If Trim(Mid(CustomerDataCheck(I), 126, 20)) = dtpDOB.Text Then CountGot = CountGot + 1
            If Trim(Mid(CustomerDataCheck(I), 146, 10)) = txtPostcode.Text Then CountGot = CountGot + 1
            If Trim(Mid(CustomerDataCheck(I), 1, 5)) = txtCustomerID.Text Then CountGot = CountGot + 1
            If CountGot = 7 Then 'If all 7 items of data are the same as an already stored record the user is notified and the record is not saved twice.
                MsgBox("Record is already stored on out system.")
                Exit Sub
            End If
            If CInt(Trim(Mid(CustomerDataCheck(I), 1, 5))) >= CInt(txtCustomerID.Text) Then
                txtCustomerID.Text = Trim(Mid(CustomerDataCheck(I), 1, 5)) + 1
            End If
        Next I




        Dim CustomerData As New Customers
        Dim sw As New System.IO.StreamWriter("Customers.txt", True)
        CustomerData.CustomerID = LSet(txtCustomerID.Text, 5)
        CustomerData.FirstName = LSet(txtFirstName.Text, 30)
        CustomerData.LastName = LSet(txtLastName.Text, 30)
        CustomerData.Address1 = LSet(txtAddress1.Text, 30)
        CustomerData.Address2 = LSet(txtAddress2.Text, 30)
        CustomerData.Postcode = LSet(txtPostcode.Text, 10) 'Filling the structure with data.
        CustomerData.DOB = LSet(dtpDOB.Text, 20)

        sw.WriteLine(CustomerData.CustomerID & CustomerData.FirstName & CustomerData.LastName & CustomerData.Address1 & CustomerData.Address2 & CustomerData.DOB & CustomerData.Postcode)
        sw.Close()                                                                  'Always need to close afterwards
        MsgBox("File Saved!")




    End Sub

    Private Sub frmCustomers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Dir$("Customers.txt") = "" Then
            Dim sw As New StreamWriter("Customers.txt", True)    'This makes sure there is actually a database to enter/read data. If not, it creates a new blank one.
            sw.WriteLine("")
            sw.Close()
            MsgBox("A new file has been created", vbExclamation, "Warning!")
        End If
    End Sub

    Private Sub btnRetrieve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRetrieve.Click
        btnRetrieve.Visible = False
        btnBack.Visible = True
        cmbCustomerID.Visible = True
        btnSave.Visible = False
        txtAddress1.Enabled = False
        txtAddress2.Enabled = False
        txtPostcode.Enabled = False
        txtFirstName.Enabled = False
        txtLastName.Enabled = False
        dtpDOB.Enabled = False
        Dim CustomerData() As String = File.ReadAllLines(Dir$("Customers.txt")) 'gets the data inside the text box linked to the program.
        For I = 0 To UBound(CustomerData) 'retrives all the records client ids stored inside the text document and outputs them in a combo box for the user to select a record from.
            cmbCustomerID.Items.Add(Trim(Mid(CustomerData(I), 1, 5)))
        Next I
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        btnRetrieve.Visible = True
        btnBack.Visible = False
        cmbCustomerID.Visible = False
        btnSave.Visible = True
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtPostcode.Text = ""
        dtpDOB.Value = Date.Now
        txtAddress1.Enabled = True
        txtAddress2.Enabled = True
        txtPostcode.Enabled = True
        txtFirstName.Enabled = True
        txtLastName.Enabled = True
        dtpDOB.Enabled = True
        cmbCustomerID.Items.Clear()


        Dim CustomerDataCheck() As String = File.ReadAllLines(Dir$("Customers.txt"))
        'checks if the data entered by the user is already a record in the database.
        For I = 0 To UBound(CustomerDataCheck)
            If CInt(Trim(Mid(CustomerDataCheck(I), 1, 5))) >= CInt(txtCustomerID.Text) Then
                txtCustomerID.Text = Trim(Mid(CustomerDataCheck(I), 1, 5)) + 1
            End If
        Next


    End Sub

    Private Sub cmbCustomerID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCustomerID.SelectedIndexChanged

        Dim CustomerData() As String = File.ReadAllLines(Dir$("Customers.txt")) 'gets the data inside the text box linked to the program.
        For I = 0 To UBound(CustomerData) 'retrives all the records client ids stored inside the text document and outputs them in a combo box for the user to select a record from.
            If Trim(Mid(CustomerData(I), 1, 5)) = cmbCustomerID.SelectedItem Then
                txtFirstName.Text = Trim(Mid(CustomerData(I), 6, 30))
                txtLastName.Text = Trim(Mid(CustomerData(I), 36, 30))
                txtAddress1.Text = Trim(Mid(CustomerData(I), 66, 30))
                txtAddress2.Text = Trim(Mid(CustomerData(I), 96, 30))
                txtPostcode.Text = Trim(Mid(CustomerData(I), 146, 10))
                If Trim(Mid(CustomerData(I), 106, 20)) = "" Then
                Else
                    dtpDOB.Value = Trim(Mid(CustomerData(I), 126, 20)) & " 12:00 AM"
                End If
                txtCustomerID.Text = Trim(Mid(CustomerData(I), 1, 5))
            End If
        Next I



    End Sub
End Class
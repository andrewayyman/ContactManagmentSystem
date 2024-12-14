open System
open System.Windows.Forms


// ------------------------- Form ------------------------- //
// Create the form
let form: Form = new Form(Text = "Contact Management System", Width = 600, Height = 450)

// UI elements
let nameLabel: Label = new Label(Text = "Name:", Top = 20, Left = 10)
let nameBox: TextBox = new TextBox(Top = 20, Left = 110, Width = 200)
let phoneLabel: Label = new Label(Text = "Phone:", Top = 60, Left = 10)
let phoneBox: TextBox = new TextBox(Top = 60, Left = 110, Width = 200)
let emailLabel: Label = new Label(Text = "Email:", Top = 100, Left = 10)
let emailBox: TextBox = new TextBox(Top = 100, Left = 110, Width = 200)
let searchBox: TextBox = new TextBox(Top = 170, Left = 100, Width = 200)
let searchButton: Button = new Button(Text = "Search", Top = 170, Left = 310)
let addButton: Button = new Button(Text = "Add Contact", Top = 140, Left = 100, Width = 100)
let deleteButton: Button = new Button(Text = "Delete Contact", Top = 140, Left = 400, Width = 100)
let updateButton: Button = new Button(Text = "Update Contact", Top = 140, Left = 200, Width = 100)
let saveButton: Button = new Button(Text = "Save Changes", Top = 140, Left = 300, Width = 100)
let contactList: ListBox = new ListBox(Top = 200, Left = 10, Width = 560, Height = 150)


// ensure that phone number only have 11 digit
phoneBox.KeyPress.Add(fun e ->
    if not (Char.IsDigit(e.KeyChar) || e.KeyChar = '\b') then
        e.Handled <- true
    elif phoneBox.Text.Length = 11 && e.KeyChar <> '\b' then
        e.Handled <- true
)


 // ensure that Email contains @ 
let isValidEmail (email: string): bool =
    email.Contains("@") 


// Add ui elements to the form
form.Controls.Add(nameLabel)
form.Controls.Add(nameBox)
form.Controls.Add(phoneLabel)
form.Controls.Add(phoneBox)
form.Controls.Add(emailLabel)
form.Controls.Add(emailBox)
form.Controls.Add(addButton)
form.Controls.Add(deleteButton)
form.Controls.Add(updateButton)
form.Controls.Add(saveButton)
form.Controls.Add(searchBox)
form.Controls.Add(searchButton)
form.Controls.Add(contactList)
// ------------------------- Form ------------------------- //


// Run the application
[<STAThread>]
Application.Run(form)









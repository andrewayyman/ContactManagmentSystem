open System
open System.Windows.Forms
open System.Drawing

// ------------------------- Form ------------------------- //
// Create the form
let form: Form = new Form(Text = "Contact Management System", Width = 800, Height = 650)
form.BackColor <- Color.White
form.StartPosition <- FormStartPosition.CenterScreen
form.FormBorderStyle <- FormBorderStyle.FixedDialog
form.MaximizeBox <- false
form.MinimizeBox <- false

// Set title label at the top
let titleLabel: Label = new Label(Text = "Contact Management System", Top = 10, Left = 10, Width = 760, Font = new Font("Segoe UI", 14.0f, FontStyle.Bold))
titleLabel.TextAlign <- ContentAlignment.MiddleCenter
titleLabel.AutoSize <- false

// UI elements styles
let nameLabel: Label = new Label(Text = "Name:", Top = 60, Left = 20, Width = 80, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), ForeColor = Color.DarkSlateGray)
let nameBox: TextBox = new TextBox(Top = 60, Left = 110, Width = 250, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), BackColor = Color.LightGray, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle)
nameBox.Padding <- new Padding(10)
nameBox.CharacterCasing <- CharacterCasing.Normal  

let phoneLabel: Label = new Label(Text = "Phone:", Top = 100, Left = 20, Width = 80, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), ForeColor = Color.DarkSlateGray)
let phoneBox: TextBox = new TextBox(Top = 100, Left = 110, Width = 250, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), BackColor = Color.LightGray, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle)
phoneBox.Padding <- new Padding(10)
phoneBox.CharacterCasing <- CharacterCasing.Normal  

let emailLabel: Label = new Label(Text = "Email:", Top = 140, Left = 20, Width = 80, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), ForeColor = Color.DarkSlateGray)
let emailBox: TextBox = new TextBox(Top = 140, Left = 110, Width = 250, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), BackColor = Color.LightGray, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle)
emailBox.Padding <- new Padding(10)
emailBox.CharacterCasing <- CharacterCasing.Normal  

let searchBox: TextBox = new TextBox(Top = 210, Left = 110, Width = 250, Font = new Font("Segoe UI", 12.0f, FontStyle.Bold), BackColor = Color.LightGray, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle)
searchBox.Padding <- new Padding(10)

// Search Button
let searchButton: Button = new Button(Text = "Search", Top = 210, Left = 370, Width = 120, Height = 40, Font = new Font("Segoe UI", 10.0f), BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat)
searchButton.FlatAppearance.BorderSize <- 0
searchButton.Cursor <- Cursors.Hand

// Buttons styles
let addButton: Button = new Button(Text = "Add Contact", Top = 270, Left = 110, Width = 120, Height = 40, Font = new Font("Segoe UI", 10.0f), BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat)
let deleteButton: Button = new Button(Text = "Delete Contact", Top = 270, Left = 240, Width = 120, Height = 40, Font = new Font("Segoe UI", 10.0f), BackColor = Color.LightCoral, FlatStyle = FlatStyle.Flat)
let updateButton: Button = new Button(Text = "Update Contact", Top = 270, Left = 370, Width = 120, Height = 40, Font = new Font("Segoe UI", 10.0f), BackColor = Color.LightYellow, FlatStyle = FlatStyle.Flat)
let saveButton: Button = new Button(Text = "Save Changes", Top = 270, Left = 500, Width = 120, Height = 40, Font = new Font("Segoe UI", 10.0f), BackColor = Color.LightSteelBlue, FlatStyle = FlatStyle.Flat)

// ListBox for contact list
let contactList: ListBox = new ListBox(Top = 320, Left = 20, Width = 740, Height = 250, Font = new Font("Segoe UI", 10.0f))

// Styling for buttons and inputs
[addButton; deleteButton; updateButton; saveButton; searchButton]
    |> List.iter (fun button -> button.FlatAppearance.BorderSize <- 0)

[addButton; deleteButton; updateButton; saveButton; searchButton]
    |> List.iter (fun button -> button.Cursor <- Cursors.Hand)

// Ensure that phone number only has 11 digits
phoneBox.KeyPress.Add(fun e ->
    if not (Char.IsDigit(e.KeyChar) || e.KeyChar = '\b') then
        e.Handled <- true
    elif phoneBox.Text.Length = 11 && e.KeyChar <> '\b' then
        e.Handled <- true
)

// Ensure that Email contains "@"
let isValidEmail (email: string): bool =
    email.Contains("@")

// Add UI elements to the form
form.Controls.Add(titleLabel)
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
// ------------------------- End Form ------------------------- //


// ------------------------- AddContact ------------------------- //
//contact type
type Contact = {
    Name: string
    PhoneNumber: string
    Email: string
}

// List to store contacts
let contacts: Contact list ref = ref []

// Function to add a contact
let addContact (contact: Contact) : unit =
    contacts := contact :: !contacts

// Function to update the ListBox with contacts after adding
let updateContactList () : unit =
    contactList.Items.Clear()
    !contacts
    |> List.iter (fun c -> 
        contactList.Items.Add($"Name: {c.Name}, Phone: {c.PhoneNumber}, Email: {c.Email}") |> ignore
    )
// ------------------------- End AddContact ------------------------- //


// ------------------------- DeleteContact ------------------------- //
let deleteContact () : unit =
    match contactList.SelectedIndex with
    | idx when idx >= 0 ->
        match !contacts |> List.tryItem idx with
        | Some selectedContact ->
            contacts := List.filter (fun c -> c <> selectedContact) !contacts
            updateContactList()
            MessageBox.Show("Contact deleted successfully.") |> ignore
        | None -> MessageBox.Show("Selected contact is invalid.") |> ignore
    | _ -> MessageBox.Show("Please select a contact to delete!") |> ignore
// ------------------------- End DeleteContact ------------------------- //


//-------------------------- SearchContact ------------------------------//
let searchContacts (query: string) : unit =
    let results = 
        !contacts 
        |> List.filter (fun c -> 
            c.Name.ToLower().Contains(query.ToLower()) || c.PhoneNumber.ToLower().Contains(query.ToLower())
        )
    contactList.Items.Clear()
    results
    |> List.iter (fun c -> 
        contactList.Items.Add($"Name: {c.Name}, Phone: {c.PhoneNumber}, Email: {c.Email}") |> ignore
    )
// ------------------------- End SearchContact ------------------------- //


//-------------------------- UpdateConntact ------------------------------//
let updateContact () : unit =
    match contactList.SelectedIndex with
    | idx when idx >= 0 ->
        match !contacts |> List.tryItem idx with
        | Some selectedContact ->
            // Populate textboxes with selected contact's details
            nameBox.Text <- selectedContact.Name
            phoneBox.Text <- selectedContact.PhoneNumber
            emailBox.Text <- selectedContact.Email
        | None -> MessageBox.Show("Selected contact is invalid.") |> ignore
    | _ -> MessageBox.Show("Please select a contact to update!") |> ignore
// save updated contact
let saveUpdatedContact () : unit =
    match contactList.SelectedIndex with
    | idx when idx >= 0 ->
        let updatedContact = { Name = nameBox.Text; PhoneNumber = phoneBox.Text; Email = emailBox.Text }
        if not (isValidEmail updatedContact.Email) then
            MessageBox.Show(" valid email address is must have this format (anything@mail.com)") |> ignore
        elif updatedContact.PhoneNumber.Length > 11 then
            MessageBox.Show("Phone number must be 11 digits or less!") |> ignore
        else
            contacts := 
                !contacts
                |> List.mapi (fun i c -> if i = idx then updatedContact else c)
            updateContactList()
            MessageBox.Show("Contact updated successfully.") |> ignore
            nameBox.Clear()
            phoneBox.Clear()
            emailBox.Clear()
    |  _-> MessageBox.Show("Please select a contact to save changes!") |> ignore 
// ------------------------- End UpdateConntact ------------------------- //



// ------------------------- EventHandlers ------------------------- //
// on add button
addButton.Click.Add (fun _ ->
    let name: string = nameBox.Text
    let phone: string = phoneBox.Text
    let email: string = emailBox.Text
    if name <> "" && phone <> "" && email <> "" then
        if not (isValidEmail email) then
            MessageBox.Show("Please enter a valid email address!") |> ignore
        elif phone.Length > 11 then
            MessageBox.Show("Phone number must be 11 digits !") |> ignore
        else
            let newContact: Contact = { Name = name; PhoneNumber = phone; Email = email }
            addContact newContact
            updateContactList ()
            MessageBox.Show($"Contact : {name} added successfully") |> ignore
            nameBox.Clear()
            phoneBox.Clear()
            emailBox.Clear()
    else
        MessageBox.Show("Please fill all fields!") |> ignore
)

// on Delete button 
deleteButton.Click.Add(fun _ -> deleteContact ())

// on search button 
searchButton.Click.Add(fun _ -> 
    let query = searchBox.Text
    if query <> "" then
        searchContacts query
    else
        MessageBox.Show("Please enter a search term.") |> ignore
)

// Update button click
updateButton.Click.Add(fun _-> updateContact ())

// Save button click
saveButton.Click.Add(fun  _-> saveUpdatedContact ())


[<STAThread>]
Application.Run(form)



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

// Run the application
[<STAThread>]
Application.Run(form)



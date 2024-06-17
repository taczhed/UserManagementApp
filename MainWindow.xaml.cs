using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using UserManagementApp.Classes;


namespace UserManagementApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<User> Users;

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            Users = new ObservableCollection<User>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Users";
                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Address = reader.GetString(3),
                            PhoneNumber = reader.GetString(4),
                            Email = reader.GetString(5)
                        });
                    }
                }
            }

            UsersDataGrid.ItemsSource = Users;
            ClearInputs();

        }

        private void ClearInputs()
        {
            NameTextBox.Text = "";
            SurnameTextBox.Text = "";
            AddressTextBox.Text = "";
            PhoneNumberTextBox.Text = "";
            EmailTextBox.Text = "";
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string insertQuery = "INSERT INTO Users (Name, Surname, Address, PhoneNumber, Email) VALUES (@Name, @Surname, @Address, @PhoneNumber, @Email)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@Surname", SurnameTextBox.Text);
                    command.Parameters.AddWithValue("@Address", AddressTextBox.Text);
                    command.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);
                    command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                    command.ExecuteNonQuery();
                }
            }
            LoadUsers();
            
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string updateQuery = "UPDATE Users SET Name = @Name, Surname = @Surname, Address = @Address, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedUser.Id);
                        command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                        command.Parameters.AddWithValue("@Surname", SurnameTextBox.Text);
                        command.Parameters.AddWithValue("@Address", AddressTextBox.Text);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTextBox.Text);
                        command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                        command.ExecuteNonQuery();
                    }
                }
                LoadUsers();
            }
        }

        // Done
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Users WHERE Id = @Id";
                    using (var command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", selectedUser.Id);
                        command.ExecuteNonQuery();
                    }
                }
                LoadUsers();

            }
        }

        //Done
        private void UsersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                DeleteButton.IsEnabled = true;
                EditButton.IsEnabled = true;

                NameTextBox.Text = selectedUser.Name;
                SurnameTextBox.Text = selectedUser.Surname;
                AddressTextBox.Text = selectedUser.Address;
                PhoneNumberTextBox.Text = selectedUser.PhoneNumber;
                EmailTextBox.Text = selectedUser.Email;
            }
            else
            {
                DeleteButton.IsEnabled = false;
                EditButton.IsEnabled = false;
            }

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}

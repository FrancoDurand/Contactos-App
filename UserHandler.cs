using Microsoft.VisualBasic.FileIO;
using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Contactos {
	class UserHandler {
		private MySqlCommand command;
		private MySqlConnection connection;
		public User user;

		private string table = "user";
		private string values = "name, email, pass, hash_pass";
		public UserHandler(DatabaseHandler db) { connection = db.connection; }
		public void addUser(User user) {
			//string query = $"insert into {table} ({values}) values ('{user.name}','{user.email}','{user.password}','{Hashier.getHash(user.password)}')";
			string query = $"insert into {table} ({values}) values (@name, @email, @password, @hash)";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@name", user.name);
			command.Parameters.AddWithValue("@email", user.email);
			command.Parameters.AddWithValue("@password", user.password);
			command.Parameters.AddWithValue("@hash", Hashier.getHash(user.password));

			command.ExecuteNonQuery();
		}
		public bool login(string email, string password) {
			string query = $"select * from {table} where email = '{email}'";
			command = new MySqlCommand(query, connection);

			using (MySqlDataReader reader = command.ExecuteReader()) {
				if (!reader.Read()) // Verificar si hay alguna fila en el resultado
					return false;

				for (int i = 0; i < reader.FieldCount; i++)
					if (reader.GetName(i) == "hash_pass" && Hashier.getHash(password) == reader.GetString(i)) {
						updateUser(int.Parse(reader.GetString("id")), reader.GetString("name"), email);
						return true;
					}
			}

			return false;
		}

		private void updateUser(int id, string name, string email) {
			user = new User(id, name, email);
		}
		public void updatePassword(string password) {
			string query = $"update {table} set pass = @pass, hash_pass = @hash_pass where id = @id";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@pass", password);
			command.Parameters.AddWithValue("@hash_pass", Hashier.getHash(password));
			command.Parameters.AddWithValue("@id", user.id);

			command.ExecuteNonQuery();
		}
		public void updateEmail(string email) {
			string query = $"update {table} set email = @email where id = @id";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@email", email);
			command.Parameters.AddWithValue("@id", user.id);

			command.ExecuteNonQuery();
		}
		public void updateName(string name) {
			string query = $"update {table} set name = @name where id = @id";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@name", name);
			command.Parameters.AddWithValue("@id", user.id);

			command.ExecuteNonQuery();
		}

		public void addContact(string id) {
			string query = $"insert into contact (user_id, contact_id) values (@user_id, @contact_id)";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@user_id", user.id);
			command.Parameters.AddWithValue("@contact_id", id);

			command.ExecuteNonQuery();
		}

		public void viewContactsNoAdded() {
			MySqlDataReader reader = getContactsNoAdded();

			while (reader.Read()) {
				int id = reader.GetInt32(0);
				string name = reader.GetString(1);
				string email = reader.GetString(2);

				Console.WriteLine($"ID: {id}");
				Console.WriteLine($"Nombre: {name}");
				Console.WriteLine($"Email: {email}");
				Console.WriteLine($"-----------------------------");
			}
			reader.Close();

			//NOTE: para obtener los datos con un for
			/*if (reader.Read()) {
				for (int i = 0; i < reader.FieldCount; i++) {
					string columnName = reader.GetName(i);
					object value = reader.GetValue(i);

					// Si el nombre de la columna no es "pass" ni "hash_pass", lo mostramos
					if (columnName != "pass" && columnName != "hash_pass") {
						Console.WriteLine($"{columnName}: {value}");
					}
				}
			}*/
		}

		private MySqlDataReader getContactsNoAdded() {
			/*
			SELECT *
			FROM user
			WHERE id != 1 AND NOT EXISTS (
				SELECT 1
				FROM contact 
				where contact_id = user.id and user_id = 1
			) 
			 */

			string query = $"select * from {table} where id != @id and not exists (" +
				$"select 1 from contact where contact_id = {table}.id and user_id = @id)";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@id", user.id);

			return command.ExecuteReader();
		}

		private MySqlDataReader getContacts() {
			string query = $"select u.id, u.name, u.email from {table} u left join contact c" +
				$" on c.contact_id = u.id where c.user_id = @id";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@id", user.id);

			return command.ExecuteReader();
		}

		public bool validIdToAdd(string id) {
			MySqlDataReader reader = getContactsNoAdded();

			while (reader.Read()) {
				if (reader.GetString(0) == id) {
					reader.Close();
					return true;
				}
			}

			reader.Close();
			return false;
		}

		public bool viewContacts() {
			MySqlDataReader reader = getContacts();
			bool hasRows = reader.HasRows;


			while (reader.Read()) {
				int id = reader.GetInt32(0);
				string name = reader.GetString(1);
				string email = reader.GetString(2);

				Console.WriteLine($"ID: {id}");
				Console.WriteLine($"Nombre: {name}");
				Console.WriteLine($"Email: {email}");
				Console.WriteLine($"-----------------------------");
			}
			
			reader.Close();
			return hasRows;
		}

		public bool validIdToDelete(string id) {
			MySqlDataReader reader = getContacts();

			while (reader.Read()) {
				if (reader.GetString(0) == id) {
					reader.Close();
					return true;
				}
			}

			reader.Close();
			return false;
		}

		public void deleteContact(string id) {
			string query = $"delete from contact where user_id = @user_id and contact_id = @contact_id";

			command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@user_id", user.id);
			command.Parameters.AddWithValue("@contact_id", id);

			command.ExecuteNonQuery();
		}
	}
}

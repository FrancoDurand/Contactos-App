using MySql.Data.MySqlClient;

namespace Contactos {
	class DatabaseHandler {
		private readonly string server = "bdzdifxdiebshypjfnnv-mysql.services.clever-cloud.com";
		private readonly string user = "ufbg9cqzbuh3onzj";
		private readonly string password = "KMjmbt8x1AK9X4Vd0OTq";
		private readonly string database = "bdzdifxdiebshypjfnnv";
		public MySqlConnection connection { get; private set; }
		public DatabaseHandler() {
			string stringConnection = $"server={server};uid={user};pwd={password};database={database}";
			connection = new MySqlConnection(stringConnection);
		}
		public void open() { connection.Open(); }
		public void close() { connection.Close(); }
	}
}

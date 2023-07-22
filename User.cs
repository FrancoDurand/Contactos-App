namespace Contactos {
	class User {
		public string name { get; private set; }
		public string email { get; private set; }
		public string password { get; private set; }
		public int id { get; private set; }

		public User(string name, string email, string password) {
			this.name = name;
			this.email = email;
			this.password = password;
		}

		public User(int id, string name, string email) {
			this.id = id;
			this.name = name;
			this.email = email;
		}

		public User() {
			this.name = "";
			this.email = "";
			this.password = "";
			this.id = 0;
		}
	}
}

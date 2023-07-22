using Contactos;
class Program {
	static DatabaseHandler db;
	static UserHandler userHandler;
	public static void Main() {
		db = new DatabaseHandler();
		userHandler = new UserHandler(db);

		db.open();

		menu();

		db.close();
	}

	static void menu() {
		string opc;

		do {
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("BIENVENIDO A LA APP DE CONTACTOS");
			Console.ResetColor();
			Console.WriteLine("1.Crear usuario");
			Console.WriteLine("2.Iniciar sesion");
			Console.WriteLine("0.Salir");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("OPCION: ");
			opc = Console.ReadLine();
			Console.ResetColor();

			switch (opc) {
				case "1":
					createUser();
					break;

				case "2":
					login();
					break;
				default:
					break;
			}

		} while (opc != "0");
	}

	static void createUser() {
		string name, email, password;

		do {
			Console.Clear();
			Console.Write("Nombre: ");
			name = Console.ReadLine();
		} while (name == "");

		do {
			Console.Clear();
			Console.WriteLine("Nombre: " + name);
			Console.Write("Email: ");
			email = Console.ReadLine();
		} while (email == "");

		do {
			Console.Clear();
			Console.WriteLine("Nombre: " + name);
			Console.WriteLine("Email: " + email);
			Console.Write("Password: ");
			password = Console.ReadLine();
		} while (password == "");

		User user = new User(name, email, password);
		userHandler.addUser(user);
	}

	static void login() {
		string email, password;

		do {
			Console.Clear();
			Console.Write("Email: ");
			email = Console.ReadLine();
		} while (email == "");

		do {
			Console.Clear();
			Console.WriteLine("Email: " + email);
			Console.Write("Password: ");
			password = Console.ReadLine();
		} while (password == "");

		if (userHandler.login(email, password))
			menuLogin();
		else {
			Console.WriteLine("ERROR LOGIN");
			Console.ReadKey();
		}
	}

	static void menuLogin() {
		string opc;

		do {
			Console.Clear();
			Console.WriteLine($"Bienvenido {userHandler.user.name}");
			Console.WriteLine("1.Actualizar datos");
			Console.WriteLine("2.Ver contactos");
			Console.WriteLine("3.Agregar contacto");
			Console.WriteLine("4.Eliminar contacto");
			Console.WriteLine("0.Cerrar sesion");
			opc = Console.ReadLine();

			switch (opc) {
				case "1":
					updateData();
					break;

				case "2":
					viewContacts();
					break;

				case "3":
					addContact();
					break;

				case "4":
					deleteContact();
					break;

				default:
					break;
			}
		} while (opc != "0");

		userHandler.user = new User();
	}

	static void updateData() {
		string opc, data;
		do {
			Console.Clear();
			Console.WriteLine("Dato a modificar");
			Console.WriteLine("1.Nombre");
			Console.WriteLine("2.Email");
			Console.WriteLine("3.Contraseña");
			opc = Console.ReadLine();
		} while (opc != "1" && opc != "2" && opc != "3");

		Console.Clear();

		switch (opc) {
			case "1":
				do {
					Console.Write("Nuevo nombre: ");
					data = Console.ReadLine();
				} while (data == "");
				userHandler.updateName(data);
				break;

			case "2":
				do {
					Console.Write("Nuevo email: ");
					data = Console.ReadLine();
				} while (data == "");
				userHandler.updateEmail(data);
				break;

			case "3":
				do {
					Console.Write("Nueva contraseña: ");
					data = Console.ReadLine();
				} while (data == "");
				userHandler.updatePassword(data);
				break;

			default:
				break;
		}

		Console.WriteLine("Actualizacion correcta");
		Console.ReadKey();
	}

	static void addContact() {
		string opc;
		Console.Clear();
		userHandler.viewContactsNoAdded();

		do {
			Console.Write("ID a agregar: ");
			opc = Console.ReadLine();
		} while (!userHandler.validIdToAdd(opc));

		userHandler.addContact(opc);
		Console.WriteLine("Contacto añadido");
		Console.ReadKey();
	}

	static void viewContacts() {
		if (!userHandler.viewContacts())
			Console.WriteLine("No tienes contactos agregados");
		Console.ReadKey();
	}

	static void deleteContact() {
		string opc;
		Console.Clear();
		userHandler.viewContacts();

		do {
			Console.Write("ID a eliminar: ");
			opc = Console.ReadLine();
		} while (!userHandler.validIdToDelete(opc));

		userHandler.deleteContact(opc);
		Console.WriteLine("Contacto eliminado");
		Console.ReadKey();
	}
}
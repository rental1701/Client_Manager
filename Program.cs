using Client_Manager.Models;
using Client_Manager.MyClasses;
using Client_Manager.Services;


string? numCommand = string.Empty;
bool isExit = false;

while (!isExit)
{
    while (numCommand == string.Empty)
    {
        GetStartMenu();
        numCommand = Console.ReadLine();

        switch (numCommand)
        {
            case "1":

                break;
            case "2":
                Environment.Exit(0);
                break;
        }
    }
}



void GetMainMenu()
{
    while (true)
    {
        Console.Clear();

        Console.WriteLine("=== Главное меню ===\n");

        Console.WriteLine("Введите номер команды:");
        Console.WriteLine("1 - Информация о заказах клиентов");
        Console.WriteLine("2 - Изменение контактного лица клиента");
        Console.WriteLine("3 - Определение золотого клиента");

        string command = Console.ReadLine();
        switch (command)
        {
            case "1":
                Console.Clear();
                Console.WriteLine("Введите наименование продукта");
                string? name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                {
                    string info = InfoService.GetInfoClientAndProduct(name);
                    Console.WriteLine(info);
                    Console.WriteLine("\nНажмите Enter чтобы продолжить");
                    Console.ReadLine();
                }
                else
                    Console.WriteLine("Введите товар");
                break;
            case "2":
                Console.Clear();
                Console.WriteLine("Введите Название организации");
                string? nameClient = Console.ReadLine();
                if (!string.IsNullOrEmpty(nameClient) && !string.IsNullOrWhiteSpace(nameClient))
                {
                    bool res = InfoService.FindClientByName(nameClient);
                    if (res)
                    {
                        Console.WriteLine("Введите ФИО нового контактного лица:");
                        string? nameContact = Console.ReadLine();
                        if (!string.IsNullOrEmpty(nameContact) && !string.IsNullOrWhiteSpace(nameContact))
                        {
                            var updateClient = InfoService.UpdateContactPerson(nameClient, nameContact);
                            if (updateClient is Client client)
                            {
                                Console.WriteLine("Контактное лицо обновленно\n");
                                Console.WriteLine(client.ToString());
                                Console.WriteLine("Нажмите Enter чтобы продолжить");
                                Console.ReadLine();
                            }
                            else
                                Console.WriteLine("Не удалось обновить информацию");
                        }
                    }
                }
                break;
            case "3":
                Console.Clear();
                Console.WriteLine("Введите дату в формате yyyy.MM (2020.01)");
                string? date = Console.ReadLine();
                if (!string.IsNullOrEmpty(date) && !string.IsNullOrWhiteSpace(date))
                {
                    var goldClient = InfoService.GetGoldClient(date);
                    if (goldClient is Client client)
                    {
                        Console.WriteLine("Золотым клиентом на выбранную дату является:\n");
                        Console.WriteLine(client.ToString());
                        Console.WriteLine("\nНажмите Enter чтобы продолжить");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Не удалось найти золотого клиента!");
                        Console.WriteLine("\nНажмите Enter чтобы продолжить");
                        Console.ReadLine();
                    }
                }
                break;
            default:
                break;
        }
    }
}

bool GetStartMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Добро пожаловать!\n");

        Console.WriteLine("=== Стартовое меню ===\n");

        Console.WriteLine("Введите номер команды:");
        Console.WriteLine("1 - Задать путь к данным");
        Console.WriteLine("2 - Выход");

        string? command = Console.ReadLine();
        bool isPathset = false;
        switch (command)
        {
            case "1":
                isPathset = MenuSetDbPath();
                if (isPathset)
                    GetMainMenu();
                break;
            case "2":
                Environment.Exit(0);
                return false;
            default:
                break;
        }
    }
}

bool MenuSetDbPath()
{
    while (true)
    {
        #region Меню
        Console.Clear();

        Console.WriteLine("=== Задать путь к данным ===\n");

        Console.WriteLine("Выберите команду:");
        Console.WriteLine("1 - Выбрать путь по умолчанию");
        Console.WriteLine("2 - Задать путь в ручную");
        Console.WriteLine("3 - Назад");
        #endregion

        string? numCommand = Console.ReadLine();
        bool res = false;
        switch (numCommand)
        {
            case "1":
                res = PathDB.SelectDataBasePath(true);
                if (res)
                    return true;
                break;
            case "2":
                res = PathDB.SelectDataBasePath();
                if (res)
                    return true;
                break;
            case "3":
                return false;
            default:
                break;
        }
    }
}
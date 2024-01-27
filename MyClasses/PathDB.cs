namespace Client_Manager.MyClasses
{
    internal static class PathDB
    {
        /// <summary>Путь к базе данных по умолчанию</summary>
        private static readonly string _pathDefault = "\\DataBase\\db.xlsx";


        private static string _pathToDb = string.Empty;
        /// <summary>
        /// Путь указзанный пользователем
        /// </summary>
        internal static string PathToDb
        {
            get { return _pathToDb; }
            set { _pathToDb = value; }
        }


        /// <summary>
        /// Метод задания пути к базе с данными
        /// </summary>
        /// <param name="isDefaultPath"></param>
        /// <returns></returns>
        internal static bool SelectDataBasePath(bool isDefaultPath = false)
        {
            if (!isDefaultPath)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("0 - Назад");
                    Console.WriteLine("Введите путь к данным: ");
                    string? path = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(path) && path == "0")
                    {
                        return false;
                    }
                    else if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    {
                        Console.Clear();
                        Console.WriteLine("Указанного файла не существует или введён не корректный адрес! Нажмите Enter");
                        Console.ReadLine();
                    }
                    else
                    {
                        PathToDb = path;
                        Console.WriteLine("Файл найден! Нажмите Enter чтобы продолжить");
                        Console.ReadLine();
                        return true;
                    }
                }
            }

            PathToDb = Path.Join(Directory.GetCurrentDirectory(), _pathDefault);
            if (File.Exists(PathToDb))
                return true;
            else
            {
                Console.WriteLine("Файл найден! Нажмите Enter чтобы продолжить");
                Console.ReadLine();
                return false;
            }
        }
    }
}

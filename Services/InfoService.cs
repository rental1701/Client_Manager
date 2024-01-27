using Client_Manager.Models;
using Client_Manager.MyClasses;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Globalization;
using System.Text;

namespace Client_Manager.Services
{
    internal static class InfoService
    {
        /// <summary>
        /// По наименованию товара выводить информацию о клиентах, заказавших этот товар, с указанием информации по количеству товара, цене и дате заказа.
        /// </summary>
        internal static string GetInfoClientAndProduct(string nameProduct)
        {
            using IXLWorkbook workbook = new XLWorkbook(PathDB.PathToDb);

            var product = GetProductByName(nameProduct, workbook);
            if (product != null)
            {
                var requsts = GetRequestByIdProduct(product.Id, workbook);
                if (requsts != null)
                {
                    StringBuilder mainBuilder = new();
                    foreach (var item in requsts)
                    {
                        var client = GetClientById(item.IdClient, workbook);
                        if (client != null)
                        {
                            mainBuilder.Append($"{client.ToString()}\n" +
                                $"Количество:{item.RequiredQuantity}\n" +
                                $"Цена: {item.RequiredQuantity * product.Price}\n" +
                                $"Дата заказа: {item.PostingDate.ToString("d")}\n\n");
                        }
                        return mainBuilder.ToString();
                    }
                }
            }
            return "Информация отсутствует!";
        }


        /// <summary>
        /// Поиск Товара по имени
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <returns></returns>
        internal static Product? GetProductByName(string nameProduct, IXLWorkbook workbook)
        {
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Товары");

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.Cell(2).Value.ToString().ToLower() == nameProduct.ToLower())
                {
                    return new Product()
                    {
                        Id = int.Parse(row.Cell(1).Value.ToString()),
                        Name = row.Cell(2).Value.ToString(),
                        Unit = row.Cell(3).Value.ToString(),
                        Price = int.Parse(row.Cell(4).Value.ToString())
                    };
                }
            }
            return null;
        }


        /// <summary>
        /// Поиск клиента по id
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <returns></returns>
        internal static Client? GetClientById(int idClient, IXLWorkbook? workbook = null)
        {
            if(workbook == null)
            {
                workbook = new XLWorkbook(PathDB.PathToDb);
            }
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Клиенты");

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.Cell(1).Value.ToString().ToLower() == idClient.ToString().ToLower())
                {
                    return new Client()
                    {
                        Id = idClient,
                        NameOfCompany = row.Cell(2).Value.ToString(),
                        Address = row.Cell(3).Value.ToString(),
                        ContactPerson = row.Cell(4).Value.ToString()
                    };
                }
            }
            return null;
        }


        /// <summary>
        /// Поиск заяки  по id товара
        /// </summary>
        /// <param name="nameProduct"></param>
        /// <returns></returns>
        internal static List<Request>? GetRequestByIdProduct(int idProduct, IXLWorkbook workbook)
        {
            List<Request> requests = new List<Request>();
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Заявки");

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.Cell(2).Value.ToString().ToLower() == idProduct.ToString().ToLower())
                {
                    requests.Add(new Request()
                    {
                        Id = int.Parse(row.Cell(1).Value.ToString()),
                        IdProduct = idProduct,
                        IdClient = int.Parse(row.Cell(3).Value.ToString()),
                        NumRequest = int.Parse(row.Cell(4).Value.ToString()),
                        RequiredQuantity = int.Parse(row.Cell(5).Value.ToString()),
                        PostingDate = DateTime.Parse(row.Cell(6).Value.ToString())
                    });
                }
            }
            return requests;
        }

        /// <summary>
        /// Получить все заявки
        /// </summary>
        /// <returns></returns>
        internal static List<Request> GetAllRequest()
        {
            List<Request> request = new List<Request>();
            using IXLWorkbook workbook = new XLWorkbook(PathDB.PathToDb);
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Заявки");
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                request.Add(new Request()
                {
                    Id = int.Parse(row.Cell(1).Value.ToString()),
                    IdProduct = int.Parse(row.Cell(2).Value.ToString()),
                    IdClient = int.Parse(row.Cell(3).Value.ToString()),
                    NumRequest = int.Parse(row.Cell(4).Value.ToString()),
                    RequiredQuantity = int.Parse(row.Cell(5).Value.ToString()),
                    PostingDate = DateTime.Parse(row.Cell(6).Value.ToString())
                });
            }
            return request;
        }


        /// <summary>
        /// Возвращает золотого клиента
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static Client? GetGoldClient(string date)
        {
            if (DateTime.TryParseExact(date, "yyyy.MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
            {
                var requests = GetAllRequest();
                var goldIdClient = requests.Where(r => r.PostingDate.ToString("yyyy.MM") == newDate.ToString("yyyy.MM"))
                                           .GroupBy(r => r.IdClient)
                                           .OrderByDescending(x => x.Sum(r=>r.RequiredQuantity))
                                           .Select(x=>x.Select(x=>x.IdClient).First()).First();


                return GetClientById(goldIdClient);
            }
            return null;
        }

        /// <summary>
        /// Поиск клиента по имени организации
        /// </summary>
        /// <param name="nameClient"></param>
        /// <returns></returns>
        internal static bool FindClientByName(string nameClient)
        {
            using IXLWorkbook workbook = new XLWorkbook(PathDB.PathToDb);
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Клиенты");
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.Cell(2).Value.ToString().ToLower() == nameClient.ToLower())
                {
                    return true;  
                }
            }
            return false;
        }

        /// <summary>
        /// обновление информации о контактном лице клиента
        /// </summary>
        /// <param name="nameClient"></param>
        /// <param name="newContact"></param>
        /// <returns></returns>
        internal static Client? UpdateContactPerson(string nameClient, string newContact)
        {
            using IXLWorkbook workbook = new XLWorkbook(PathDB.PathToDb);
            IXLWorksheet worksheet = workbook.Worksheets.First(x => x.Name == "Клиенты");
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                if (row.Cell(2).Value.ToString().ToLower() == nameClient.ToLower())
                {
                    row.Cell(4).Value = newContact;

                    Client client = new()
                    {
                        NameOfCompany = row.Cell(2).Value.ToString(),
                        Address = row.Cell(3).Value.ToString(),
                        ContactPerson = row.Cell(4).Value.ToString()
                    };
                    workbook.Save();
                    return client;
                }
            }
            return null;
        }

    }
}

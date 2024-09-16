using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ex.Models;
using System.Data;

namespace Shop_ex.Services
{
    public class ReadXLS
    {
        public List<AutoParts> ReadExcelFile(string filePath)
        {
            var products = new List<AutoParts>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });
                    var table = result.Tables[0];
                    int rowIndex = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        if (rowIndex == 0)
                        {
                            rowIndex++;
                            continue;
                        }

                        var n = row.Field<string>(0);           // название детали в файле номенклатура
                        var c = row.Field<string>(2);           // код в файле номенклатура
                        double? p = row.Field<double?>(3);      // наличие в файле номенклатура
                        if (n == null || c == null)
                        {
                            continue;
                        }
                        if (p == null)
                        {
                            p = 0;
                        }
                        else
                        {
                            p = Convert.ToInt32(row.Field<double>(3));
                        }
                        products.Add(new AutoParts
                        {
                            Name = row.Field<string>(0),
                            Code = row.Field<string>(2),
                            Count = (int?)p,
                        });
                    }
                }
            }
            return products;
        }

        public List<AutoParts> ReadExcelFile(string filePath, List<AutoParts> product)
        {
            var products = new List<AutoParts>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });
                    var table = result.Tables[0];

                    foreach (var p in product)
                    {
                        for (int i = 9; i < table.Rows.Count; i++) 
                        {
                            var row = table.Rows[i];
                            var secName = row.Field<string?>(1);        // название детали в файле ЦЕНА
                            double? price = row.Field<double?>(6);      // цена детали в файле ЦЕНА
                            if (p.Name == secName)
                            {
                                if (price == null)
                                {
                                    price = 0;
                                }
                                p.Price = Convert.ToInt32(price);
                                products.Add(p);
                                break;
                            }
                        }
                    }
                }
            }
            return products;
        }
    }
}

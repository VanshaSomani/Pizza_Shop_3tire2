using OfficeOpenXml;
using OfficeOpenXml.Style;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class CustomerExcelService : ICustomerExcelService
{
    public async Task<byte[]> ExportDataToExcel(CustomerListViewModel obj , string SearchCriteria , string FromDate , string ToDate , string timeDropDown){
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            //Status
            worksheet.Cells[2, 1, 3, 2].Merge = true;
            worksheet.Cells[2, 1, 3, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[2, 1, 3, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[2, 1, 3, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));
            worksheet.Cells[2, 1].Value = "Account:";

            worksheet.Cells[2, 3, 3, 6].Merge = true;
            worksheet.Cells[2, 3, 3, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[2, 3].Value = "";

            //Search Text
            worksheet.Cells[2, 8, 3, 9].Merge = true;
            worksheet.Cells[2, 8, 3, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[2, 8, 3, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[2, 8, 3, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));
            worksheet.Cells[2, 8].Value = "Search Text:";

            worksheet.Cells[2, 10, 3, 13].Merge = true;
            worksheet.Cells[2, 10, 3, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[2, 10].Value = string.IsNullOrEmpty(SearchCriteria) ? "" : SearchCriteria;

            //Date
            worksheet.Cells[5, 1, 6, 2].Merge = true;
            worksheet.Cells[5, 1, 6, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[5, 1, 6, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 1, 6, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));
            worksheet.Cells[5, 1].Value = "Date:";

            worksheet.Cells[5, 3, 6, 6].Merge = true;
            worksheet.Cells[5, 3, 6, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 3].Value = string.IsNullOrEmpty(timeDropDown) ? "All Time" : timeDropDown == "7" ? "Last 7 Days" : timeDropDown == "30" ? "Last 30 Days" : timeDropDown == "Month" ? "CurrentMonth" : "Date Range ";

            //No. Of Records
            worksheet.Cells[5, 8, 6, 9].Merge = true;
            worksheet.Cells[5, 8, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[5, 8, 6, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 8, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));
            worksheet.Cells[5, 8].Value = "No. Of Records:";

            worksheet.Cells[5, 10, 6, 13].Merge = true;
            worksheet.Cells[5, 10, 6, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 10].Value = obj.CustomerPaggination.TotalRecord;

            //filter style
            worksheet.Cells[2, 1, 6, 13].Style.Font.Bold = true;
            //worksheet.Cells[2,1,6,13].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[2, 1, 6, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 1, 6, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //pizza shop logo
            var img = "wwwroot/images/logos/pizzashop_logo.png";
            var image = new FileInfo(img);
            if (image.Exists)
            {
                var picture = worksheet.Drawings.AddPicture("pizzaShopLogo", new FileInfo(img));
                picture.SetPosition(1, 0, 14, 0);
                picture.SetSize(100, 100);
            }

            //Id
            worksheet.Cells[9, 1].Value = "Id";
            worksheet.Cells[9, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Name
            worksheet.Cells[9, 2, 9, 5].Merge = true;
            worksheet.Cells[9, 2].Value = "Name";
            worksheet.Cells[9, 2, 9, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 2, 9, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 2, 9, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Email
            worksheet.Cells[9, 6, 9, 9].Merge = true;
            worksheet.Cells[9, 6].Value = "Email";
            worksheet.Cells[9, 6, 9, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 6, 9, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 6, 9, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Date
            worksheet.Cells[9, 10, 9, 12].Merge = true;
            worksheet.Cells[9, 10].Value = "Date";
            worksheet.Cells[9, 10, 9, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 10, 9, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 10, 9, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Mobile Number
            worksheet.Cells[9, 13, 9, 15].Merge = true;
            worksheet.Cells[9, 13].Value = "Mobile Number";
            worksheet.Cells[9, 13, 9, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 13, 9, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 13, 9, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Total Orders
            worksheet.Cells[9, 16, 9, 17].Merge = true;
            worksheet.Cells[9, 16].Value = "Total Orders";
            worksheet.Cells[9, 16, 9, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 16, 9, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 16, 9, 17].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //header style
            worksheet.Cells[9, 1, 9, 16].Style.Font.Bold = true;
            worksheet.Cells[9, 1, 9, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 1, 9, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[9, 1, 9, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[9, 1, 9, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            int row = 10;
            foreach (var o in obj.CustomerList)
            {
                worksheet.Cells[row, 1].Value = o.CustomerId;
                worksheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 2, row, 5].Merge = true;
                worksheet.Cells[row, 2].Value = o.CustomerName;
                worksheet.Cells[row, 2, row, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 6, row, 9].Merge = true;
                worksheet.Cells[row, 6].Value = o.Email;
                worksheet.Cells[row, 6, row, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 10, row, 12].Merge = true;
                worksheet.Cells[row, 10].Value = o.Date.ToString("dd-MM-yyyy");
                worksheet.Cells[row, 10, row, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 13, row, 15].Merge = true;
                worksheet.Cells[row, 13].Value = o.PhoneNo;
                worksheet.Cells[row, 13, row, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 16, row, 17].Merge = true;
                worksheet.Cells[row, 16].Value = o.TotalOrder;
                worksheet.Cells[row, 16, row, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                row++;
            }

            /*string FileName = DateTime.Now.Second.ToString() + "_OrderDetails.xlsx";
            File.WriteAllBytes(FileName , package.GetAsByteArray());
            Console.WriteLine("File Created");*/
            return package.GetAsByteArray();
        }
        catch (Exception)
        {
            // Console.WriteLine(e);
            return null;
        }
    }
}

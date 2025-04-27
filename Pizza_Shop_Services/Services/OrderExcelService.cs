using OfficeOpenXml;
using OfficeOpenXml.Style;
using Pizza_Shop_Repository.ViewModels;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class OrderExcelService : IOrderExcelService
{
    public async Task<byte[]> ExportDataToExcel(OrderListViewModel obj, string SearchCriteria, string timeDropDown, string Status)
    {
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
            worksheet.Cells[2, 1].Value = "Status:";

            worksheet.Cells[2, 3, 3, 6].Merge = true;
            worksheet.Cells[2, 3, 3, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[2, 3].Value = string.IsNullOrEmpty(Status) ? "All Status" : Status;

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
            worksheet.Cells[5, 3].Value = string.IsNullOrEmpty(timeDropDown) ? "All Time" : timeDropDown == "7" ? "Last 7 Days" : timeDropDown == "30" ? "Last 30 Days" : "Current Month";

            //No. Of Records
            worksheet.Cells[5, 8, 6, 9].Merge = true;
            worksheet.Cells[5, 8, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[5, 8, 6, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 8, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));
            worksheet.Cells[5, 8].Value = "No. Of Records:";

            worksheet.Cells[5, 10, 6, 13].Merge = true;
            worksheet.Cells[5, 10, 6, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[5, 10].Value = obj.Order_Paggination.TotalRecord;

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

            //Date
            worksheet.Cells[9, 2, 9, 4].Merge = true;
            worksheet.Cells[9, 2].Value = "Date";
            worksheet.Cells[9, 2, 9, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 2, 9, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 2, 9, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Customer
            worksheet.Cells[9, 5, 9, 7].Merge = true;
            worksheet.Cells[9, 5].Value = "Customer";
            worksheet.Cells[9, 5, 9, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 5, 9, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 2, 9, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Status
            worksheet.Cells[9, 8, 9, 10].Merge = true;
            worksheet.Cells[9, 8].Value = "Status";
            worksheet.Cells[9, 8, 9, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 8, 9, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 8, 9, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Payment Mode
            worksheet.Cells[9, 11, 9, 12].Merge = true;
            worksheet.Cells[9, 11].Value = "Payment Mode";
            worksheet.Cells[9, 11, 9, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 11, 9, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 11, 9, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Rating
            worksheet.Cells[9, 13, 9, 14].Merge = true;
            worksheet.Cells[9, 13].Value = "Ratting";
            worksheet.Cells[9, 13, 9, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 13, 9, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 13, 9, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //Total Amount
            worksheet.Cells[9, 15, 9, 16].Merge = true;
            worksheet.Cells[9, 15].Value = "Total Amount";
            worksheet.Cells[9, 15, 9, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 15, 9, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[9, 15, 9, 16].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0d6efd"));

            //header style
            worksheet.Cells[9, 1, 9, 16].Style.Font.Bold = true;
            worksheet.Cells[9, 1, 9, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[9, 1, 9, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[9, 1, 9, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[9, 1, 9, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            int row = 10;
            foreach (var o in obj.Order_list)
            {
                worksheet.Cells[row, 1].Value = o.OrderId;
                worksheet.Cells[row, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 2, row, 4].Merge = true;
                worksheet.Cells[row, 2].Value = o.OrderDate.ToString("dd-MM-yyyy");
                worksheet.Cells[row, 2, row, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 5, row, 7].Merge = true;
                worksheet.Cells[row, 5].Value = o.CustomerName;
                worksheet.Cells[row, 2, row, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 8, row, 10].Merge = true;
                worksheet.Cells[row, 8].Value = o.Status;
                worksheet.Cells[row, 8, row, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 11, row, 12].Merge = true;
                worksheet.Cells[row, 11].Value = o.PaymentMode;
                worksheet.Cells[row, 11, row, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 13, row, 14].Merge = true;
                worksheet.Cells[row, 13].Value = o.Rating;
                worksheet.Cells[row, 13, row, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[row, 15, row, 16].Merge = true;
                worksheet.Cells[row, 15].Value = o.TotalAmount;
                worksheet.Cells[row, 15, row, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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

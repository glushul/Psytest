using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Psytest.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Psytest.UI.PsychologistInterface.UserControls
{
    /// <summary>
    /// Логика взаимодействия для YearTabUserControl.xaml
    /// </summary>
    public partial class GroupTestingTabUserControl : UserControl
    {
        Group _group;
        Testing _testing;
        int _year;
        static BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf",
            BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        Font[] fonts = { new Font(baseFont, 9, Font.NORMAL), new Font(baseFont, 9, Font.ITALIC), new Font(baseFont, 9, Font.BOLD) };
        int studentCount = 0;

        public GroupTestingTabUserControl(Testing testing, Group group, int year)
        {
            InitializeComponent(); 
            _testing = testing;
            _group = group;
            _year = year;
            studentCount = PsytestDBEntities.GetContext().StudentResults.
                Where(p => p.GroupId == group.Id && p.TestingId == testing.Id && p.TestingYear == year).
                GroupBy(p => new { p.Surname, p.Name }).Count();
            TextBlockFullName.Text = testing.Id.ToString() + ". " + testing.FullName.ToString();
            TextBlockStudentCount.Text = "Количество прошедших: " + studentCount.ToString();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (studentCount != 0)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "|*.pdf";
                string fileName = "";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = saveFileDialog.FileName;
                else
                    return;

                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());

                PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

                //Открытие документ
                doc.Open();

                if (_testing.OutputType == 1)
                {
                    var categories = PsytestDBEntities.GetContext().Categories.
                    Where(p => p.TestingId == _testing.Id).ToList();

                    var results = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.GroupId == _group.Id && p.TestingId == _testing.Id && p.TestingYear == _year).
                    OrderBy(p => new { p.Surname, p.Name, p.CategoryId }).GroupBy(p => new { p.Surname, p.Name });

                    //Создание объекта таблицы
                    PdfPTable table = new PdfPTable(1 + categories.Count*3);

                    //Добавление в таблицу общего заголовка
                    PdfPCell cell = new PdfPCell(new Phrase(100, "Результаты тестирования " +
                        _testing.FullName + " группы " + _group.FullName, new Font(baseFont, 12, Font.NORMAL)));
                    cell.Colspan = 1 + categories.Count*3;
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    float[] widths = new float[1 + categories.Count * 3];

                    cell = new PdfPCell(new Phrase(new Phrase(0, "ФИО", fonts[0])));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    widths[0] = 4f;

                    for (int j = 0; j < categories.Count*3; j += 3)
                    {
                        cell = new PdfPCell(new Phrase(new Phrase("Категория", fonts[0])));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        widths[j+1] = 2f;

                        cell = new PdfPCell(new Phrase(new Phrase("Балл", fonts[0])));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        widths[j+2] = 1f;

                        cell = new PdfPCell(new Phrase(new Phrase("Результат", fonts[0])));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        widths[j+3] = 3f;
                    }

                    table.SetWidths(widths);

                    foreach (var result in results)
                    {
                        table.AddCell(new Phrase(75, result.Key.Surname + " " +
                        result.Key.Name, fonts[0]));
                        foreach(var studentResult in result) 
                        {
                            Raging raging = PsytestDBEntities.GetContext().Ragings.
                                Where(p => p.CategoryId == studentResult.Category.Id
                            && p.MinSum <= studentResult.PointSum && p.MaxSum >= studentResult.PointSum).FirstOrDefault();

                            table.AddCell(new Phrase(50, studentResult.Category.Name, fonts[0]));
                            table.AddCell(new Phrase(25, studentResult.PointSum.ToString(), fonts[raging.FontStyle]));
                            table.AddCell(new Phrase(75, raging.Name.ToString(), fonts[raging.FontStyle]));
                        }
                    }

                    //Добавление таблицы в документ
                    doc.Add(table);
                    //Закрытие документа
                    doc.Close();
                }
                else if (_testing.OutputType == 2)
                {
                    var categories = PsytestDBEntities.GetContext().Categories.
                    Where(p => p.TestingId == _testing.Id).ToList();

                    var results = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.GroupId == _group.Id && p.TestingId == _testing.Id).
                    OrderBy(p => new { p.Surname, p.Name, p.CategoryId }).
                    GroupBy(p => new { p.Surname, p.Name, p.Gender, p.Age, p.Group });

                    foreach (var result in results)
                    {
                        //Создание объекта таблицы
                        PdfPTable table = new PdfPTable(2);
                        table.SetWidths(new float[] { 1f, 5f });

                        //Добавление в таблицу общего заголовка
                        PdfPCell cell = new PdfPCell(new Phrase(100, result.Key.Group.FullName
                            + "   " + result.Key.Surname
                            + "  " + result.Key.Name + "   " + result.Key.Age
                            + "   " + result.Key.Gender.Name,
                            new Font(baseFont, 16, Font.BOLD)));
                        cell.Colspan = 2;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        //Добавление всех остальных ячеек
                        foreach (var studentResult in result)
                        {
                            PdfPCell cellCategory = new PdfPCell(new Phrase(100, studentResult.Category.Name,
                                new Font(baseFont, 9, Font.NORMAL)));
                            cellCategory.Colspan = 2;
                            cellCategory.HorizontalAlignment = 1;
                            table.AddCell(cellCategory);

                            var raging = PsytestDBEntities.GetContext().Ragings.
                                Where(p => p.CategoryId == studentResult.CategoryId
                            && p.MinSum <= studentResult.PointSum && p.MaxSum >= studentResult.PointSum).FirstOrDefault();
                            table.AddCell(new Phrase(75, raging.Description, fonts[raging.FontStyle]));
                            table.AddCell(new Phrase(75, raging.Name.ToString(), fonts[raging.FontStyle]));
                        }
                        //Добавление таблицы в документ
                        doc.Add(table);
                        doc.NewPage();
                    }

                    //Закрытие документа
                    doc.Close();
                }
                else if (_testing.OutputType == 3)
                {
                    var results = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.GroupId == _group.Id && p.TestingId == _testing.Id).
                    OrderBy(p => new { p.Surname, p.Name, p.CategoryId }).
                    GroupBy(p => new { p.Surname, p.Name });

                    //Создание объекта таблицы
                    PdfPTable table = new PdfPTable(2);

                    //Добавление в таблицу общего заголовка
                    PdfPCell cell = new PdfPCell(new Phrase(100, "Результаты тестирования " +
                        _testing.FullName + " группы " + _group.FullName, new Font(baseFont, 12, Font.NORMAL)));
                    cell.Colspan = 2;
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    float[] widths = { 1f, 3f };

                    cell = new PdfPCell(new Phrase(new Phrase(0, "ФИО", fonts[0])));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(new Phrase(0, "Результат", fonts[0])));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    table.SetWidths(widths);

                    foreach (var result in results)
                    {
                        table.AddCell(new Phrase(75, result.Key.Surname + " " +
                        result.Key.Name, fonts[0]));
                        var studentResults = result.OrderByDescending(p => p.PointSum).ToList();
                        table.AddCell(new Phrase(75, studentResults[0].Category.Name + "(" + studentResults[0].PointSum + ")" + " - " +
                        studentResults[1].Category.Name + "(" + studentResults[1].PointSum + ")" + " - " 
                        + studentResults[2].Category.Name + "(" + studentResults[2].PointSum + ")", fonts[0]));
                    }

                    //Добавление таблицы в документ
                    doc.Add(table);
                    //Закрытие документа
                    doc.Close();
                }
            }
            else
            {

            }
        }
    }
}

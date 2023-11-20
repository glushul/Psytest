using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using Psytest.Data;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Psytest.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для GroupTestingUserControl.xaml
    /// </summary>
    public partial class GroupTestingUserControl : UserControl
    {
        Group _group;
        Testing _testing;
        static BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf",
            BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        Font[] fonts = { new Font(baseFont, 9, Font.NORMAL), new Font(baseFont, 9, Font.ITALIC), new Font(baseFont, 9, Font.BOLD) };
        Font font = new Font(baseFont, 9, Font.NORMAL);
        int studentCount = 0;

        public GroupTestingUserControl(Testing testing, Group group)
        {
            InitializeComponent();
            _testing = testing;
            _group = group;
            TestingTextBlock.Text = testing.FullName;
            studentCount = PsytestDBEntities.GetContext().StudentResults.
                Where(p => p.GroupId == group.Id && p.TestingId == testing.Id).
                GroupBy(p => new { p.Surname, p.Name }).Count();
            StudentCountTextBlock.Text = "Количество прошедших: " +
                studentCount.ToString();

        }

        /// <summary>
        /// Печать результатов тестирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrintResults_Click(object sender, RoutedEventArgs e)
        {
            if (studentCount != 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "|*.pdf";
                string fileName = "";
                if (saveFileDialog.ShowDialog() == true)
                    fileName = saveFileDialog.FileName.ToString();
                else
                    return;

                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());

                PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

                //Открытие документ
                doc.Open();

                if (_testing.Id == 1 || _testing.Id == 2 || _testing.Id == 3)
                {
                    int count = 1 + PsytestDBEntities.GetContext().Categories.
                    Where(p => p.TestingId == _testing.Id).Count() * 3;

                    var results = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.GroupId == _group.Id && p.TestingId == _testing.Id).
                    OrderBy(p => new { p.Surname, p.Name, p.CategoryId });

                    //Создание объекта таблицы
                    PdfPTable table = new PdfPTable(count);

                    //Добавление в таблицу общего заголовка
                    PdfPCell cell = new PdfPCell(new Phrase(100, "Результаты тестирования " +
                        _testing.FullName + " группы " + _group.FullName, new Font(baseFont, 12, Font.NORMAL)));

                    cell.Colspan = count;
                    cell.Border = 0;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                    //Задание ширины колонок
                    float[] widths7 = new float[] { 4f, 2f, 1f, 3f, 2f, 1f, 3f };
                    float[] widths4 = new float[] { 4f, 2f, 1f, 3f };
                    if (count == 4)
                        table.SetWidths(widths4);
                    else
                        table.SetWidths(widths7);

                    //Добавление заголовков таблицы
                    string[] columnNames = { "ФИО", "Категория", "Балл", "Результат", "Категория", "Балл", "Результат" };

                    for (int j = 0; j < count; j++)
                    {
                        cell = new PdfPCell(new Phrase(new Phrase(columnNames[j], font)));
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                    }

                    for (int i = 0; i < results.ToArray().Length; i++)
                    {
                        table.AddCell(new Phrase(75, results.ToArray()[i].Surname + " " +
                        results.ToArray()[i].Name, font));
                        int pointSum = results.ToArray()[i].PointSum;
                        int categoryId = results.ToArray()[i].CategoryId;
                        Raging raging = PsytestDBEntities.GetContext().Ragings.
                            Where(p => p.CategoryId == categoryId
                        && p.MinSum <= pointSum && p.MaxSum >= pointSum).FirstOrDefault();
                        table.AddCell(new Phrase(50, results.ToArray()[i].Category.Name, font));
                        table.AddCell(new Phrase(25, results.ToArray()[i].PointSum.ToString(), fonts[raging.FontStyle]));
                        table.AddCell(new Phrase(75, raging.Name.ToString(), fonts[raging.FontStyle]));

                        if (count == 7)
                        {
                            i++;
                            pointSum = results.ToArray()[i].PointSum;
                            categoryId = results.ToArray()[i].CategoryId;
                            raging = PsytestDBEntities.GetContext().Ragings.
                                Where(p => p.CategoryId == categoryId
                            && p.MinSum <= pointSum && p.MaxSum >= pointSum).FirstOrDefault();
                            table.AddCell(new Phrase(50, results.ToArray()[i].Category.Name, font));
                            table.AddCell(new Phrase(25, results.ToArray()[i].PointSum.ToString(), fonts[raging.FontStyle]));
                            table.AddCell(new Phrase(75, raging.Name.ToString(), fonts[raging.FontStyle]));
                        }

                    }

                    //Добавление таблицы в документ
                    doc.Add(table);
                    //Закрытие документа
                    doc.Close();
                }
                else
                {
                    var results = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.GroupId == _group.Id && p.TestingId == _testing.Id).
                    OrderBy(p => new { p.Surname, p.Name, p.CategoryId }).ToArray();

                    for (int i = 0; i < results.Length; i++)
                    {
                        //Создание объекта таблицы
                        PdfPTable table = new PdfPTable(2);
                        table.SetWidths(new float[] { 1f, 5f });

                        //Добавление в таблицу общего заголовка
                        PdfPCell cell = new PdfPCell(new Phrase(100, results[i].Group.FullName
                            + "   " + results[i].Surname
                            + " " + results[i].Name + "   " + results[i].Age
                            + "   " + results[i].Gender,
                            new Font(baseFont, 16, Font.BOLD)));

                        cell.Colspan = 2;
                        cell.Border = 0;
                        cell.HorizontalAlignment = 1;
                        table.AddCell(cell);

                        int o = i + 7;
                        //Добавление всех остальных ячеек
                        for (int k = i; k < o; k++)
                        {
                            int pointSum = results[k].PointSum;
                            int categoryId = results[k].CategoryId;
                            PdfPCell cellCategory = new PdfPCell(new Phrase(100, results[k].Category.Name,
                                new Font(baseFont, 9, Font.NORMAL)));
                            cellCategory.Colspan = 2;
                            cellCategory.HorizontalAlignment = 1;
                            table.AddCell(cellCategory);
                            var raging = PsytestDBEntities.GetContext().Ragings.
                                Where(p => p.CategoryId == categoryId
                            && p.MinSum <= pointSum && p.MaxSum >= pointSum).FirstOrDefault();
                            table.AddCell(new Phrase(75, raging.Description, fonts[raging.FontStyle]));
                            table.AddCell(new Phrase(75, raging.Name.ToString(), fonts[raging.FontStyle]));
                            i = k;
                        }
                        //Добавление таблицы в документ
                        doc.Add(table);
                        doc.NewPage();
                    }

                    //Закрытие документа
                    doc.Close();
                }
                MessageBox.Show("Pdf-документ сохранен");
            }
        }
    }
}

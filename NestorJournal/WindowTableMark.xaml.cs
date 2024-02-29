using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NestorJournal
{
    /// <summary>
    /// Логика взаимодействия для WindowTableMark.xaml
    /// </summary>
    public partial class WindowTableMark : Window
    {
        NestorJournalEntities BD;
        Shedule item;

        List<DateTime> listDate;
        List<Students> listStudent;
        List<int> dayweek;

        public WindowTableMark(NestorJournalEntities BDD, Shedule itemm)
        {
            InitializeComponent();
            this.BD = BDD;
            this.item = itemm;

            this.DataContext = item;

            listStudent = BD.Students.Where(stud => stud.groupsID == item.id_Group).OrderBy(stud => stud.firstname).ToList();
            dayweek = BD.DayWeekShedule.Where(shed => shed.SheduleID == item.id).Select(shed => shed.DayWeekID).ToList();

            DateTime date_start = item.DateStart;
            DateTime date_end = item.DateEnd;

            listDate = new List<DateTime>();

            while (date_end >= date_start)
            {
                if (dayweek.Contains(((int)date_start.DayOfWeek)))
                {
                    listDate.Add(date_start);
                    dg_mark.Columns.Add(new DataGridTextColumn()
                    {
                        Header = date_start.ToString("dd.MM"),
                        Width = 40,
                        Binding = new Binding(date_start.ToString("dd-MM")),
                        CanUserReorder = false,
                        CanUserSort = false
                    });
                }
                date_start = date_start.AddDays(1);
            }
            SetMarkTable();
        }

        private void SetMarkTable()
        {
            dg_mark.ItemsSource = null;

            var listMark = BD.MarkTable.Where(marks => marks.id_Trainer == item.id_Trainer
                                    & marks.id_Study == item.id_Study & marks.Students.groupsID == item.id_Group)
                                    .OrderBy(marks => marks.Students.firstname).ToList();

            var table = new DataTable();

            table.Columns.Add("Студент");

            foreach (var date in listDate)
            {
                table.Columns.Add(date.ToString("dd-MM"));
            }

            foreach (var row in listStudent)
            {
                table.Rows.Add(row.firstname.ToString());
            }
             
            int x;
            string y;
            foreach (var row in listMark)
            {
                x = listStudent.IndexOf(row.Students);
                y = row.date_mark.ToString("dd-MM");
                table.Rows[x][y] = row.Marks.title;
            }
            dg_mark.ItemsSource = table.DefaultView;
        }


        private void dg_mark_MouseClick(object sender, MouseButtonEventArgs e)
        {
            int b = dg_mark.SelectedCells[0].Column.DisplayIndex - 1;
            if (dg_mark.SelectedCells.Count == 1 & b >=0)
            {

                string a = (dg_mark.SelectedCells[0].Item as DataRowView).Row.ItemArray[0].ToString();
                var dat = listDate[dg_mark.SelectedCells[0].Column.DisplayIndex - 1];
                var mark = BD.MarkTable.Where(m => m.date_mark == dat & m.Students.firstname == a).ToList()
                    .DefaultIfEmpty(new MarkTable
                    {
                        Students = BD.Students.Where(st => st.firstname == a).First(),
                        Study = item.Study,
                        Trainer = item.Trainer,
                        date_mark = dat
                    }).FirstOrDefault();
                new WindowMark(BD, mark).ShowDialog();
                SetMarkTable();
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NestorJournal
{
    /// <summary>
    /// Логика взаимодействия для WindowMark.xaml
    /// </summary>
    public partial class WindowMark : Window
    {
        NestorJournalEntities BD;
        MarkTable Mark;
        Marks OldMark;
        public WindowMark(NestorJournalEntities bD, MarkTable mark)
        {
            InitializeComponent();
            BD = bD;
            Mark = mark;
            OldMark = mark.Marks;
            this.DataContext = Mark;
            cmb_mark.ItemsSource = BD.Marks.ToList();
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            if (Mark.id == 0)
            {
                BD.MarkTable.Add(Mark);
            }
            BD.SaveChanges();
            this.Close();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            Mark.Marks = OldMark;
            this.Close();
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NestorJournal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NestorJournalEntities BD = new NestorJournalEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var trainer = BD.Trainer.Where(t => t.log_in == tb_login.Text & t.pass_word == tb_password.Password).FirstOrDefault();
            if (trainer != null )
            {
                AppTempData.trainer = trainer;
                //MessageBox.Show("успех");
                new WindowShedule().Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Не успешная авторизация");
            }
        }
    }
}

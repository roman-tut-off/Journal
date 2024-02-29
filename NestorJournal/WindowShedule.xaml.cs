using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NestorJournal
{
    /// <summary>
    /// Логика взаимодействия для WindowShedule.xaml
    /// </summary>
    public partial class WindowShedule : Window
    {
        NestorJournalEntities BD = new NestorJournalEntities();
        public WindowShedule()
        {
            InitializeComponent();

            var temp1 = BD.Shedule.Where(sh => sh.Trainer.id == AppTempData.trainer.id).Select(sh => sh.Groups).Distinct().ToList();
            temp1.Insert(0, new Groups() { title = "Все варианты" });
            cmb_group.ItemsSource = temp1;
            cmb_group.SelectedIndex = 0;

            var temp2 = BD.Shedule.Where(sh => sh.Trainer.id == AppTempData.trainer.id).Select(sh => sh.Study).Distinct().ToList();
            temp2.Insert(0, new Study() { title = "Все варианты" });
            cmb_study.ItemsSource = temp2;
            cmb_study.SelectedIndex = 0;

            this.Title = AppTempData.trainer.firstname + " " + AppTempData.trainer.secondname + " " + AppTempData.trainer.thirdname;

            SetShedule();
        }

        private void dg_shedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg_shedule.SelectedValue != null)
            {
                int i = (int)dg_shedule.SelectedValue;
                Shedule item = BD.Shedule.Where(a => a.id == i).First();
                var win = new WindowTableMark(BD, item);
                win.ShowDialog();
                win.Close();
                dg_shedule.SelectedValue = null;
            }
        }
        public void SetShedule()
        {
            dg_shedule.ItemsSource = BD.Shedule.Where(shed => shed.Trainer.id == AppTempData.trainer.id).ToList()
                .Where(shed => (shed.Study == cmb_study.SelectedItem | cmb_study.SelectedIndex == 0)
                & (shed.Groups == cmb_group.SelectedItem | cmb_group.SelectedIndex == 0))
                .Select(shed => new
                {
                    id = shed.id,
                    Study = shed.Study,
                    Groups = shed.Groups,
                    textday = string.Join(", ", shed.DayWeekShedule.Select(day => day.DayWeek.title + " " + day.lesson + " пара").ToList())
                });
        }

        private void cmb_study_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetShedule();
        }

        private void cmb_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetShedule();
        }
    }
}

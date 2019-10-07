using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Dapper;

namespace DB_2_dabas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool showChoice = false;
        public Planai planas;
        public Planai maxValues;
        private ObservableCollection<Planai> _planais;

        public ObservableCollection<Planai> planais
        {
            get
            {
                return _planais;
            }
            set
            {
                _planais = value;
                UpdateList(listVisiPlanai, value, false);
                UpdateList(listTinkamiausiPlanai, value, true);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            planais = GetPlanais();
            Console.WriteLine();
            SetUpPreparePlan();
        }

        #region Database access
        private ObservableCollection<Planai> GetPlanais()
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=./db.db;Version=3;"))
            {
                var output = cnn.Query<Planai>("select * from Planai", new DynamicParameters());
                return new ObservableCollection<Planai>(output.ToList());
            }
        }

        private void InsertPlanas(Planai planas)
        {
            using (IDbConnection cnn = new SQLiteConnection("Data Source=./db.db;Version=3;"))
            {
                cnn.Execute("insert into Planai (Operatorius, Pokalbiai, Sms, Internetas, Kaina, PlanoPavadinimas) VALUES (@Operatorius,@pokalbiai,@Sms,@Internetas,@Kaina,@PlanoPavadinimas)", planas);
            }
        }

        #endregion

        #region UI
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-Z_ ]*$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void UpdateList(object listbox, ObservableCollection<Planai> list, bool selection)
        {
            (listbox as ListBox).Items.Clear();

            foreach (Planai item in list)
            {
                if (selection == item.selected)
                    (listbox as ListBox).Items.Add(item);
            }
        }

        private void ListVisiPlanai_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListBox).SelectedItem != null)
            {
                ((sender as ListBox).SelectedItem as Planai).selected = !((sender as ListBox).SelectedItem as Planai).selected;
                UpdateList(listVisiPlanai, _planais, false);
                UpdateList(listTinkamiausiPlanai, _planais, true);
            }
        }

        private void Clear()
        {
            Internetas.Text = "";
            Kaina.Text = "";
            Minutes.Text = "";
            Pokalbiai.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Planai planas = new Planai();
            planas.Internetas = int.Parse(Internetas.Text);
            planas.Kaina = int.Parse(Kaina.Text);
            planas.Pokalbiai = int.Parse(Minutes.Text);
            planas.Sms = int.Parse(Pokalbiai.Text);
            InsertPlanas(planas);
            planais = GetPlanais();
            Clear();
        }

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            showChoice = !showChoice;
            if (showChoice)
            {
                PasirinkimaiUC.Visibility = Visibility.Visible;
            }
            else
            {
                PasirinkimaiUC.Visibility = Visibility.Collapsed;
            }
            SavePreparedPlan();
        }

        private void SetUpPreparePlan()
        {
            planas = new Planai();
            GetMaxValues();
            /*
            SmsSlider.Maximum = maxValues.Sms;
            PokalbiaiSlider.Maximum = maxValues.Pokalbiai;
            InternetasSlider.Maximum = maxValues.Internetas;
            KainaSlider.Maximum = maxValues.Kaina;*/


            SmsSlider.Maximum = 10000;
            PokalbiaiSlider.Maximum = 10000;
            InternetasSlider.Maximum = 10000;
            KainaSlider.Maximum = maxValues.Kaina;
        }

        private void SavePreparedPlan()
        {
            planas.Sms = (int)SmsSlider.Value;
            planas.Pokalbiai = (int)PokalbiaiSlider.Value;
            planas.Internetas = (int)InternetasSlider.Value;
            planas.Kaina = SmsSlider.Value;
        }

        private void GetMaxValues()
        {
            maxValues = new Planai();
            foreach (Planai tmpPlanas in planais)
            {
                if (maxValues.Sms < tmpPlanas.Sms)
                    maxValues.Sms = tmpPlanas.Sms;

                if (maxValues.Pokalbiai < tmpPlanas.Pokalbiai)
                    maxValues.Pokalbiai = tmpPlanas.Pokalbiai;

                if (maxValues.Internetas < tmpPlanas.Internetas)
                    maxValues.Internetas = tmpPlanas.Internetas;

                if (maxValues.Kaina < tmpPlanas.Kaina)
                    maxValues.Kaina = tmpPlanas.Kaina;
            }


        }

        private Planai getMinVlue(Dictionary<double, Planai> obj)
        {
            KeyValuePair<double, Planai> bestPlanas = new KeyValuePair<double, Planai>();
            foreach(KeyValuePair<double,Planai> pl in obj)
            {
                if (bestPlanas.Value == null)
                    bestPlanas = pl;
                if (bestPlanas.Key > pl.Key)
                    bestPlanas = pl;
            }
            return (Planai)bestPlanas.Value;
        }


        private Planai Getbestdalykas()
        {
            Dictionary<double, Planai> answers = new Dictionary<double, Planai>();
            
            foreach(Planai pl in planais)
            {
                answers.Add(Math.Sqrt(
                  double.Parse(Pokalbiai.Text) * Math.Pow(SmsSlider.Value - pl.Sms, 2)
                + double.Parse(Minutes.Text) * Math.Pow(PokalbiaiSlider.Value - pl.Pokalbiai, 2)
                + double.Parse(Internetas.Text) * Math.Pow(InternetasSlider.Value - pl.Internetas, 2) 
                + double.Parse(Kaina.Text) * Math.Pow(KainaSlider.Value - pl.Kaina, 2)), pl);
            }
            return getMinVlue(answers);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            listTinkamiausiPlanai.Items.Clear();
            listTinkamiausiPlanai.Items.Add(Getbestdalykas());
        }
    }
}

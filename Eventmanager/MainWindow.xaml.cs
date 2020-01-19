using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Linq;
using System.Windows.Input;
using System.IO;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using Newtonsoft.Json;

namespace Eventmanager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Save saveObject = new Save();
        Export _export = new Export();
        Klassen _klasse = new Klassen();
        public MainWindow()
        {
            InitializeComponent();

            saveObject.CreateDirectory();
            Seite1.Visibility = Visibility.Visible;
            Seite2.Visibility = Visibility.Hidden;
            Seite3.Visibility = Visibility.Hidden;
            Seite4.Visibility = Visibility.Hidden;
            Seite5.Visibility = Visibility.Hidden;
            DruckGrid.Visibility = Visibility.Hidden;
            Importieren.IsEnabled = false;
            Exportieren.IsEnabled = false;
            Speichern.IsEnabled = false;
        }

        public List<Teilnehmer> Starterliste = new List<Teilnehmer>();
        public List<Teilnehmer> Rankingliste = new List<Teilnehmer>();
        public int StarterIndex = 0;

        private void Öffnen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".json";
            openfile.Filter = "(.json)|*.json";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                using (StreamReader r = new StreamReader(openfile.FileName))
                {
                    string json = r.ReadToEnd();
                    Starterliste = JsonConvert.DeserializeObject<List<Teilnehmer>>(json);
                }

                if(Starterliste.Count > 0)
                {
                    if(Starterliste[0].Turniername != null)
                    {
                        Turniername_Label.Content = Starterliste[0].Turniername;
                        Turniername_LabelAktuell.Content = Starterliste[0].Turniername;
                        TurniernameNeu_TextBox.Text = Starterliste[0].Turniername;
                    }

                    if(Starterliste[0].Ort != null)
                    {
                        OrtNeu_TextBox.Text = Starterliste[0].Ort;
                    }

                    if(Starterliste[0].Datum != null)
                    {
                        WettkampfDatum_TimePicker.Value = Starterliste[0].Datum;
                    }

                    if(Starterliste[0].T1Name != null)
                    {
                        T1Name_Textbox.Text = Starterliste[0].T1Name;
                    }

                    if(Starterliste[0].T2Name != null)
                    {
                        T2Name_Textbox.Text = Starterliste[0].T2Name;
                    }

                    if(Starterliste[0].T3Name != null)
                    {
                        T3Name_Textbox.Text = Starterliste[0].T3Name;
                    }

                    if(Starterliste[0].T4Name != null)
                    {
                        T4Name_Textbox.Text = Starterliste[0].T4Name;
                    }

                    if(Starterliste[0].A1Name != null)
                    {
                        A1Name_Textbox.Text = Starterliste[0].A1Name;
                    }
 
                    if(Starterliste[0].A2Name != null)
                    {
                        A2Name_Textbox.Text = Starterliste[0].A2Name;
                    }

                    if(Starterliste[0].A3Name != null)
                    {
                        A3Name_Textbox.Text = Starterliste[0].A3Name;
                    }
 
                    if(Starterliste[0].A4Name != null)
                    {
                        A4Name_Textbox.Text = Starterliste[0].A4Name;
                    }

                    if(Starterliste[0].CJP != null)
                    {
                        CJP_Textbox.Text = Starterliste[0].CJP;
                    }

                    if(Starterliste[0].Schwierigkeitskampfrichter != null)
                    {
                        Schwierigkeitskampfrichter_Textbox.Text = Starterliste[0].Schwierigkeitskampfrichter;
                    }

                    Starterliste_DataGrid.Items.Clear();

                    foreach (var element in Starterliste)
                    {
                        Starterliste_DataGrid.Items.Add(element);
                    }

                    foreach (var element in Starterliste)
                    {
                        if (StartNummer_Aktuell.Text == element.Startnummer)
                        {

                            Technik1_Textbox.Text = element.Punkte.Technik1;
                            Technik2_Textbox.Text = element.Punkte.Technik2;
                            Technik3_Textbox.Text = element.Punkte.Technik3;
                            Technik4_Textbox.Text = element.Punkte.Technik4;
                            Artistik1_Textbox.Text = element.Punkte.Artistik1;
                            Artistik2_Textbox.Text = element.Punkte.Artistik2;
                            Artistik3_Textbox.Text = element.Punkte.Artistik3;
                            Artistik4_Textbox.Text = element.Punkte.Artistik4;
                            MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                            MinTechnik_Label.Content = element.Punkte.TechnikMin;
                            MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                            MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                            Technik_Label.Content = element.Punkte.TechnikGesamt;
                            Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                            GesamtWertung_Label.Content = element.Punkte.GWertung;
                            Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                            Abzüge_Textbox.Text = element.Punkte.Abzüge;

                            if (element.Platz > 0)
                            {
                                Platzierung_Label.Content = element.Platz;
                            }
                            else
                            {
                                Platzierung_Label.Content = "";
                            }

                            Rankingliste.Clear();

                            foreach (var klassenElement in Starterliste)
                            {

                                if (klassenElement.KlasseGesamt == element.KlasseGesamt)
                                {
                                    Rankingliste.Add(klassenElement);
                                }
                            }

                            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

                            //Setze Ranking ins Data Grid
                            RankingGrid.Items.Clear();
                            foreach (var rankingElement in Rankingliste)
                            {
                                if (rankingElement.Platz > 0)
                                {
                                    RankingGrid.Items.Add(rankingElement);
                                }
                            }

                            break;
                        }
                    }

                }
            }
        }

        private void Beenden_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Neu_Click(object sender, RoutedEventArgs e)
        {
            Importieren.IsEnabled = true;
            Exportieren.IsEnabled = true;
            Seite1.Visibility = Visibility.Visible;
            Seite2.Visibility = Visibility.Hidden;
            Seite3.Visibility = Visibility.Hidden;
            Seite4.Visibility = Visibility.Hidden;
            Seite5.Visibility = Visibility.Hidden;
            Starterliste.Clear();
            Rankingliste.Clear();
            Starterliste_DataGrid.Items.Clear();
            TurniernameNeu_TextBox.Text = "";
            OrtNeu_TextBox.Text = "";
            Technik1_Textbox.Text = "";
            Technik2_Textbox.Text = "";
            Technik3_Textbox.Text = "";
            Technik4_Textbox.Text = "";
            Artistik1_Textbox.Text = "";
            Artistik2_Textbox.Text = "";
            Artistik3_Textbox.Text = "";
            Artistik4_Textbox.Text = "";
            StartNummer_Aktuell.Text = "";
            Riege_Aktuell.Content = "";
            Disziplin_Aktuell.Content = "";
            Klasse_Aktuell.Content = "";
            Starter_Aktuell.Content = "";
            Verein_Aktuell.Content = "";
            T1Name_Textbox.Text = "";
            T2Name_Textbox.Text = "";
            T3Name_Textbox.Text = "";
            T4Name_Textbox.Text = "";
            A1Name_Textbox.Text = "";
            A2Name_Textbox.Text = "";
            A3Name_Textbox.Text = "";
            A4Name_Textbox.Text = "";
        }

        private void Hinzufügen_Button_Click(object sender, RoutedEventArgs e)
        {
            if(Startnummer_Textbox.Text == null)
            {
                return;
            }

            if (Riege_Textbox.Text == null)
            {
                return;
            }


            if (Starter_Textbox.Text == null)
            {
                return;
            }

            if(Verein_Textbox.Text == null)
            {
                return;
            }

            if(Klasse_Combobox.SelectedItem == null)
            {
                return;
            }

            if(Disziplin_Combobox.SelectedItem == null)
            {
                return;
            }

            var neuerStarter = new Teilnehmer
            {
                Startnummer = Startnummer_Textbox.Text,
                Riege = Riege_Textbox.Text,
                Starter = Starter_Textbox.Text,
                Verein = Verein_Textbox.Text,
                Klasse = ((ComboBoxItem)Klasse_Combobox.SelectedItem).Content.ToString(),
                Disziplin = ((ComboBoxItem)Disziplin_Combobox.SelectedItem).Content.ToString(),
                StartVorbereitung = StartVorbereitung_TimePicker.Value,
                StartWettkampf = StartWettkampf_TimePicker.Value,
            };

            Starterliste.Add(neuerStarter);

            if(StartzeitNutzen_CheckBox.IsChecked == true)
            {
                Starterliste = Starterliste.OrderBy(i => i.StartWettkampf).ToList();
            }
           
            Starterliste_DataGrid.Items.Clear();

            foreach (var element in Starterliste)
            {
                Starterliste_DataGrid.Items.Add(element);
            }

        }

        private void TurniernameNeu_Button_Click(object sender, RoutedEventArgs e)
        {
            Seite3.Visibility = Visibility.Visible;
            Turniername_Label.Content = TurniernameNeu_TextBox.Text;
            Turniername_LabelAktuell.Content = TurniernameNeu_TextBox.Text;
            Seite2.Visibility = Visibility.Hidden;
            Öffnen.IsEnabled = true;
            Speichern.IsEnabled = true;
        }

        private void Importieren_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(openfile.FileName.ToString(), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                Starterliste.Clear();
                Rankingliste.Clear();
                Starterliste_DataGrid.Items.Clear();
                DataTable dt = new DataTable();

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += strCellData + "|";
                        }
                        catch (Exception)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    var dataList = strData.Split('|');

                    DateTime vorbereitung = DateTime.Parse(dataList[6]);
                    DateTime wettkampf = DateTime.Parse(dataList[7]);

                    var neuerStarter = new Teilnehmer
                    {
                        Startnummer = dataList[0],
                        Riege = dataList[1],
                        Starter = dataList[2],
                        Verein = dataList[3],
                        Klasse = dataList[4],
                        Disziplin = dataList[5],
                        StartVorbereitung = vorbereitung,
                        StartWettkampf = wettkampf
                };

                    neuerStarter.KlasseGesamt =_klasse.Klassenermittlung(neuerStarter.Klasse, neuerStarter.Disziplin);
                    neuerStarter.Punkte = new Punkte();

                    Starterliste.Add(neuerStarter);
                }

                if(StartzeitNutzen_CheckBox.IsChecked == true)
                {
                    Starterliste = Starterliste.OrderBy(i => i.StartWettkampf).ToList();
                }
               

                foreach(var element in Starterliste)
                {
                    Starterliste_DataGrid.Items.Add(element);
                }

                if (Starterliste_DataGrid.Items.Count > 0)
                {
                    Teilnehmer row = (Teilnehmer)Starterliste_DataGrid.Items[StarterIndex];
                    StartNummer_Aktuell.Text = row.Startnummer;
                    Riege_Aktuell.Content = row.Riege;
                    Klasse_Aktuell.Content = row.Klasse;
                    Verein_Aktuell.Content = row.Verein;
                    Disziplin_Aktuell.Content = row.Disziplin;
                    Starter_Aktuell.Content = row.Starter;
                }

                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
        }

        private void Starterliste_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Teilnehmer row = (Teilnehmer)Starterliste_DataGrid.SelectedItem;

            if(row != null)
            {
                if (row.Startnummer != null)
                {
                    Startnummer_Textbox.Text = row.Startnummer;
                }

                if (row.Riege != null)
                {
                    Riege_Textbox.Text = row.Riege;
                }

                if (row.Starter != null)
                {
                    Starter_Textbox.Text = row.Starter;
                }

                if (row.Verein != null)
                {
                    Verein_Textbox.Text = row.Verein;
                }

                if (row.Klasse != null)
                {
                    if(row.Klasse == NKL1.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = NKL1;
                    }

                    if (row.Klasse == NKL2.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = NKL2;
                    }

                    if (row.Klasse == AgeGroup11.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = AgeGroup11;
                    }

                    if (row.Klasse == AgeGroup12.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = AgeGroup12;
                    }

                    if (row.Klasse == AgeGroup13.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = AgeGroup13;
                    }

                    if (row.Klasse == Junior1.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = Junior1;
                    }

                    if (row.Klasse == Junior2.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = Junior2;
                    }

                    if (row.Klasse == Senior.Content.ToString())
                    {
                        Klasse_Combobox.SelectedItem = Senior;
                    }

                }

                if (row.Disziplin != null)
                {
                    if(row.Disziplin == DamenPaare.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = DamenPaare;
                    }

                    if (row.Disziplin == HerrenPaare.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = HerrenPaare;
                    }

                    if (row.Disziplin == DamenGruppe.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = DamenGruppe;
                    }

                    if (row.Disziplin == HerrenGruppe.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = HerrenGruppe;
                    }

                    if (row.Disziplin == mixedPaare.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = mixedPaare;
                    }

                    if (row.Disziplin == DamenPodest.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = DamenPodest;
                    }

                    if (row.Disziplin == HerrenPodest.Content.ToString())
                    {
                        Disziplin_Combobox.SelectedItem = HerrenPodest;
                    }

                }

                foreach(var element in Starterliste)
                {
                    if(row.Startnummer == element.Startnummer)
                    {
                        StartVorbereitung_TimePicker.Value = element.StartVorbereitung;
                        StartWettkampf_TimePicker.Value = element.StartWettkampf;
                    }
                }

            }

            else
            {
                Startnummer_Textbox.Text = null;
                Riege_Textbox.Text = null;
                Starter_Textbox.Text = null;
                Verein_Textbox.Text = null;
                Klasse_Combobox.SelectedItem = null;
                Disziplin_Combobox.SelectedItem = null;
                StartVorbereitung_TimePicker.Value = null;
                StartWettkampf_TimePicker.Value = null;
            }

            
        }

        private void NeuesTurnier_Button_Click(object sender, RoutedEventArgs e)
        {

            Importieren.IsEnabled = true;
            Exportieren.IsEnabled = true;

            Seite1.Visibility = Visibility.Hidden;
            Seite2.Visibility = Visibility.Visible;
        }

        private void TurnierÖffnen_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".json";
            openfile.Filter = "(.json)|*.json";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                using (StreamReader r = new StreamReader(openfile.FileName))
                {
                    string json = r.ReadToEnd();
                    Starterliste = JsonConvert.DeserializeObject<List<Teilnehmer>>(json);
                }

                if (Starterliste.Count > 0)
                {
                    if (Starterliste[0].Turniername != null)
                    {
                        Turniername_Label.Content = Starterliste[0].Turniername;
                        Turniername_LabelAktuell.Content = Starterliste[0].Turniername;
                        TurniernameNeu_TextBox.Text = Starterliste[0].Turniername;
                    }

                    if (Starterliste[0].Ort != null)
                    {
                        OrtNeu_TextBox.Text = Starterliste[0].Ort;
                    }

                    if (Starterliste[0].Datum != null)
                    {
                        WettkampfDatum_TimePicker.Value = Starterliste[0].Datum;
                    }

                    if (Starterliste[0].T1Name != null)
                    {
                        T1Name_Textbox.Text = Starterliste[0].T1Name;
                    }

                    if (Starterliste[0].T2Name != null)
                    {
                        T2Name_Textbox.Text = Starterliste[0].T2Name;
                    }

                    if (Starterliste[0].T3Name != null)
                    {
                        T3Name_Textbox.Text = Starterliste[0].T3Name;
                    }

                    if (Starterliste[0].T4Name != null)
                    {
                        T4Name_Textbox.Text = Starterliste[0].T4Name;
                    }

                    if (Starterliste[0].A1Name != null)
                    {
                        A1Name_Textbox.Text = Starterliste[0].A1Name;
                    }

                    if (Starterliste[0].A2Name != null)
                    {
                        A2Name_Textbox.Text = Starterliste[0].A2Name;
                    }

                    if (Starterliste[0].A3Name != null)
                    {
                        A3Name_Textbox.Text = Starterliste[0].A3Name;
                    }

                    if (Starterliste[0].A4Name != null)
                    {
                        A4Name_Textbox.Text = Starterliste[0].A4Name;
                    }

                    if (Starterliste[0].CJP != null)
                    {
                        CJP_Textbox.Text = Starterliste[0].CJP;
                    }

                    if (Starterliste[0].Schwierigkeitskampfrichter != null)
                    {
                        Schwierigkeitskampfrichter_Textbox.Text = Starterliste[0].Schwierigkeitskampfrichter;
                    }

                    Starterliste_DataGrid.Items.Clear();

                    foreach (var element in Starterliste)
                    {
                        Starterliste_DataGrid.Items.Add(element);
                    }

                    foreach (var element in Starterliste)
                    {
                        if (StartNummer_Aktuell.Text == element.Startnummer)
                        {

                            Technik1_Textbox.Text = element.Punkte.Technik1;
                            Technik2_Textbox.Text = element.Punkte.Technik2;
                            Technik3_Textbox.Text = element.Punkte.Technik3;
                            Technik4_Textbox.Text = element.Punkte.Technik4;
                            Artistik1_Textbox.Text = element.Punkte.Artistik1;
                            Artistik2_Textbox.Text = element.Punkte.Artistik2;
                            Artistik3_Textbox.Text = element.Punkte.Artistik3;
                            Artistik4_Textbox.Text = element.Punkte.Artistik4;
                            MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                            MinTechnik_Label.Content = element.Punkte.TechnikMin;
                            MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                            MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                            Technik_Label.Content = element.Punkte.TechnikGesamt;
                            Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                            GesamtWertung_Label.Content = element.Punkte.GWertung;
                            Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                            Abzüge_Textbox.Text = element.Punkte.Abzüge;

                            if (element.Platz > 0)
                            {
                                Platzierung_Label.Content = element.Platz;
                            }
                            else
                            {
                                Platzierung_Label.Content = "";
                            }

                            Rankingliste.Clear();

                            foreach (var klassenElement in Starterliste)
                            {

                                if (klassenElement.KlasseGesamt == element.KlasseGesamt)
                                {
                                    Rankingliste.Add(klassenElement);
                                }
                            }

                            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

                            //Setze Ranking ins Data Grid
                            RankingGrid.Items.Clear();
                            foreach (var rankingElement in Rankingliste)
                            {
                                if (rankingElement.Platz > 0)
                                {
                                    RankingGrid.Items.Add(rankingElement);
                                }
                            }

                            break;
                        }
                    }

                }
            }

            Seite1.Visibility = Visibility.Hidden;
            Seite2.Visibility = Visibility.Visible;
            Öffnen.IsEnabled = true;
            Speichern.IsEnabled = true;
            Importieren.IsEnabled = true;
            Exportieren.IsEnabled = true;

        }

        private void Beenden_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Exportieren_Click(object sender, RoutedEventArgs e)
        {
            _export.Exportieren(Turniername_Label.Content.ToString(), Starterliste_DataGrid);
        }

        private void Ändern_Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (Teilnehmer)Starterliste_DataGrid.SelectedItem;
            var index = Starterliste_DataGrid.SelectedIndex;

            if (item != null)
            {
                if (Startnummer_Textbox.Text != null)
                {
                    item.Startnummer = Startnummer_Textbox.Text;
                }

                if(Riege_Textbox.Text != null)
                {
                    item.Riege = Riege_Textbox.Text;
                }

                if (Starter_Textbox.Text != null)
                {
                    item.Starter = Starter_Textbox.Text;
                }

                if (Verein_Textbox.Text != null)
                {
                    item.Verein = Verein_Textbox.Text;
                }

                if (Klasse_Combobox.SelectedItem != null)
                {
                    item.Klasse = ((ComboBoxItem)Klasse_Combobox.SelectedItem).Content.ToString();
                }

                if (Disziplin_Combobox.SelectedItem != null)
                {
                    item.Disziplin = ((ComboBoxItem)Disziplin_Combobox.SelectedItem).Content.ToString();
                }

                if(StartVorbereitung_TimePicker.Text != null)
                {
                    item.StartVorbereitung = StartVorbereitung_TimePicker.Value;
                }

                if(StartWettkampf_TimePicker.Text != null)
                {
                    item.StartWettkampf = StartWettkampf_TimePicker.Value;
                }

                Starterliste.Insert(index, item);
                Starterliste.RemoveAt(index + 1);

                if(StartzeitNutzen_CheckBox.IsChecked == true)
                {
                    Starterliste = Starterliste.OrderBy(i => i.StartWettkampf).ToList();
                }
                
                Starterliste_DataGrid.Items.Clear();

                foreach (var element in Starterliste)
                {
                    Starterliste_DataGrid.Items.Add(element);
                }
            }

        }

        private void Löschen_Button_Click(object sender, RoutedEventArgs e)
        {
            var item = Starterliste_DataGrid.SelectedItem;
            Starterliste_DataGrid.Items.Remove(item);

            Starterliste.Remove((Teilnehmer)item);

            StartVorbereitung_TimePicker.Text = "";
            StartWettkampf_TimePicker.Text = "";

            if(StartzeitNutzen_CheckBox.IsChecked == true)
            {
                Starterliste = Starterliste.OrderBy(i => i.StartWettkampf).ToList();
            }

            Starterliste_DataGrid.Items.Clear();

            foreach (var element in Starterliste)
            {
                Starterliste_DataGrid.Items.Add(element);
            }
        }

        private void ZurückTurnierButton_Click(object sender, RoutedEventArgs e)
        {
            Seite4.Visibility = Visibility.Hidden;
            Seite3.Visibility = Visibility.Visible;
        }

        private void WeiterTurnierButton_Click(object sender, RoutedEventArgs e)
        {
            Seite4.Visibility = Visibility.Hidden;
            Seite5.Visibility = Visibility.Visible;
            StarterIndex = 0;
            var starterVorhanden = false;


            if (Starterliste_DataGrid.Items.Count > 0)
            {
                Teilnehmer row = (Teilnehmer)Starterliste_DataGrid.Items[StarterIndex];
                StartNummer_Aktuell.Text = row.Startnummer;
                Riege_Aktuell.Content = row.Riege;
                Klasse_Aktuell.Content = row.Klasse;
                Verein_Aktuell.Content = row.Verein;
                Disziplin_Aktuell.Content = row.Disziplin;
                Starter_Aktuell.Content = row.Starter;
            }

            Rankingliste.Clear();

            foreach (var element in Starterliste)
            {
                if (element.KlasseGesamt == Starterliste[StarterIndex].KlasseGesamt)
                {
                    Rankingliste.Add(element);
                }
            }

            foreach (var element in Rankingliste)
            {
                if (element.Startnummer == StartNummer_Aktuell.Text.ToString())
                {
                    starterVorhanden = true;

                    if (element.Punkte != null)
                    {
                        Technik1_Textbox.Text = element.Punkte.Technik1;
                        Technik2_Textbox.Text = element.Punkte.Technik2;
                        Technik3_Textbox.Text = element.Punkte.Technik3;
                        Technik4_Textbox.Text = element.Punkte.Technik4;
                        Artistik1_Textbox.Text = element.Punkte.Artistik1;
                        Artistik2_Textbox.Text = element.Punkte.Artistik2;
                        Artistik3_Textbox.Text = element.Punkte.Artistik3;
                        Artistik4_Textbox.Text = element.Punkte.Artistik4;
                        MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                        MinTechnik_Label.Content = element.Punkte.TechnikMin;
                        MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                        MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                        Technik_Label.Content = element.Punkte.TechnikGesamt;
                        Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                        GesamtWertung_Label.Content = element.Punkte.GWertung;
                        Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                        Abzüge_Textbox.Text = element.Punkte.Abzüge;

                        if (element.Platz > 0)
                        {
                            Platzierung_Label.Content = element.Platz;
                        }
                        else
                        {
                            Platzierung_Label.Content = "";
                        }

                        break;
                    }

                }
            }

            if (!starterVorhanden)
            {
                Technik1_Textbox.Text = "";
                Technik2_Textbox.Text = "";
                Technik3_Textbox.Text = "";
                Technik4_Textbox.Text = "";
                Artistik1_Textbox.Text = "";
                Artistik2_Textbox.Text = "";
                Artistik3_Textbox.Text = "";
                Artistik4_Textbox.Text = "";
                Schwierigkeit_Textbox.Text = "";
                Abzüge_Textbox.Text = "";
                MaxTechnik_Label.Content = "";
                MinTechnik_Label.Content = "";
                MaxArtistik_Label.Content = "";
                MinArtistik_Label.Content = "";
                Technik_Label.Content = "";
                Artistik_Label.Content = "";
                GesamtWertung_Label.Content = "";
                Platzierung_Label.Content = "";
            }

            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

            //Setze Ranking ins Data Grid
            RankingGrid.Items.Clear();
            foreach (var element in Rankingliste)
            {
                if (element.Platz > 0)
                {
                    RankingGrid.Items.Add(element);
                }
            }

        }

        private void NaechsteStarter_Button_Click(object sender, RoutedEventArgs e)
        {
            StarterIndex++;
            var starterVorhanden = false;

            if (Starterliste.Count > StarterIndex )
            {
                StartNummer_Aktuell.Text = Starterliste[StarterIndex].Startnummer;
                Riege_Aktuell.Content = Starterliste[StarterIndex].Riege;
                Klasse_Aktuell.Content = Starterliste[StarterIndex].Klasse;
                Verein_Aktuell.Content = Starterliste[StarterIndex].Verein;
                Disziplin_Aktuell.Content = Starterliste[StarterIndex].Disziplin;
                Starter_Aktuell.Content = Starterliste[StarterIndex].Starter;
            }
            else
            {
                StarterIndex--;
            }

            Rankingliste.Clear();

            foreach(var element in Starterliste)
            {
                if(element.KlasseGesamt == Starterliste[StarterIndex].KlasseGesamt)
                {
                    Rankingliste.Add(element);
                }
            }

            foreach (var element in Rankingliste)
            {
                if (element.Startnummer == StartNummer_Aktuell.Text.ToString())
                {
                    starterVorhanden = true;

                    if(element.Punkte != null)
                    {
                        Technik1_Textbox.Text = element.Punkte.Technik1;
                        Technik2_Textbox.Text = element.Punkte.Technik2;
                        Technik3_Textbox.Text = element.Punkte.Technik3;
                        Technik4_Textbox.Text = element.Punkte.Technik4;
                        Artistik1_Textbox.Text = element.Punkte.Artistik1;
                        Artistik2_Textbox.Text = element.Punkte.Artistik2;
                        Artistik3_Textbox.Text = element.Punkte.Artistik3;
                        Artistik4_Textbox.Text = element.Punkte.Artistik4;
                        MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                        MinTechnik_Label.Content = element.Punkte.TechnikMin;
                        MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                        MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                        Technik_Label.Content = element.Punkte.TechnikGesamt;
                        Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                        GesamtWertung_Label.Content = element.Punkte.GWertung;
                        Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                        Abzüge_Textbox.Text = element.Punkte.Abzüge;

                        if(element.Platz > 0)
                        {
                            Platzierung_Label.Content = element.Platz;
                        }
                        else
                        {
                            Platzierung_Label.Content = "";
                        }
                        
                        break;
                    }

                }
            }

            if (!starterVorhanden)
            {
                Technik1_Textbox.Text = "";
                Technik2_Textbox.Text = "";
                Technik3_Textbox.Text = "";
                Technik4_Textbox.Text = "";
                Artistik1_Textbox.Text = "";
                Artistik2_Textbox.Text = "";
                Artistik3_Textbox.Text = "";
                Artistik4_Textbox.Text = "";
                Schwierigkeit_Textbox.Text = "";
                Abzüge_Textbox.Text = "";
                MaxTechnik_Label.Content = "";
                MinTechnik_Label.Content = "";
                MaxArtistik_Label.Content = "";
                MinArtistik_Label.Content = "";
                Technik_Label.Content = "";
                Artistik_Label.Content = "";
                GesamtWertung_Label.Content = "";
                Platzierung_Label.Content = "";
            }

            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

            //Setze Ranking ins Data Grid
            RankingGrid.Items.Clear();
            foreach (var element in Rankingliste)
            {
                if (element.Platz > 0)
                {
                    RankingGrid.Items.Add(element);
                }
            }
        }

        private void VorherigeStarter_Button_Click(object sender, RoutedEventArgs e)
        {
            StarterIndex--;
            var starterVorhanden = false;

            if (StarterIndex >= 0)
            {
                StartNummer_Aktuell.Text = Starterliste[StarterIndex].Startnummer;
                Riege_Aktuell.Content = Starterliste[StarterIndex].Riege;
                Klasse_Aktuell.Content = Starterliste[StarterIndex].Klasse;
                Verein_Aktuell.Content = Starterliste[StarterIndex].Verein;
                Disziplin_Aktuell.Content = Starterliste[StarterIndex].Disziplin;
                Starter_Aktuell.Content = Starterliste[StarterIndex].Starter;
            }
            else
            {
                StarterIndex++;
            }

            Rankingliste.Clear();

            foreach (var element in Starterliste)
            {
                if (element.KlasseGesamt == Starterliste[StarterIndex].KlasseGesamt)
                {
                   Rankingliste.Add(element);
                }
            }

            foreach (var element in Rankingliste)
            {
                if (element.Startnummer == StartNummer_Aktuell.Text.ToString())
                {
                    starterVorhanden = true;

                    if(element.Punkte != null)
                    {
                        Technik1_Textbox.Text = element.Punkte.Technik1;
                        Technik2_Textbox.Text = element.Punkte.Technik2;
                        Technik3_Textbox.Text = element.Punkte.Technik3;
                        Technik4_Textbox.Text = element.Punkte.Technik4;
                        Artistik1_Textbox.Text = element.Punkte.Artistik1;
                        Artistik2_Textbox.Text = element.Punkte.Artistik2;
                        Artistik3_Textbox.Text = element.Punkte.Artistik3;
                        Artistik4_Textbox.Text = element.Punkte.Artistik4;
                        MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                        MinTechnik_Label.Content = element.Punkte.TechnikMin;
                        MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                        MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                        Technik_Label.Content = element.Punkte.TechnikGesamt;
                        Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                        GesamtWertung_Label.Content = element.Punkte.GWertung;
                        Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                        Abzüge_Textbox.Text = element.Punkte.Abzüge;

                        if (element.Platz > 0)
                        {
                            Platzierung_Label.Content = element.Platz;
                        }
                        else
                        {
                            Platzierung_Label.Content = "";
                        }

                        break;
                    }

                }
            }

            if (!starterVorhanden)
            {
                Technik1_Textbox.Text = "";
                Technik2_Textbox.Text = "";
                Technik3_Textbox.Text = "";
                Technik4_Textbox.Text = "";
                Artistik1_Textbox.Text = "";
                Artistik2_Textbox.Text = "";
                Artistik3_Textbox.Text = "";
                Artistik4_Textbox.Text = "";
                Schwierigkeit_Textbox.Text = "";
                Abzüge_Textbox.Text = "";
                MaxTechnik_Label.Content = "";
                MinTechnik_Label.Content = "";
                MaxArtistik_Label.Content = "";
                MinArtistik_Label.Content = "";
                Technik_Label.Content = "";
                Artistik_Label.Content = "";
                GesamtWertung_Label.Content = "";
                Platzierung_Label.Content = "";
            }

            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

            //Setze Ranking ins Data Grid
            RankingGrid.Items.Clear();
            foreach (var element in Rankingliste)
            {
                if (element.Platz > 0)
                {
                    RankingGrid.Items.Add(element);
                }
            }

        }

        private void Berechnen_Button_Click(object sender, RoutedEventArgs e)
        {
            double TechnikGesamt = 0;
            List<double> technikList = new List<double>();
            double ArtistikGesamt = 0;
            List<double> artistikList = new List<double>();
            MaxTechnik_Label.Content = "";
            MinTechnik_Label.Content = "";
            MaxArtistik_Label.Content = "";
            MinArtistik_Label.Content = "";
            var teilnehmer = new Teilnehmer();

            //Finde aktuellen Teilnehmer
            foreach(var element in Starterliste)
            {
                if(element.Startnummer == StartNummer_Aktuell.Text.ToString())
                {
                    teilnehmer = element;
                    break;
                }
            }

            //Technik Punkte
            if(Technik1_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Technik1_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Technik1 = eingabe;
                    technikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Technik 1.");
                }
            }

            if (Technik2_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Technik2_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Technik2 = eingabe;
                    technikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Technik 2.");
                }
            }

            if (Technik3_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Technik3_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Technik3 = eingabe;
                    technikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Technik 3.");
                }
            }

            if (Technik4_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Technik4_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Technik4 = eingabe;
                    technikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Technik 4.");
                }
            }

            //Alle Technik Punkte vergeben
            if(technikList.Count == 4)
            {
                technikList.Sort();
                TechnikGesamt = ((technikList[1] + technikList[2]) / 2) * 2;
                MaxTechnik_Label.Content = technikList[3];
                teilnehmer.Punkte.TechnikMax = technikList[3].ToString();
                MinTechnik_Label.Content = technikList[0];
                teilnehmer.Punkte.TechnikMin = technikList[0].ToString();
            }
            else
            {
                double technikSumme = 0;
                foreach(double element in technikList)
                {
                    technikSumme = technikSumme + element;
                }

                if(technikSumme > 0)
                {
                    TechnikGesamt = (technikSumme / technikList.Count) * 2;
                }
                

            }
            
            Technik_Label.Content = TechnikGesamt.ToString();
            teilnehmer.Punkte.TechnikGesamt = TechnikGesamt.ToString();

            //Artistik Punkte
            if (Artistik1_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Artistik1_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Artistik1 = eingabe;
                    artistikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Artistik 1.");
                }
            }

            if (Artistik2_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Artistik2_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Artistik2 = eingabe;
                    artistikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Artistik 2.");
                }
            }

            if (Artistik3_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Artistik3_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Artistik3 = eingabe;
                    artistikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Artistik 3.");
                }
            }

            if (Artistik4_Textbox.Text != "")
            {
                try
                {
                    var eingabe = Artistik4_Textbox.Text.Replace(".", ",");
                    teilnehmer.Punkte.Artistik4 = eingabe;
                    artistikList.Add(Convert.ToDouble(eingabe));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Artistik 4.");
                }
            }

            //Alle Artistik Punkte vergeben
            if (artistikList.Count == 4)
            {
                artistikList.Sort();
                ArtistikGesamt = ((artistikList[1] + artistikList[2]) / 2);
                MaxArtistik_Label.Content = artistikList[3];
                teilnehmer.Punkte.ArtistikMax = artistikList[3].ToString();
                MinArtistik_Label.Content = artistikList[0];
                teilnehmer.Punkte.ArtistikMin = artistikList[0].ToString();
            }
            else
            {
                double artistikSumme = 0;
                foreach (double element in artistikList)
                {
                    artistikSumme = artistikSumme + element;
                }

                if(artistikSumme > 0)
                {
                    ArtistikGesamt = (artistikSumme / artistikList.Count);
                }
                

            }

            Artistik_Label.Content = ArtistikGesamt.ToString();
            teilnehmer.Punkte.ArtistikGesamt = ArtistikGesamt.ToString();

            //Schwierigkeit und Abzüge
            double ergebnis = 0;
            if((Schwierigkeit_Textbox.Text != "") && (Abzüge_Textbox.Text != ""))
            {
                try
                {
                    ergebnis = TechnikGesamt + ArtistikGesamt + Convert.ToDouble(Schwierigkeit_Textbox.Text) - Convert.ToDouble(Abzüge_Textbox.Text);
                    teilnehmer.Punkte.Schwierigkeit = Schwierigkeit_Textbox.Text;
                    teilnehmer.Punkte.Abzüge = Abzüge_Textbox.Text;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Schwrierigkeit oder Abzüge");
                }
            }

            if ((Schwierigkeit_Textbox.Text != "") && (Abzüge_Textbox.Text == ""))
            {
                try
                {
                    ergebnis = TechnikGesamt + ArtistikGesamt + Convert.ToDouble(Schwierigkeit_Textbox.Text);
                    teilnehmer.Punkte.Schwierigkeit = Schwierigkeit_Textbox.Text;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Schwrierigkeit oder Abzüge");
                }
            }

            if ((Schwierigkeit_Textbox.Text == "") && (Abzüge_Textbox.Text != ""))
            {
                try
                {
                    ergebnis = TechnikGesamt + ArtistikGesamt - Convert.ToDouble(Abzüge_Textbox.Text);
                    teilnehmer.Punkte.Abzüge = Abzüge_Textbox.Text;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Schwrierigkeit oder Abzüge");
                }
            }

            if ((Schwierigkeit_Textbox.Text == "") && (Abzüge_Textbox.Text == ""))
            {
                try
                {
                    ergebnis = TechnikGesamt + ArtistikGesamt;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Falsche Eingabe bei Schwrierigkeit oder Abzüge");
                }
            }

            GesamtWertung_Label.Content = ergebnis.ToString();
            teilnehmer.Punkte.GWertung = ergebnis.ToString();
            teilnehmer.PunkteGesamt = ergebnis;

            //Fülle Rankingliste nur mit den Startern einer Klasse und Disziplin
            Rankingliste.Clear();
            foreach(var starter in Starterliste)
            {
                if (starter.KlasseGesamt == teilnehmer.KlasseGesamt)
                {
                    Rankingliste.Add(starter);
                }
            }

            //Erhalte aktuelle Platzierung
            Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();
            var rankingCounter = 1;
            foreach(var element in Rankingliste)
            {              
                if (teilnehmer.Startnummer == element.Startnummer)
                {
                    element.Platz = rankingCounter;
                    Platzierung_Label.Content = rankingCounter.ToString();
                    break;
                }

                rankingCounter++;
            }


            //Setze Ranking ins Data Grid
            RankingGrid.Items.Clear();
            foreach(var element in Rankingliste)
            {
                if(element.Platz > 0)
                {
                    RankingGrid.Items.Add(element);
                }
            }

            saveObject.Autosave(Starterliste);

        }

        private void ZurückSeite3Button_Click(object sender, RoutedEventArgs e)
        {
            Seite5.Visibility = Visibility.Hidden;
            Seite4.Visibility = Visibility.Visible;
        }

        private void StartNummer_Aktuell_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var starterVorhanden = false;

                foreach (var starter in Starterliste)
                {
                    if(starter.Startnummer == StartNummer_Aktuell.Text)
                    {
                        Riege_Aktuell.Content = starter.Riege;
                        Klasse_Aktuell.Content = starter.Klasse;
                        Verein_Aktuell.Content = starter.Verein;
                        Disziplin_Aktuell.Content = starter.Disziplin;
                        Starter_Aktuell.Content = starter.Starter;

                        Rankingliste.Clear();

                        foreach (var element in Starterliste)
                        {
                            if (element.KlasseGesamt == starter.KlasseGesamt)
                            { 
                                 Rankingliste.Add(element);
                            }
                        }

                        break;
                    }

                }
            

                foreach (var element in Rankingliste)
                {
                    if (element.Startnummer == StartNummer_Aktuell.Text.ToString())
                    {
                        starterVorhanden = true;

                        if (element.Punkte != null)
                        {
                            Technik1_Textbox.Text = element.Punkte.Technik1;
                            Technik2_Textbox.Text = element.Punkte.Technik2;
                            Technik3_Textbox.Text = element.Punkte.Technik3;
                            Technik4_Textbox.Text = element.Punkte.Technik4;
                            Artistik1_Textbox.Text = element.Punkte.Artistik1;
                            Artistik2_Textbox.Text = element.Punkte.Artistik2;
                            Artistik3_Textbox.Text = element.Punkte.Artistik3;
                            Artistik4_Textbox.Text = element.Punkte.Artistik4;
                            MaxTechnik_Label.Content = element.Punkte.TechnikMax;
                            MinTechnik_Label.Content = element.Punkte.TechnikMin;
                            MaxArtistik_Label.Content = element.Punkte.ArtistikMax;
                            MinArtistik_Label.Content = element.Punkte.ArtistikMin;
                            Technik_Label.Content = element.Punkte.TechnikGesamt;
                            Artistik_Label.Content = element.Punkte.ArtistikGesamt;
                            GesamtWertung_Label.Content = element.Punkte.GWertung;
                            Schwierigkeit_Textbox.Text = element.Punkte.Schwierigkeit;
                            Abzüge_Textbox.Text = element.Punkte.Abzüge;

                            if (element.Platz > 0)
                            {
                                Platzierung_Label.Content = element.Platz;
                            }
                            else
                            {
                                Platzierung_Label.Content = "";
                            }

                            break;
                        }

                    }
                }

                if (!starterVorhanden)
                {
                    Technik1_Textbox.Text = "";
                    Technik2_Textbox.Text = "";
                    Technik3_Textbox.Text = "";
                    Technik4_Textbox.Text = "";
                    Artistik1_Textbox.Text = "";
                    Artistik2_Textbox.Text = "";
                    Artistik3_Textbox.Text = "";
                    Artistik4_Textbox.Text = "";
                    Schwierigkeit_Textbox.Text = "";
                    Abzüge_Textbox.Text = "";
                    MaxTechnik_Label.Content = "";
                    MinTechnik_Label.Content = "";
                    MaxArtistik_Label.Content = "";
                    MinArtistik_Label.Content = "";
                    Technik_Label.Content = "";
                    Artistik_Label.Content = "";
                    GesamtWertung_Label.Content = "";
                    Platzierung_Label.Content = "";
                }

                Rankingliste = Rankingliste.OrderByDescending(i => i.Punkte.GWertung).ToList();

                //Setze Ranking ins Data Grid
                RankingGrid.Items.Clear();
                foreach (var element in Rankingliste)
                {
                    if (element.Platz > 0)
                    {
                        RankingGrid.Items.Add(element);
                    }
                }

            }
        }

        private void StartzeitNutzen_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            StartVorbereitung_TimePicker.IsEnabled = true;
            StartWettkampf_TimePicker.IsEnabled = true;
        }

        private void StartzeitNutzen_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            StartVorbereitung_TimePicker.IsEnabled = false;
            StartWettkampf_TimePicker.IsEnabled = false;
        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            if (Starterliste.Count > 0)
            {
                Starterliste[0].Turniername = TurniernameNeu_TextBox.Text;
                Starterliste[0].Ort = OrtNeu_TextBox.Text;
                Starterliste[0].Datum = WettkampfDatum_TimePicker.Value;
                Starterliste[0].T1Name = T1Name_Textbox.Text;
                Starterliste[0].T2Name = T2Name_Textbox.Text;
                Starterliste[0].T3Name = T3Name_Textbox.Text;
                Starterliste[0].T4Name = T4Name_Textbox.Text;
                Starterliste[0].A1Name = A1Name_Textbox.Text;
                Starterliste[0].A2Name = A2Name_Textbox.Text;
                Starterliste[0].A3Name = A3Name_Textbox.Text;
                Starterliste[0].A4Name = A4Name_Textbox.Text;
                Starterliste[0].CJP = CJP_Textbox.Text;
                Starterliste[0].Schwierigkeitskampfrichter = Schwierigkeitskampfrichter_Textbox.Text;
            }
            else
            {

                var neuerStarter = new Teilnehmer
                {
                    Turniername = TurniernameNeu_TextBox.Text,
                    Ort = OrtNeu_TextBox.Text,
                    Datum = WettkampfDatum_TimePicker.Value,
                    T1Name = T1Name_Textbox.Text,
                    T2Name = T2Name_Textbox.Text,
                    T3Name = T3Name_Textbox.Text,
                    T4Name = T4Name_Textbox.Text,
                    A1Name = A1Name_Textbox.Text,
                    A2Name = A2Name_Textbox.Text,
                    A3Name = A3Name_Textbox.Text,
                    A4Name = A4Name_Textbox.Text,
                    CJP = CJP_Textbox.Text,
                    Schwierigkeitskampfrichter = Schwierigkeitskampfrichter_Textbox.Text
                };

            }

            saveObject.SaveTurnier(Turniername_Label.Content.ToString(), Starterliste);
        }

        private void Drucken_Click(object sender, RoutedEventArgs e)
        {
            DruckGrid.Visibility = Visibility.Visible;
            TurniernameDruck_Label.Content = Turniername_LabelAktuell.Content;

            Rankingliste.Clear();
            foreach (var starter in Starterliste)
            {
                if (starter.KlasseGesamt == 1)
                {
                    Rankingliste.Add(starter);
                }
            }

            Rankingliste = Rankingliste.OrderByDescending(i => i.Platz).ToList();

            foreach (var element in Rankingliste)
            {
                if (element.Platz > 0)
                {
                    ResultGrid.Items.Add(element);
                }
            }

            ResultGrid.Items.Add(new Teilnehmer());

            Rankingliste.Clear();
            foreach (var starter in Starterliste)
            {
                if (starter.KlasseGesamt == 2)
                {
                    Rankingliste.Add(starter);
                }
            }

            Rankingliste = Rankingliste.OrderByDescending(i => i.Platz).ToList();

            foreach (var element in Rankingliste)
            {
                if (element.Platz > 0)
                {
                    ResultGrid.Items.Add(element);
                }
            }

            ResultGrid.Items.Add(new Teilnehmer());

            Print_WPF_Preview(DruckGrid);
        }

        private void ZurückSeite2_Button_Click(object sender, RoutedEventArgs e)
        {
            Seite3.Visibility = Visibility.Hidden;
            Seite2.Visibility = Visibility.Visible;
        }

        private void WeiterSeite3_Button_Click(object sender, RoutedEventArgs e)
        {
            Seite3.Visibility = Visibility.Hidden;
            Seite4.Visibility = Visibility.Visible;
        }

        public void Print_WPF_Preview(FrameworkElement wpf_Element)
        {

            //------------< WPF_Print_current_Window >------------

            //--< create xps document >--

            if (File.Exists("printPreview.xps"))
            {
                File.Delete("printPreview.xps");
            }

            XpsDocument doc = new XpsDocument("printPreview.xps", FileAccess.ReadWrite);

            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();

            preview_Document.BeginBatchWrite();

            preview_Document.Write(wpf_Element);  //*this or wpf xaml control

            preview_Document.EndBatchWrite();

            //--</ create xps document >--

            //var doc2 = new XpsDocument("Druckausgabe.xps", FileAccess.Read);

            FixedDocumentSequence preview = doc.GetFixedDocumentSequence();


            var window = new Window();

            window.Content = new DocumentViewer
            { Document = preview };

            window.ShowDialog();


            doc.Close();

            //------------</ WPF_Print_current_Window >------------

        }
    }
}
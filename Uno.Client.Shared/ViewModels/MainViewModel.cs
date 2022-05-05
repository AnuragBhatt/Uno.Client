using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Uno.Client.Notification;
using Windows.UI;

namespace Uno.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
           
        }

        public string SearchText
        {
            get { return GetValue(() => SearchText); }
            set { SetValue(() => SearchText, value); }
        }
        public bool IsSearchEnabled
        {
            get { return GetValue(() => IsSearchEnabled); }
            set { SetValue(() => IsSearchEnabled, value); }
        }
        public async Task LoadAsync()
        {
            SearchText = "Initial text";
            await Task.CompletedTask;

            CreateListView3();
        }
        public void GetScanDetails()
        {
            Console.WriteLine("in GetScanDetails");
            var result = ReadResource("JsonScanDetails.txt");
            Console.WriteLine("able to read file");
            //ScansData3 = ScansData3?.Select(s => { s.ScanDetailCells?.Clear(); return s; }).ToList();
            foreach (var s in ScansData3)
            {
                s.ScanDetailCells.Clear();
            }
            Console.WriteLine("scan details changed..");
            var dt = JsonConvert.DeserializeObject<DataTable>(result);
            //ScansDataSelected.ScanDetailCells = result;
            var res = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                //var rowC = new ScanRow(); ;// new List<string>(); //string.Empty;
                foreach (DataColumn col in dt.Columns)
                {
                    string fieldValue = "";

                    if (row[col] != DBNull.Value)
                        fieldValue = row[col].ToString();

                    fieldValue = fieldValue.PadRight(col.ColumnName.Length);

                    //rowC += fieldValue + "\t\t";//row[col] + "\t";
                    //ScansDataSelected.ScanDetailCells.Add(fieldValue);
                    res.Add(fieldValue);
                }
                //info.Add(rowC);
            }
            ScansDataSelected.ScanDetailCells = new ObservableCollection<string>(res);
            Console.WriteLine("foreach loop completed..");
        }
        void CreateListView()
        {
            Console.WriteLine("fileText: ");
            var result = ReadResource("JsonText.txt");
            //Console.WriteLine(result);
            var dt = JsonConvert.DeserializeObject<DataTable>(result);
            Console.WriteLine("Write to dataTable");
            //Console.WriteLine(dt.HasErrors);
            Console.WriteLine(dt.Rows.Count);

            var info = new List<string>();

            //info.Add(dt.Columns.);
            string header = string.Empty;
            foreach (DataColumn col in dt.Columns)
            {
                header += col.ColumnName + "\t\t";
            }
            info.Add(header);

            string rowF = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                string rowC = string.Empty;
                foreach (DataColumn col in dt.Columns)
                {
                    string fieldValue = "";

                    if (row[col] != DBNull.Value)
                        fieldValue = row[col].ToString();

                    fieldValue = fieldValue.PadRight(col.ColumnName.Length);

                    rowC += fieldValue + "\t\t";//row[col] + "\t";
                }
                info.Add(rowC);
            }


            ScansData = new ObservableCollection<string>(info);
            foreach (var c in ScansData)
            {
                Console.WriteLine(c);
            }
        }
        void CreateListView2()
        {
            Console.WriteLine("fileText: ");
            var result = ReadResource("JsonText.txt");
            //Console.WriteLine(result);
            var dt = JsonConvert.DeserializeObject<DataTable>(result);
            Console.WriteLine("Write to dataTable");
            //Console.WriteLine(dt.HasErrors);
            Console.WriteLine(dt.Rows.Count);

            var info = new List<IEnumerable<string>>();

            //info.Add(dt.Columns.);
            //string header = string.Empty;
            var header = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                //header += col.ColumnName + "\t\t";
                header.Add(col.ColumnName);
            }
            info.Add(header);

            foreach (DataRow row in dt.Rows)
            {
                var rowC = new List<string>(); //string.Empty;
                foreach (DataColumn col in dt.Columns)
                {
                    string fieldValue = "";

                    if (row[col] != DBNull.Value)
                        fieldValue = row[col].ToString();

                    fieldValue = fieldValue.PadRight(col.ColumnName.Length);

                    //rowC += fieldValue + "\t\t";//row[col] + "\t";
                    rowC.Add(fieldValue);
                }
                info.Add(rowC);
            }


            ScansData2 = new ObservableCollection<IEnumerable<string>>(info);
            foreach (var c in ScansData)
            {
                Console.WriteLine(c);
            }
        }
        void CreateListView3()
        {
            Console.WriteLine("fileText: ");
            var result = ReadResource("JsonText.txt");
            //Console.WriteLine(result);
            var dt = JsonConvert.DeserializeObject<DataTable>(result);
            Console.WriteLine("Write to dataTable");
            //Console.WriteLine(dt.HasErrors);
            Console.WriteLine(dt.Rows.Count);

            var info = new List<ScanRow>();

            //info.Add(dt.Columns.);
            //string header = string.Empty;
            var header = new List<string>();//new ScanRow();//
            foreach (DataColumn col in dt.Columns)
            {
                //header += col.ColumnName + "\t\t";
                //header.ScanCells.Add(col.ColumnName);
                header.Add(col.ColumnName);
            }
            //header.RowColor = GetSolidColorBrush("#001A72");
            //header.RowColor = new SolidColorBrush(Colors.Blue);

            //info.Add(header);
            ScansHeader = new ObservableCollection<string>(header);
            //ScansHeader = new ObservableCollection<ScanRow>( new List<ScanRow> { new ScanRow() { ScanCells = header }});

            foreach (DataRow row in dt.Rows)
            {
                var rowC = new ScanRow(); ;// new List<string>(); //string.Empty;
                foreach (DataColumn col in dt.Columns)
                {
                    string fieldValue = "";

                    if (row[col] != DBNull.Value)
                        fieldValue = row[col].ToString();

                    fieldValue = fieldValue.PadRight(col.ColumnName.Length);

                    //rowC += fieldValue + "\t\t";//row[col] + "\t";
                    rowC.ScanCells.Add(fieldValue);
                }
                info.Add(rowC);
            }


            ScansData3 = new ObservableCollection<ScanRow>(info);
            foreach (var c in ScansData)
            {
                Console.WriteLine(c);
            }
        }
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
        public ObservableCollection<string> ScansData
        {
            get { return GetValue(() => ScansData); }
            set { SetValue(() => ScansData, value); }
        }
        public ObservableCollection<IEnumerable<string>> ScansData2
        {
            get { return GetValue(() => ScansData2); }
            set { SetValue(() => ScansData2, value); }
        }
        public ObservableCollection<ScanRow> ScansData3
        {
            get { return GetValue(() => ScansData3); }
            set { SetValue(() => ScansData3, value);
                
            }
        }
        //public ObservableCollection<ScanRow> ScansHeader
        public ObservableCollection<string> ScansHeader
        {
            get { return GetValue(() => ScansHeader); }
            set { SetValue(() => ScansHeader, value); }
        }
        public ScanRow ScansDataSelected
        {
            get { return GetValue(() => ScansDataSelected); }
            set { SetValue(() => ScansDataSelected, value);
                Console.WriteLine("setting ScansDataSelected");
                GetScanDetails();
            }
        }
        //public IEnumerable<string> ScanRow { get; set; }

        public async Task ChangeAndEnableTextBoxAsync()
        {
            IsSearchEnabled = true;
            SearchText = "Changed text";
            await Task.CompletedTask;
        }
        static string ReadResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly(); //typeof(Program).Assembly;
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(f => f.Contains(fileName));

            if (!string.IsNullOrEmpty(resourceName))
            {
                using (var s = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
                {
                    return s.ReadToEnd();
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to find resource {fileName} in {assembly}");
            }
        }
        
    }
    public class ScanRow: PropertyChangedNotification
    {
        public ScanRow()
        {
            ScanDetailCells = new ObservableCollection<string>();
        }
        public List<string> ScanCells { get; set; } = new List<string>();
        //public List<string> ScanDetailCells { get; set; } = new List<string>();
        public ObservableCollection<string> ScanDetailCells
        {
            get { return GetValue(() => ScanDetailCells); }
            set
            {
                SetValue(() => ScanDetailCells, value);
            }
        }
        public SolidColorBrush RowColor = new SolidColorBrush(Colors.Transparent);
    }
    public class ScanCell
    {
        public List<string> ScanCells { get; set; } = new List<string>();
    }
}

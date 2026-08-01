namespace DylanBrown_QuoteAppPt1
{
    using Microsoft.Maui.Layouts;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;

    public class QuoteData
    {
        public string fileName = "Quotes.json";
        public List<string> quotesList = new List<string>();

    public class DataModel
    {

        public string fileName = "Quotes.json";
        public List<QuoteData> quotesList = new List<QuoteData>();

        public Random randomInt = new Random();

        public void Save(string newQuote, string newAuthor)
        {            
            var localFolder = FileSystem.Current.AppDataDirectory;
            var filePath = Path.Combine(localFolder, fileName);
            Debug.WriteLine(filePath);

            var content_json = JsonConvert.SerializeObject(quotesList);

            File.WriteAllText(filePath, content_json);            
        }

        public void Load()
        {
            var localFolder = FileSystem.Current.AppDataDirectory;
            var filePath = Path.Combine(localFolder, fileName);
            Debug.WriteLine(filePath);

            var content_json = File.ReadAllText(filePath);
            List<string> content = JsonConvert.DeserializeObject<List<string>>(content_json);

            
            if (!(content.Count() == 0))
            {
                quotesList = content;
            }
        }

        public void CreateFile()
        {
            string quoteListEntry = $"{quote} - {author}";

            quotesList.Add(quoteListEntry);
        }
    }

    public partial class MainPage : ContentPage
    {

        public DataModel data = new DataModel();
        public QuoteData quoteData = new QuoteData();

        public MainPage()
        {   
            InitializeComponent();

            try
            {
                data.Load();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception caught: {e}");
                data.CreateFile();
                data.Load();
            }

        }
        private void EnterQuote_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(quoteEntry.Text) && !String.IsNullOrWhiteSpace(authorEntry.Text))
            {
                string quoteText = $"\"{quoteEntry.Text}\"";
                string author = authorEntry.Text;

                data.Save(quoteEntry.Text, authorEntry.Text);

                quoteEntry.Text = "";
                authorEntry.Text = "";
            }
            else
            {
                DisplayAlertAsync("Data Entry Error", "Please fill in all entries", "Accept");
            }
        }

        private void RandomQuote_Clicked(object sender, EventArgs e)
        {
            if (data.quotesList.Count > 0)
            {
                int quoteNumber = data.randomInt.Next(data.quotesList.Count);
                var randomQuote = data.quotesList[quoteNumber];
                randomQuoteDisplay.Text = randomQuote.ToString();
            }
            else
            {
                DisplayAlertAsync("Null Quote Error", "No quotes found", "Accept");
            }            
        }
    }
}

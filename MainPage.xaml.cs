namespace DylanBrown_QuoteAppPt1
{
    using Microsoft.Maui.Layouts;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;

    public class Quotes
    {
        public string quote { get; set; }
        public string author { get; set; }

        public Quotes(string quote, string author)
        {
            this.quote = quote;
            this.author = author;
        }
    }


    public class FileData
    {
        public string fileName = "Quotes.json";
        public List<Quotes> quotesList = new List<Quotes>();

        public Random randomInt = new Random();

        public void Save()
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

            string content_json = File.ReadAllText(filePath);
            quotesList = JsonConvert.DeserializeObject<List<Quotes>>(content_json);

            if (quotesList.Count == 0)
            {
                quotesList = new List<Quotes>();
            }
        }

        public void EnterQuote(string quote, string author)
        {
            quotesList.Add(new Quotes (quote, author));
        }
    }

    public partial class MainPage : ContentPage
    {

        public FileData file = new FileData();

        public MainPage()
        {
            InitializeComponent();

            try
            {
                file.Load();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception caught: {e}");
                file.Save();
                file.Load();
            }

        }
        private void EnterQuote_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(quoteEntry.Text) || !String.IsNullOrWhiteSpace(authorEntry.Text))
            {
                string quoteText = $"\"{quoteEntry.Text}\"";
                string author = authorEntry.Text;

                file.EnterQuote(quoteText, author);
                file.Save();

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
            if (file.quotesList.Count > 0)
            {
                int quoteNumber = file.randomInt.Next(file.quotesList.Count);
                Quotes randomQuote = file.quotesList[quoteNumber];
                string quoteName = randomQuote.quote;
                string authorName = randomQuote.author;
                randomQuoteDisplay.Text = $"{quoteName} - {authorName}";
            }
            else
            {
                DisplayAlertAsync("Null Quote Error", "No quotes found", "Accept");
            }
        }
    }
}

namespace DylanBrown_QuoteAppPt1
{
    using Microsoft.Maui.Layouts;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;

    public class Quote
    {
        public string fileName = "Quotes.json";
        public List<string> quotesList = new List<string>();

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

            var content_json = File.ReadAllText(filePath);
            List<string> content = JsonConvert.DeserializeObject<List<string>>(content_json);


            if (!(content.Count() == 0))
            {
                quotesList = content;
            }
        }

        public void EnterQuote(string quote, string author)
        {
            string quoteListEntry = $"{quote} - {author}";

            quotesList.Add(quoteListEntry);
        }
    }

    public partial class MainPage : ContentPage
    {

        public Quote quote = new Quote();

        public MainPage()
        {
            InitializeComponent();

            try
            {
                quote.Load();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception caught: {e}");
                quote.Save();
                quote.Load();
            }

        }
        private void EnterQuote_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(quoteEntry.Text) || !String.IsNullOrWhiteSpace(authorEntry.Text))
            {
                string quoteText = $"\"{quoteEntry.Text}\"";
                string author = authorEntry.Text;

                quote.EnterQuote(quoteText, author);
                quote.Save();

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
            if (quote.quotesList.Count > 0)
            {
                int quoteNumber = quote.randomInt.Next(quote.quotesList.Count);
                string randomQuote = quote.quotesList[quoteNumber];
                randomQuoteDisplay.Text = randomQuote;
            }
            else
            {
                DisplayAlertAsync("Null Quote Error", "No quotes found", "Accept");
            }
        }
    }
}

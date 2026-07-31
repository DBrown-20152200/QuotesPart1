namespace DylanBrown_QuoteAppPt1
{
    using Microsoft.Maui.Layouts;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;

    public class Quote
    {
        public string fileName = "Quotes.txt";
        public string content = "\"Hello World\"";
        public List<string> quotesList = new List<string>();

        public Random randomInt = new Random();
    
        public void save()
        {
            var localFolder = FileSystem.Current.AppDataDirectory;
            var filePath = Path.Combine(localFolder, fileName);
            Debug.WriteLine(filePath);

            content = String.Join("\n", quotesList);

            File.WriteAllText(filePath, content);            
        }

        public void load()
        {
            var localFolder = FileSystem.Current.AppDataDirectory;
            var filePath = Path.Combine(localFolder, fileName);

            Debug.WriteLine(filePath);

            content = File.ReadAllText(filePath);
            
            if (!String.IsNullOrWhiteSpace(content))
            {
                quotesList.Add(content);
            }
        }

        public void enterQuote(string quote, string author)
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
                quote.load();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception caught: {e}");
                quote.save();
                quote.load();
            }

        }
        private void EnterQuote_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(quoteEntry.Text) || !String.IsNullOrWhiteSpace(authorEntry.Text))
            {
                string quoteText = $"\"{quoteEntry.Text}\"";
                string author = authorEntry.Text;

                quote.enterQuote(quoteText, author);
                quote.save();

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

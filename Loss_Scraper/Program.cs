using System;

namespace Loss_Scraper
{
    using HtmlAgilityPack;
    using CsvHelper;
    using System.IO;
    using System.Collections.Generic;
    using System.Globalization;
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HtmlWeb web = new HtmlWeb();
            FileStream csvfile = File.Open("C:/Users/Dominic Gravina/Documents/TLoss.csv",FileMode.Create);
            StreamWriter writer = new StreamWriter(csvfile);
            CsvWriter csv = new CsvWriter(writer,CultureInfo.InvariantCulture);
            List<Loss> losses = new List<Loss>();
            writer.WriteLine("Name,Date");
            for (int i=1; i<61; i++)
            {
                string Link = "https://ukr.warspotting.net/search/?belligerent=2&weapon=1&page=" + i;
                HtmlDocument doc = web.Load(Link);
                bool FirstFlag = false;
                foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//table[@id='vehicleList']//tr"))
                {
                    HtmlNode TName = row.SelectSingleNode(".//a[@class='vehicle-link ']");
                    HtmlNode TDate = row.SelectSingleNode(".//span[@class='d-none d-lg-inline']");
                    //Skip the first row
                    if (FirstFlag == true)
                    {
                        losses.Add(new Loss
                        {
                            Name = TName.InnerText,
                            Date = TDate.InnerText
                        });
                        writer.WriteLine(TName.InnerText + "," + TDate.InnerText);
                        Console.WriteLine(TName.InnerText + "\t" + TDate.InnerText);
                    }
                    else
                    {
                        FirstFlag = true;
                    }
                }
            }
            writer.Close();
            csvfile.Close();
            //csv.WriteRecords(losses);
        }
        public class Loss
        {
            public string Name { get; set; }
            public string Date { get; set; }
        }
    }
}

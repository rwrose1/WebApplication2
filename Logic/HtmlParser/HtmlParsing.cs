using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using NazdaqSearch.Models;

namespace NazdaqSearch.Logic.HtmlParser
{

	public class HtmlParsing
	{

		public static List<TitleAndLink> getTitlesAndLinks() {

			//Sets up HTML document for getting information
			HtmlWeb nazdaqSite = new HtmlWeb();
			HtmlDocument nazDoc = nazdaqSite.Load("https://www.nasdaq.com/options/");
			
			String dt = DateTime.Now.ToString("MM/dd/yyyy");

			List<TitleAndLink> toBeReturned = new List<TitleAndLink>();

			//Gets /a elements from Nazdaq/options/
			HtmlNodeCollection nodeCollection = nazDoc.DocumentNode.SelectNodes("//a[@href][contains(@class, 'fontS16px')]");

			foreach (HtmlNode item in nodeCollection) {

				String link = item.Attributes["href"].Value;
				
				HtmlNode titleNode = item.SelectSingleNode("b");
				String title = titleNode.InnerHtml;
				
				String condition = title.Split(' ')[0];
				
				if (condition == "Notable" || condition == "Noteworthy") 
				{
					toBeReturned.Add(new TitleAndLink(title, link, dt));
					//Console.WriteLine();
				}
			
			}

			return toBeReturned;
		
		}

		public static List<NDData> getData() 
		{
			List<NDData> toBeReturned = new List<NDData>();
			List<TitleAndLink> titleLinks = getTitlesAndLinks();

			foreach(TitleAndLink item in titleLinks)
			{
				/* Console.WriteLine("LINk = " + item.Link
					+ "\nTITLE = " + item.Title
					+ "\nTIME = " + item.Time); 
				*/ 
				HtmlWeb web = new HtmlWeb();

				HtmlDocument dataDoc = web.Load(item.Link);

				HtmlNode articleText = dataDoc.DocumentNode.SelectSingleNode("//*[@id=\"articleText\"]");
				
				List<HtmlNode> master = new List<HtmlNode>();

				//Console.WriteLine("\t" + articleText.InnerHtml);

				foreach (HtmlNode textNode in articleText.SelectNodes("p")) {

					HtmlNode test = textNode.SelectSingleNode("a");
					
					if (test != null) {
						foreach(HtmlNode toBeRemoved in textNode.SelectNodes("a")) 
							toBeRemoved.ParentNode.RemoveChild(toBeRemoved, true);
					} 
					
					test = textNode.SelectSingleNode("img");

					if (test != null) {
						foreach(HtmlNode toBeRemoved in textNode.SelectNodes("img")) 
							toBeRemoved.ParentNode.RemoveChild(toBeRemoved);
					}

					test = textNode.SelectSingleNode("p");

					if (test != null) {
						foreach(HtmlNode toBeRemoved in textNode.SelectNodes("p")) 
							toBeRemoved.ParentNode.RemoveChild(toBeRemoved);
					}

					test = textNode.SelectSingleNode("div");

					if (test != null) {
						foreach(HtmlNode toBeRemoved in textNode.SelectNodes("div")) 
							toBeRemoved.ParentNode.RemoveChild(toBeRemoved);
					}

					master.Add(textNode);

				}

				//toBeReturned.addAll(parseString(master))
				toBeReturned.AddRange(parseString(master, item));
			}

			return toBeReturned;
		}
		
		public static List<NDData> parseString(List<HtmlNode> cleanedNodes, TitleAndLink titleLink) {

			List<NDData> toBeReturned = new List<NDData>();

			foreach (HtmlNode item in cleanedNodes) {
				String baseText = item.InnerHtml;

				String[] paragraphs = System.Text.RegularExpressions.Regex.Split(item.InnerHtml, @"\s{4,}");

				foreach(String paragraph in paragraphs) {

					//Console.WriteLine("\tPARAGRAPH: " + paragraph);

					String[] splitData = paragraph.Split(':');

					if (splitData.Length <= 1) continue;

					String dataNeeded = splitData[1];

					String Symbol = dataNeeded.Substring(0, dataNeeded.IndexOf(")")).Replace(")", "").Replace(",", "").Trim();
					//Console.WriteLine("\tSYMBOL = " + Symbol);

					dataNeeded = dataNeeded.Remove(0, dataNeeded.IndexOf(")")).Replace(")", "").TrimStart();

					if (dataNeeded[0] == ',') 
					{
						//Console.WriteLine("The first char is a ,");
						dataNeeded = dataNeeded.Remove(0, 1);
						dataNeeded = dataNeeded.TrimStart();
					}

					dataNeeded = dataNeeded[0].ToString().ToUpper() + dataNeeded.Substring(1);

					//Console.WriteLine("\tDATA = " + dataNeeded + "\n");
					
					NDData data = new NDData();

					data.Symbol = Symbol;
					data.Data = dataNeeded;
					data.TimeandDate = titleLink.Time;
					data.Title = titleLink.Title;

					toBeReturned.Add(data);

				}
			}
			
			return toBeReturned;
		}

	
		public static void consoleDisplay(List<NDData> fullData) {
			foreach(NDData item in fullData) {
				
				Console.WriteLine("Title: " + item.Title);
				Console.WriteLine("Date and Time: " + item.TimeandDate);
				Console.WriteLine("Symbol: " + item.Symbol);
				Console.WriteLine("Data: " + item.Data + "\n\n");

			}
		}

		public static void removeNodes(HtmlNode parent, String xpath) {
			
			HtmlNode test = parent.SelectSingleNode(xpath);

			if (test != null) {
				foreach(HtmlNode toBeRemoved in parent.SelectNodes("a")) 
					toBeRemoved.ParentNode.RemoveChild(toBeRemoved, true);
			}

		}

	}
}

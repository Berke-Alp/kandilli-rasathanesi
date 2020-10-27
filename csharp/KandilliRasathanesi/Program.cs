using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace KandilliRasathanesi
{
	class Program
	{
		private static string apiroot = "http://sc3.koeri.boun.edu.tr/eqevents/";
		private static List<Event> events = new List<Event>();
		private static bool last = false;
		private static int lastcount = 0;

		static void Main(string[] args)
		{
			events.Clear();
			if (args.Length == 0) return;
			else if (args.Length == 1)
			{
				if (args[0] == "all") FetchAll(); // Tüm depremleri getirir
				else if (args[0] == "last") // Son depremi getir
				{
					last = true;
					lastcount = 1;
					FetchAll();
				}
			}
			else if (args.Length == 2)
			{
				if (args[0] == "page" && int.TryParse(args[1], out int page)) FetchEvents(page - 1); // Bir sayfadaki depremleri getirir
				else if (args[0] == "last" && int.TryParse(args[1], out int lastc)) // Son x depremi getir
				{
					if (lastc < 1) return;
					last = true;
					lastcount = lastc;
					FetchAll();
				}
			}
		}

		static void FetchAll()
		{
			int page = 0;
			string resp = FetchPage(page);
			while (resp != null)
			{
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(resp);

				var nodes = doc.DocumentNode.SelectNodes("//table[@class='index']/tr[position()>1]");
				for (int i = 0; i < nodes.Count; i++)
				{
					var inside = nodes[i].SelectNodes(".//td");
					string onclick = nodes[i].Attributes["onclick"].Value;
					string subone = onclick.Substring(onclick.IndexOf("/") + 1);
					string id = subone.Substring(0, subone.IndexOf("/"));

					Event ev = new Event
					{
						ID = id,
						MapImage = "http://sc3.koeri.boun.edu.tr/eqevents/event/" + id + "/" + id + "-map.jpeg",
						Time = inside[0].InnerText,
						Magnitude = float.Parse(inside[1].InnerText),
						MagnitudeType = inside[2].InnerText,
						Latitude = inside[3].InnerText.Replace("&deg;", "°"),
						Longitude = inside[4].InnerText.Replace("&deg;", "°"),
						Depth = float.TryParse(inside[5].InnerText, out float res) ? res : 0,
						Region = inside[6].InnerText,
						AM = inside[7].InnerText,
						LastUpdate = inside[8].InnerText
					};

					events.Add(ev);
					if (last && events.Count == lastcount) break;
				}
				if (last && events.Count == lastcount) break;
				page++;
				resp = FetchPage(page);
			}

			File.WriteAllText("Events.json", JsonConvert.SerializeObject(events));
		}

		static void FetchEvents(int page)
		{
			string resp = FetchPage(page);

			if (resp!=null)
			{
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(resp);

				var nodes = doc.DocumentNode.SelectNodes("//table[@class='index']/tr[position()>1]");
				for (int i = 0; i < nodes.Count; i++)
				{
					var inside = nodes[i].SelectNodes(".//td");
					string onclick = nodes[i].Attributes["onclick"].Value;
					string subone = onclick.Substring(onclick.IndexOf("/") + 1);
					string id = subone.Substring(0, subone.IndexOf("/"));

					Event ev = new Event
					{
						ID = id,
						MapImage = "http://sc3.koeri.boun.edu.tr/eqevents/event/" + id + "/" + id + "-map.jpeg",
						Time = inside[0].InnerText,
						Magnitude = float.Parse(inside[1].InnerText),
						MagnitudeType = inside[2].InnerText,
						Latitude = inside[3].InnerText.Replace("&deg;", "°"),
						Longitude = inside[4].InnerText.Replace("&deg;", "°"),
						Depth = float.TryParse(inside[5].InnerText, out float res) ? res : 0,
						Region = inside[6].InnerText,
						AM = inside[7].InnerText,
						LastUpdate = inside[8].InnerText
					};

					events.Add(ev);
				}

				File.WriteAllText("Events.json", JsonConvert.SerializeObject(events));
				Console.WriteLine($"Sayfa {page + 1} için veriler dosyaya yazıldı.");
			}
		}

		static string FetchPage(int page)
		{
			if (page < 0) return null;
			string url = page == 0 ? apiroot + "events.html" : apiroot + "events" + page + ".html";
			try
			{
				var response = url.GetStringAsync().Result;
				return response;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}

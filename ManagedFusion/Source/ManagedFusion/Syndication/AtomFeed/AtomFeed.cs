using System;
using System.Collections.Generic;
using System.Text;

namespace ManagedFusion.Syndication.AtomFeed
{
	internal class AtomFeed : ISyndication
	{
		#region ISyndication Members

		public DateTime LastModified
		{
			get { return DateTime.Now; }
		}

		public string Serialize()
		{
			StringBuilder sb = new StringBuilder();
			using (SyndicationWriter writer = new SyndicationWriter(sb))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("feed");
				writer.WriteAttributeString("xmlns", "http://www.w3.org/2005/Atom");

				Feed feed = Common.ExecutingModule.Syndication;

				// write main part of feed
				WriteSource(writer, feed);

				// <entry>...
				foreach (Entry entry in feed.Items)
					WriteEntry("entry", writer, entry);
			}
			return sb.ToString();
		}

		private void WriteSource(SyndicationWriter writer, Source source)
		{
			// <title>
			WriteText("title", writer, source.Title);

			// <subtitle>
			if (source.SubTitle != null)
				WriteText("subtitle", writer, source.SubTitle);

			// <updated>
			WriteDateTime("updated", writer, source.Updated);

			// <id>
			WriteId("id", writer, source.Id);

			// <link>...
			foreach (Link link in source.Links)
				WriteLink("link", writer, link);

			// <rights>
			if (source.Rights != null)
				WriteText("rights", writer, source.Rights);

			// <generator>
			if (source.Generator != null)
				WriteGenerator("generator", writer, source.Generator);

			// <author>...
			foreach (Person person in source.Authors)
				WritePerson("author", writer, person);

			// <contributor>...
			foreach (Person person in source.Contributors)
				WritePerson("contributor", writer, person);

			// <category>...
			foreach (Category category in source.Categories)
				WriteCategory("category", writer, category);

			// <logo>
			if (source.Logo != null)
				WriteUri("logo", writer, source.Logo);

			// <icon>
			if (source.Icon != null)
				WriteUri("icon", writer, source.Icon);
		}

		private void WriteEntry(string name, SyndicationWriter writer, Entry entry)
		{
			writer.WriteStartElement("entry");

			// <title>
			WriteText("title", writer, entry.Title);

			// <updated>
			WriteDateTime("updated", writer, entry.Updated);

			// <published>
			if (entry.Published.HasValue)
				WriteDateTime("published", writer, entry.Published.Value);

			// <id>
			WriteId("id", writer, entry.Id);

			// <summary>
			if (entry.Summary != null)
				WriteText("summary", writer, entry.Summary);

			// <link>...
			foreach (Link link in entry.Links)
				WriteLink("link", writer, link);

			// <rights>
			if (entry.Rights != null)
				WriteText("rights", writer, entry.Rights);

			// <author>...
			foreach (Person person in entry.Authors)
				WritePerson("author", writer, person);

			// <contributor>...
			foreach (Person person in entry.Contributors)
				WritePerson("contributor", writer, person);

			// <category>...
			foreach (Category category in entry.Categories)
				WriteCategory("category", writer, category);

			// <content>
			if (entry.Content != null)
				WriteContent("content", writer, entry.Content);

			// <source>
			if (entry.Source != null)
			{
				writer.WriteStartElement("source");
				WriteSource(writer, entry.Source);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private void WriteText(string name, SyndicationWriter writer, Text text)
		{
			writer.WriteStartElement(name);
			writer.WriteAttributeString("type", text.Type);
			writer.WriteString(text.InnerText);
			writer.WriteEndElement();
		}

		private void WriteContent(string name, SyndicationWriter writer, Content content)
		{
			writer.WriteStartElement(name);
			writer.WriteAttributeString("type", content.Type);

			// checks which to write to the output of the content
			// you can either write a source URL or text, but not both
			if (content.SourceUrl != null)
			{
				writer.WriteStartAttribute("src");
				writer.WriteValue(content.SourceUrl);
				writer.WriteEndAttribute();
			}
			else
			{
				writer.WriteString(content.InnerText);
			}

			writer.WriteEndElement();
		}

		private void WritePerson(string name, SyndicationWriter writer, Person person)
		{
			writer.WriteStartElement(name);
			writer.WriteStartElement("name");
			writer.WriteString(person.Name);
			writer.WriteEndElement();
			writer.WriteStartElement("uri");
			writer.WriteValue(person.Link);
			writer.WriteEndElement();
			writer.WriteStartElement("email");
			writer.WriteString(person.Email);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		private void WriteLink(string name, SyndicationWriter writer, Link link)
		{
			writer.WriteStartElement(name);
			writer.WriteStartAttribute("href");
			writer.WriteValue(link.Href);
			writer.WriteEndAttribute();

			if (link.Relationship != LinkRelationship.NotDefined)
				writer.WriteAttributeString("rel", link.Relationship.ToString().ToLower());

			if (link.Type != null)
				writer.WriteAttributeString("type", link.Type);

			if (link.Language != null)
				writer.WriteAttributeString("hreflang", link.Language);

			if (link.Title != null)
				writer.WriteAttributeString("title", link.Title);

			if (link.Length.HasValue)
				writer.WriteAttributeString("length", link.Length.Value.ToString());

			writer.WriteEndElement();
		}

		private void WriteCategory(string name, SyndicationWriter writer, Category category)
		{
			writer.WriteStartElement(name);
			writer.WriteAttributeString("term", category.Term);

			if (category.Label != null)
				writer.WriteAttributeString("label", category.Label);

			if (category.SchemeUrl != null)
			{
				writer.WriteStartAttribute("scheme");
				writer.WriteValue(category.SchemeUrl);
				writer.WriteEndAttribute();
			}

			writer.WriteEndElement();
		}

		private void WriteGenerator(string name, SyndicationWriter writer, Generator generator)
		{
			writer.WriteStartElement(name);
			writer.WriteStartAttribute("uri");
			writer.WriteValue(generator.Url);
			writer.WriteEndAttribute();
			writer.WriteStartAttribute("version");
			writer.WriteString(generator.Version.ToString());
			writer.WriteEndAttribute();
			writer.WriteString(generator.InnerText);
			writer.WriteEndElement();
		}

		private void WriteDateTime(string name, SyndicationWriter writer, DateTime date)
		{
			writer.WriteStartElement(name);
			writer.WriteValue(date);
			writer.WriteEndElement();
		}

		private void WriteUri(string name, SyndicationWriter writer, Uri uri)
		{
			writer.WriteStartElement(name);
			writer.WriteValue(uri);
			writer.WriteEndElement();
		}

		private void WriteId(string name, SyndicationWriter writer, string id)
		{
			writer.WriteStartElement(name);
			writer.WriteValue(id);
			writer.WriteEndElement();
		}

		#endregion
	}
}

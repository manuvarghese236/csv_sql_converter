using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.VisualBasic.FileIO;
class Program
{
	static void Main(string[] args)
	{
		// Check if at least one command-line argument is provided
		String tableName = "srv_document_data";
		String csvFileName = "";
		if (args.Length < 1)
		{
			Console.WriteLine("Please provide a string argument.");
			return;
		}
		else { csvFileName = args[0]; }
		if (args.Length > 1){
			tableName = args[1];
		}
		int NoOfLines = 0;
		string currentDirectory = Environment.CurrentDirectory;
		String filePath = currentDirectory + "/" + csvFileName;
		Console.WriteLine("Input File : " + filePath);
		String ouputFile = filePath + ".sql";
		Console.WriteLine("Input File : " + filePath);
		Console.WriteLine("Output File : " + ouputFile);
		// @"C:\Users\ASUS\Desktop\DLink Data upload\dddocument2020-2021.csv";
		String sql = "";
		bool inital = true;
		using (var parser = new TextFieldParser(filePath))
		{
			// Set delimiter (comma for CSV)
			parser.SetDelimiters(",");

			// Ignore whitespace
			parser.TrimWhiteSpace = true;

			// Read and process each row
			String columnlist = "";
			string[] fieldHeader = parser.ReadFields();
			if (fieldHeader != null)
			{
				columnlist = "(`" + String.Join("`,`", fieldHeader) + "`)";
			}
			File.WriteAllText(ouputFile, "");
			Console.WriteLine(columnlist);
			sql = "INSERT INTO `" + tableName + "` " + columnlist + " VALUES " + Environment.NewLine;
			

			while (!parser.EndOfData)
			{
				NoOfLines++;
				string[] fields = parser.ReadFields();
				if (fields != null)
				{
					if (!inital)
					{
						sql +=  ","+Environment.NewLine ;

					}
					else {
						inital = false;
					}
					sql += "('" + String.Join("','", fields) + "')";


				}
				if (NoOfLines % 500 == 0) {
					
					File.AppendAllText(ouputFile, sql);
					sql = "";
				}
				Console.WriteLine("Reading Line " + NoOfLines.ToString("#,##0"));
			}
			sql += ";";

		}
		Console.WriteLine("Completed convert " + NoOfLines.ToString("#,##0") + " Lines"); 
		Console.WriteLine(ouputFile);
		File.AppendAllText(ouputFile, sql);
		Console.WriteLine("Writing to file completed");
	}
}
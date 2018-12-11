using System;
using System.IO;
using System.Collections.Generic;
using NazdaqSearch.Models;
using CsvHelper;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace NazdaqSearch.Logic.CSVConversions 
{

    class NazdaqCSV
    {

        public static void dataToCSV(List<NDData> toBeRecorded) 
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.csv");

            if (toBeRecorded == null || toBeRecorded.Count == 0) 
            { 
                Console.WriteLine("List empty. Stop process");

            } else

            {

                using (StreamWriter writer = new StreamWriter(path))
                {
                    var csv = new CsvWriter(writer);
                    csv.WriteRecords(toBeRecorded);
                }
            }
        }

        public static void convertCSVtoPDF(List<NDData> toBeRecorded)
        {
            Document pdfDoc = new Document();
            string path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/datapdf.pdf");
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));
            pdfDoc.Open();
            pdfDoc.Add(new Paragraph("Nasdaq Data"));
            foreach(NDData item in toBeRecorded)
            {
                pdfDoc.Add(new Paragraph(item.ToString()));
            }
            pdfDoc.Close();
        }

    }

}
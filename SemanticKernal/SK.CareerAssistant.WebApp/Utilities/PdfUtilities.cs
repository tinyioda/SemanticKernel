using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace SK.CareerAssistant.WebApp.Services;

public class PdfUtilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pdfPath"></param>
    /// <returns></returns>
    public static string ExtractTextFromPdf(string pdfPath)
    {
        var text = new StringBuilder();

        using (var document = PdfDocument.Open(pdfPath))
        {
            foreach (var page in document.GetPages())
            {
                var pageText = ContentOrderTextExtractor.GetText(page);
                text.AppendLine(pageText);
            }
        }

        return text.ToString();
    }
}
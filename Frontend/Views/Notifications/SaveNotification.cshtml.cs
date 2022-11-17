using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Testapplication1.Views.Notifications;

public class SaveNotif : PageModel
{
    public static string path = Path.GetFullPath("SavedPDF");
    public static void SavedNotification(Guid id)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //makes the PDF doc
        PdfDocument document = new PdfDocument(); 
        //creates a new page
        PdfPage page = document.AddPage();
        
        XGraphics gfx = XGraphics.FromPdfPage(page);
        //lettertype
        XFont font = new XFont("Arial", 20);
        
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnect>();
        optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=blub;Database=Chengeta");
        using (var context = new DatabaseConnect(optionsBuilder.Options))
        {
            var notifToSave = (from n in context.Notifs
                                            where n.ID == id
                                            select n).ToList();
            string notifId = null;
            foreach (var n in notifToSave)
            {
                gfx.DrawString("Date and Time:", font, XBrushes.Black, new XPoint(200, 50));
                gfx.DrawString((n.Time).ToString(), font, XBrushes.Black, new XPoint(200, 100));
                
                gfx.DrawString("Soundtype:", font, XBrushes.Black, new XPoint(200, 150));
                gfx.DrawString((n.Sound_Type), font, XBrushes.Black, new XPoint(200, 200));
                
                gfx.DrawString("Probability", font, XBrushes.Black,new XPoint(200,250));
                gfx.DrawString((n.Probability).ToString(), font, XBrushes.Black,new XPoint(200,300));
                
                gfx.DrawString("Longitude:", font, XBrushes.Black,new XPoint(200,350));
                gfx.DrawString((n.Longitude).ToString(), font, XBrushes.Black,new XPoint(200,400));
                
                gfx.DrawString("Latitude:", font, XBrushes.Black,new XPoint(200,450));
                gfx.DrawString((n.Latitude).ToString(), font, XBrushes.Black,new XPoint(200,500));

                notifId = n.ID.ToString();
            }
            document.Save($@"{path}\{notifId}.pdf");
        }        
    }
}
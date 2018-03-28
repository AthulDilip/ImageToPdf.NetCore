# ImageToPdf.NetCore
ImageToPdf.NetCore provides a simple way of <b>Converting Image to PDF for Asp.NetCore</b>

## Usage
```csharp
public ActionResult GetPdf()
{
    var pdfBytes = ImageToPdfConvertor.Convert(@"C:\sample.jpg");
    return File(pdfBytes, "application/pdf");
}
```

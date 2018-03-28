using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ImageToPdf.NetCore.Demo.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values/5
        [HttpGet]
        public IActionResult Get()
        {
            byte[] pdfBytes = ImageToPdfConvertor.Convert(@"C:\Users\athul\source\repos\ImageToPdf.NetCore\ImageToPdf.NetCore.Demo\Images\sample1.jpg");
            return File(pdfBytes, "application/pdf");
        }
    }
}

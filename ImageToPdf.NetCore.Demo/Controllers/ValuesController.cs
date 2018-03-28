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
            var pdfBytes = ImageToPdfConvertor.Convert(Environment.CurrentDirectory + @"\Images\sample.jpg");
            return File(pdfBytes, "application/pdf");
        }
    }
}

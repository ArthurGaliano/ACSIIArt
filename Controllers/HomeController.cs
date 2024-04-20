using ACSIIArt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ACSIIArt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static string[] _asciiArt = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //UploadImage
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {

            if (imageFile != null && imageFile.Length >0)
            {
                //byte[] imageData;
                //using (var stream = imageFile.OpenReadStream())
                //{
               
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        await stream.CopyToAsync(memoryStream);
                //        imageData = memoryStream.ToArray();
                //    }
                //}   
                                
                var acciiContent = GenerateASCII(new Bitmap(imageFile.OpenReadStream()));

                // Convertir el arte ASCII en un arreglo de bytes
                byte[] asciiBytes = Encoding.UTF8.GetBytes(acciiContent);

                // Devolver el arreglo de bytes como una descarga al usuario
                return File(asciiBytes, "text/plain", $"{imageFile.FileName}ascii_art.txt");

            }
            else
            {
                // Manejar el caso en el que no se seleccione ninguna imagen
                return RedirectToAction("ImageUploadFailed");

            }

        }

        public string GenerateASCII(Bitmap image)
        {
            StringBuilder sb = new StringBuilder();

            for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
                {
                    Color pixelColor = image.GetPixel(w, h);
                    int gray = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    int index = gray * (_asciiArt.Length - 1) / 255; // Escalar el valor de gris al rango de índices de _asciiArt
                    sb.Append(_asciiArt[index]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }


        public IActionResult ImageUploadedSuccessfully()
        {
            return View();
        }

        public IActionResult ImageUploadFailed()
        {
            return View();
        }

    }
}
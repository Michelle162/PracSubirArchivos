using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PracSubirArchivos.Models;
using System.Diagnostics;

namespace PracSubirArchivos.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

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

		[HttpPost]

		public async Task<AcceptedResult>SubirArchivo(IFormFile archivo) 
		{
            Stream archivoASubir = archivo.OpenReadStream();

            string email = "roxana.valencia@catolica.edu.sv";
            string clave = "123456";
            string ruta = "practica-firebase-268ac.appspot.com";
            string api_key = "AIzaSyAemw6Ej542pUJBguvDcoBx0RXz2WgnzwA";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                new FirebaseStorageOptions
                                                {
                                                    AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                                    ThrowOnCancel = true
                                                }).Child("Archivos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);
            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("Index");

        }

	}
}

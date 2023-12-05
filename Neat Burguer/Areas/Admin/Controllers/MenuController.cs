using Microsoft.AspNetCore.Mvc;
using Neat_Burguer.Areas.Admin.Models.ViewModels;
using Neat_Burguer.Models.Entities;
using Neat_Burguer.Models.ViewModels;
using Neat_Burguer.Repositories;

namespace Neat_Burguer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        public MenuController(ClasificacionRepository clasificacionRepository, MenuRepository menuRepository) 
        {
            ClasificacionRepository = clasificacionRepository;
            MenuRepository = menuRepository;
        }

        public ClasificacionRepository ClasificacionRepository { get; }
        public MenuRepository MenuRepository { get; }


        [HttpGet]
        [HttpPost]
        public IActionResult Index(AdminMenuViewModel vm)
        {
            vm.ListaClasicacion = ClasificacionRepository.GetAll().OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Agregar(AdminAgregarViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.menu.Nombre))
            {
                ModelState.AddModelError("", "Escribir el Nombre de la Hamburguesa");
            }
            if(vm.menu.Precio == null || vm.menu.Precio <= 0)
            {
                ModelState.AddModelError("", "Escriba el precio que corresponde");
            }
            if (string.IsNullOrWhiteSpace(vm.menu.Descripción))
            {
                ModelState.AddModelError("", "Escribe la descripcion");
            }
            if(vm.Archivo != null)
            {
                if(vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo imagenes png");
                }
                if(vm.Archivo.Length > 500 * 1024)
                {
                    ModelState.AddModelError("", "Solamente estan permitidos archivos no mayores a 500kb");
                }
            }
            if (ModelState.IsValid)
            {
                MenuRepository.Insert(vm.menu);
                if(vm.Archivo == null)
                {
                    System.IO.File.Copy("wwwroot/images/burguer.png", $"wwwroot/hamburguesas/{vm.menu.Id}.png");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.menu.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.clasificaciones = ClasificacionRepository
                .GetAll()
                .OrderBy(x => x.Nombre);
                return View(vm);
        }
        public IActionResult Editar(int id)
        {
            var hambur = MenuRepository.GetById(id);
            if(hambur == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarViewModel vm = new();
                vm.menu = hambur;
                vm.clasificaciones = ClasificacionRepository
                    .GetAll()
                    .OrderBy(x => x.Nombre);
                return View(vm);
            }
           
        }
        [HttpPost]
        public IActionResult Editar(AdminAgregarViewModel vm)
        {
            ModelState.Clear();
            if(vm.Archivo != null)
            {
                if(vm.Archivo.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "SOLAMENTE SE PERMITEN IMAGENES JPG");
                }
                if(vm.Archivo.Length > 500 * 1024)
                {
                    ModelState.AddModelError("", "SOLAMENTE SE PERMITEN ARCHIVOS NO MAYORES A 500KB");
                }
            }
            if (ModelState.IsValid)
            {
                var element = MenuRepository.GetById(vm.menu.Id);
                if(element == null)
                {
                    return RedirectToAction("Index");
                }
                element.Nombre = vm.menu.Nombre;
                element.Precio = vm.menu.Precio;
                element.Descripción = vm.menu.Descripción;
                element.IdClasificacion = vm.menu.IdClasificacion;

                MenuRepository.Update(element);
                if(vm.Archivo != null)
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.menu.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }
            vm.clasificaciones = ClasificacionRepository.GetAll().OrderBy(x => x.Nombre);
            return View(vm);
        }
        public IActionResult Eliminar(int id)
        {
            var hambur = MenuRepository.GetById(id);
            if(hambur == null)
            {
                RedirectToAction("Index");
            }
            return View(hambur);
        }

        [HttpPost]
        public IActionResult Eliminar(Menu menu)
        {
            var hambue = MenuRepository.GetById(menu.Id);
            if(hambue == null)
            {
                return RedirectToAction("Index");
            }
            MenuRepository.Delete(hambue);
            var imagen = $"wwwroor/hamburguesas/{menu.Id}.jpg";
            if (System.IO.File.Exists(imagen))
            {
                System.IO.File.Delete(imagen);
            }
            return RedirectToAction("Index");
        }
        public IActionResult AgregarPromocion(int id)
        {
            var hambur = MenuRepository.GetById(id);

            if (hambur == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionViewModel vm = new();

                vm.Id = hambur.Id;
                vm.Nombre = hambur.Nombre;
                vm.Precio = (decimal?)(hambur.Precio ?? 0);
                vm.PrecioPromocion = (decimal?)(hambur.PrecioPromocion ?? 0);

                return View(vm);
            }
        }
        [HttpPost]
        public IActionResult AgregarPromocion(AdminPromocionViewModel vm)
        {
            if (vm.PrecioPromocion < 0)
            {
                ModelState.AddModelError("", "Ingrese el precio que se solicita de la promo");
            }
            if (vm.PrecioPromocion > vm.Precio)
            {
                ModelState.AddModelError("", "El precio debe ser menor al que ya esta ahi");
            }

            if (ModelState.IsValid)
            {
                var burguer = MenuRepository.GetById(vm.Id);
                if (burguer == null)
                {
                    return RedirectToAction("Index");
                }
                burguer.PrecioPromocion = (double?)vm.PrecioPromocion;

                MenuRepository.Update(burguer);

                return RedirectToAction("Index");

            }
            return View(vm);
        }
        public IActionResult EliminarPromocion(int id)
        {
            var burguer = MenuRepository.GetById(id);
            if (burguer == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionViewModel vm = new();
                vm.Id = burguer.Id;
                vm.Nombre = burguer.Nombre;
                vm.Precio = (decimal?)burguer.Precio;
                vm.PrecioPromocion = (decimal?)burguer.PrecioPromocion;

                return View(vm);
            }
        }
        [HttpPost]
        public IActionResult EliminarPromocion(AdminPromocionViewModel vm)
        {
            if (vm != null && (vm.PrecioPromocion != null || vm.PrecioPromocion != 0))
            {
                var hamburguesa = MenuRepository.GetById(vm.Id);
                if (hamburguesa == null)
                {
                    return RedirectToAction("Index");
                }
                hamburguesa.PrecioPromocion = null;
                vm.PrecioPromocion = (decimal?)hamburguesa.PrecioPromocion;

                MenuRepository.Update(hamburguesa);

                return RedirectToAction("Index");
            }
            return View(vm);
        }
    }
}


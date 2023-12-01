using Microsoft.AspNetCore.Mvc;
using Neat_Burguer.Areas.Admin.Models.ViewModels;
using Neat_Burguer.Models.Entities;
using Neat_Burguer.Repositories;

namespace Neat_Burguer.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly MenuRepository menuRepository;
        private readonly ClasificacionRepository clasificacionRepository;

        public HomeController(MenuRepository menuRepository, ClasificacionRepository clasificacionRepository)
        {
            this.menuRepository = menuRepository;
            this.clasificacionRepository = clasificacionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("admin/menu")]
        public IActionResult Menu()
        {
            var viewModel = new MenuViewModel()
            {
                Clasificaciones = clasificacionRepository.GetAll().Select(x => new Models.ClasificacionModel()
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Menus = x.Menu.Select(x=> new Models.MenuModel()
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Descripcion = x.Descripción,
                        PrecioOriginal=(decimal)x.Precio,
                        PrecioNuevo = (decimal?)x.PrecioPromocion
                    })
                })
            };

            return View(viewModel);
        }
        public IActionResult AgregarMenu()
        {
            var viewModel = new AgregarMenuViewModel()
            {
                Clasificaciones = clasificacionRepository.
                GetAll()
                .Select(x => new Models.ClasificacionModel()
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                })
            };
            return View(viewModel);
        }
        [HttpPost]
        [Route("admin/menu/agregar")]
        public IActionResult AgregarMenu(AgregarMenuViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Nombre)) ModelState.AddModelError(string.Empty, "Debes de agregar el nombre");

            if (string.IsNullOrEmpty(vm.Descripcion)) ModelState.AddModelError(string.Empty, "Se requiere la descripcion");

            if (vm.Precio == 0) ModelState.AddModelError(string.Empty, "El precio no puede ser $0");

            if (vm.Archivo?.Length > 500 * 1024) ModelState.AddModelError(string.Empty, "La imagen debe pesar menos de 500kb");

            if  (!ModelState.IsValid)  return View(vm);

            var entity = new Menu()
            {
                Nombre = vm.Nombre,
                Descripción = vm.Descripcion,
                Precio = (double)vm.Precio,
                PrecioPromocion = null,
                IdClasificacion = vm.IdClasificacion
            };

          menuRepository.Insert(entity); // preg
            if(vm.Archivo is null)
            {
                System.IO.File.Copy("wwwroot/images/burguer.png", $"wwwroot/hamburguesas/{entity.Id}.png");

            }
            else
            {
                var fs = new FileStream($"wwwroot/hamburguesas/{entity.Id}.png", FileMode.Create);
                vm.Archivo.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Menu");
            
        }
        public IActionResult EditarMenu(string Id)
        {
            Id = Id.Replace("-", " ");

            var entity = menuRepository.GetByName(Id);

            if (entity is null) return RedirectToAction("Menu");

            var viewModel = new EditarMenuViewModel
            {
                IdMenu = entity.Id,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripción,
                Precio = (decimal)entity.Precio,
                IdClasificacion = entity.IdClasificacion,
                Clasificaciones = clasificacionRepository
                .GetAll()
                .Select(x => new Models.ClasificacionModel()
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                })
            };
            return View(viewModel);
        }
        [HttpPost]
        [Route("admin/menu/editar")]
        public IActionResult EditarMenu(EditarMenuViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Nombre)) ModelState.AddModelError(string.Empty, "Se requiere el nombre");

            if (string.IsNullOrEmpty(vm.Descripcion)) ModelState.AddModelError(string.Empty, "Se requiere la descripcion");

            if (vm.Precio == 0) ModelState.AddModelError(string.Empty, "El precio no puede ser $0");

            if (vm.Archivo?.Length > 500 * 1024) ModelState.AddModelError(string.Empty, "La imagen no debe pesar mas de 500kb");

            if (!ModelState.IsValid) return View(vm);

            var entity = menuRepository.Get(vm.IdMenu);
            if (entity == null) return RedirectToAction("Menu");

            entity.Nombre = vm.Nombre;
            entity.Descripción = vm.Descripcion;
            entity.Precio = (double)vm.Precio;
            entity.IdClasificacion = vm.IdClasificacion;

            menuRepository.Update(entity);

            if (vm.Archivo is not null)
            {
                var fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.IdMenu}.png");
                vm.Archivo.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("Menu");
        }
        [Route("admi/menu/eliminar/{Id}")]
        public IActionResult EliminarMenu(string Id)
        {
            Id = Id.Replace("-", " ");

            var entity = menuRepository.GetByName(Id);

            if (entity is null) return RedirectToAction("Menu");

            var vm = new EliminarMenuViewModel()
            {
                IdMenu = entity.Id,
                Nombre = entity.Nombre
            };
            return View(vm);
        }
        [HttpPost]
        [Route("admin/menu/eliminar")]

        public IActionResult EliminarMenu(EliminarMenuViewModel vm)
        {
            var entity = menuRepository.Get(vm.IdMenu);

            if (entity is null) return RedirectToAction("Menu");

            menuRepository.Delete(entity);

            return RedirectToAction("Menu");
        }
        [Route("admin/promocion/quitar/{Id}")]
        public IActionResult QuitarPromocion(string Id)
        {
            Id = Id.Replace("-", " ");

            var entity = menuRepository.GetByName(Id);

            if (entity is null) return RedirectToAction("Menu");

            var vm = new QuitarPromocionViewModel()
            {
                IdMenu = entity.Id,
                Nombre = entity.Nombre,
                PrecioOriginal = (decimal)entity.Precio,
                PrecioNuevo = (decimal)entity.PrecioPromocion!
            };

            return View(vm);
        }
        [HttpPost]
        [Route("admin/promocion/quuitar")]
        public IActionResult QuitarPromocion(QuitarPromocionViewModel vm)
        {
            var entity = menuRepository.Get(vm.IdMenu);

            if (entity is null) return RedirectToAction("Menu");

            entity.PrecioPromocion = null;

            menuRepository.Update(entity);
            
            return RedirectToAction("Menu");
        }
        [Route("admin/promocion/agregar/{Id}")]
        public IActionResult AgregarPromocion(string Id)
        {
            Id = Id.Replace("-", " ");

            var entity = menuRepository.GetByName(Id);

            if (entity is null) return RedirectToAction("Menu");

            var vm = new AgregarPromocionViewModel()
            {
                IdMenu = entity.Id,
                Nombre = entity.Nombre,
                PrecioOriginal = (decimal)entity.Precio,
                PrecioNuevo = (decimal)entity.Precio
            };
            return View(vm);
        }
        [HttpPost]
        [Route("admin/promocion/agregar")]
        public IActionResult AgregarPromocion(AgregarPromocionViewModel vm)
        {
            if (vm.PrecioNuevo == 0) ModelState.AddModelError(string.Empty, "el precio no puede ser $0");

            if (vm.PrecioNuevo >= vm.PrecioOriginal) ModelState.AddModelError(string.Empty, "El nuevo precio promocional  debe ser menor al original");

            if (!ModelState.IsValid) return View(vm);

            var entity = menuRepository.Get(vm.IdMenu);

            if (entity is null) return RedirectToAction("Menu");

            if (entity is null) return RedirectToAction("Menu");

            entity.PrecioPromocion = (double)vm.PrecioNuevo;

            menuRepository.Update(entity);

            return RedirectToAction("Menu");
        }
    }
}

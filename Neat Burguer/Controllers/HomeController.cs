using Microsoft.AspNetCore.Mvc;
using Neat_Burguer.Areas.Admin.Models.ViewModels;
using Neat_Burguer.Models.ViewModels;
using Neat_Burguer.Repositories;


namespace Neat_Burguer.Controllers
{
    public class HomeController : Controller
    {
        public  ClasificacionRepository clasificacionRepository;
        public  MenuRepository menurepos;

        public HomeController(ClasificacionRepository clasificacionRepository, MenuRepository menurepos)
        {
            this.clasificacionRepository = clasificacionRepository;
            this.menurepos = menurepos;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Home/Promociones/{id}")]
        public IActionResult Promociones(PromocionesViewModel vm, int id)
        {
            vm.INDX = id;
            // se buscan las burgues con promo
            var list = menurepos.GetAll()
                .Where(x => x.PrecioPromocion > 0);
            vm.INDF = list.Count();

            // y luego el object en la database
            var hamburguesa = list.ToList()[vm.INDX]; //
            if(hamburguesa != null)
            {
                vm.Nombre = hamburguesa.Nombre;
                vm.Descripcion = hamburguesa.Descripción;
                vm.Id = hamburguesa.Id;
                vm.Precio = hamburguesa.Precio > 0 ? (decimal)hamburguesa.Precio : 0;
                vm.PrecioPromocion = hamburguesa.PrecioPromocion > 0 ? (decimal)hamburguesa.PrecioPromocion : 0;
            }

            return View(vm);
        }
        [HttpGet]
        [HttpPost]
        public IActionResult Menu(MenuViewModel vm)
        {
            vm.ListaClasificacion = clasificacionRepository.GetAll()
                .Select(x => new ClasificaciónModel
                {
                    Nombre = x.Nombre,
                    ListaMenu = x.Menu.OrderBy(x => x.Nombre)
                    .Select(x => new MenuModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Precio = x.Precio ?? 0,
                        Seleccionado = vm.Nombre == x.Nombre
                    })
                });
            return View(vm);
        }

        [HttpGet("Home/Menu/{id}")]
        public IActionResult Menu(MenuViewModel vm, string id)
        {
            id = id.Replace("-", " ");
            id ??= "";
            var selected = menurepos.GetByNombre(id);
            if(selected == null)
            {
                return RedirectToAction("Menu");
            }
            vm.ListaClasificacion = clasificacionRepository.GetAll()
                .Select(x => new ClasificaciónModel
                {
                    Nombre = x.Nombre,
                    ListaMenu = x.Menu.OrderBy(x => x.Nombre)
                    .Select(x => new MenuModel
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Precio = x.Precio ?? 0,
                        Seleccionado = vm.Nombre == x.Nombre
                    })
                });

            vm.Hamburguesa = new HamburguesaModel()
            {

                Id = selected != null ? selected.Id : 0,
                Descripción = selected != null ? selected.Descripción ?? "" : ""
            };
            return View(vm);
        }

    }
}

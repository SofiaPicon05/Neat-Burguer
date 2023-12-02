using Microsoft.AspNetCore.Mvc;
using Neat_Burguer.Models.ViewModels;
using Neat_Burguer.Repositories;
using NuGet.Protocol.Core.Types;

namespace Neat_Burguer.Controllers
{
    public class HomeController : Controller
    {
        private readonly MenuRepository repository;

        public HomeController(MenuRepository repository)
        {
            this.repository = repository;
        }


        public IActionResult Index()
        {
            return View();
        }
        [Route("menu")]
        public IActionResult Menu()
        {
            var viewModel = new MenuViewModel()
            {
                Clasificaciones = repository.GetAll()
               .GroupBy(x => x.IdClasificacionNavigation)
               .Select(x => new ClasificacionModel
               {
                   Nombre = x.Key.Nombre,
                   Hamburguesas = x.Select(x => new HamburguesaModel
                   {
                       Id = x.Id,
                       Descripcion = x.Descripción,
                       Nombre = x.Nombre,
                       //precio

                   }).ToList()
               }).ToList()
            };

            viewModel.Clasificaciones.First()
                .Hamburguesas.First()
                .Seleccionado = true;

            viewModel.Clasificaciones = (IEnumerable<ClasificacionModel>)viewModel.Clasificaciones
                .SelectMany(x => x.Hamburguesas)
                .First(x => x.Seleccionado);
                

            return View(viewModel);
        }
        [Route("menu/{Id}")]
        public IActionResult Menu(string Id)
        {
            Id = Id.Replace('-', ' ');
            var menu = repository.GetByName(Id);

            if (menu == null) return RedirectToAction(nameof(Index));
            var viewModel = new MenuViewModel()
            {
                Clasificaciones = repository.GetAll()
                .GroupBy(x => x.IdClasificacionNavigation)
                .Select(x => new ClasificacionModel
                {
                    Nombre = x.Key.Nombre,
                    Hamburguesas = x.Select(x => new HamburguesaModel
                    {
                        Id = x.Id,
                        Descripcion = x.Descripción,
                        Nombre = x.Nombre,
                        Precio = x.PrecioPromocion is null ? (decimal)x.Precio : (decimal)x.PrecioPromocion,
                        Seleccionado = x.Id == menu.Id
                    })
                })
            };

            viewModel.Clasificaciones = (IEnumerable<ClasificacionModel>)viewModel.Clasificaciones
                .SelectMany(x => x.Hamburguesas)
                .First(x => x.Seleccionado);

            return View(viewModel);
        }
        [Route("promociones")]
        public IActionResult Promociones()
        {
            var data = repository.GetAll()
                .Where(x => x.PrecioPromocion is not null)
                .ToList();
            var promocion = data?.FirstOrDefault();

            if (promocion == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new PromocionesViewModel()
            {
                Id = promocion.Id,
                Nombre = promocion.Nombre,
                Descripcion=promocion.Descripción,
                PrecioOriginal=(decimal)promocion.Precio,
                PrecioNuevo=(decimal)promocion.PrecioPromocion!,
                AnteriorPromocion = data.ElementAtOrDefault(data.IndexOf(promocion)-1)?.Nombre ?? promocion.Nombre,
                SiguientePromocion = data.ElementAtOrDefault(data.IndexOf(promocion)+ 1)?.Nombre ?? promocion.Nombre
            };
            
            return View(viewModel);
        }

        [Route("promociones/{Id}")]
        public IActionResult Promociones(string Id)
        {
            Id = Id.Replace('-', ' ');

            var data = repository.GetAll()
                .Where(x => x.PrecioPromocion is not null).ToList();

            var promocion = data.FirstOrDefault(x => x.Nombre.ToLower() == Id);
            if (promocion == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new PromocionesViewModel()
            {
                Id = promocion.Id,
                Nombre = promocion.Nombre,
                Descripcion = promocion.Descripción,
                PrecioOriginal = (decimal)promocion.Precio,
                PrecioNuevo = (decimal)promocion.PrecioPromocion!,
                AnteriorPromocion = data.ElementAtOrDefault(data.IndexOf(promocion) - 1)?.Nombre ?? promocion.Nombre,
                SiguientePromocion = data.ElementAtOrDefault(data.IndexOf(promocion) + 1)?.Nombre ?? promocion.Nombre
            };

            return View(viewModel);
        }


    }
}

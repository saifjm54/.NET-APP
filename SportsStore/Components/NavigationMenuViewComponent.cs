using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        // dependency injection , allow the view component to access data without knowing which repository implementation will be used
        public NavigationMenuViewComponent(IStoreRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
        }

    }
}
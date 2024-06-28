using FiorelloApp.ViewModels;

namespace FiorelloApp.Services.Interfaces
{
    public interface IBasketService
    {
        public List<BasketVM> GetBasketList();
        List<BasketVM> GetBasketCookies();
    }
}

using System.Threading.Tasks;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public interface INavigationService
    {
        Task GoToAsync(string route, params object[] navigationData);
    }
}
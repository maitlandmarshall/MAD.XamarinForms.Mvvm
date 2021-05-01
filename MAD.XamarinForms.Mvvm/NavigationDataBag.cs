using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.XamarinForms.Mvvm
{
    internal class NavigationDataBag
    {
        private readonly IDictionary<string, object[]> navigationData = new Dictionary<string, object[]>();

        public NavigationDataBag()
        {

        }

        public string Put(object[] data)
        {
            var id = Guid.NewGuid().ToString();
            this.navigationData.Add(id, data);

            return id;
        }

        public object[] Get(string id)
        {
            if (this.navigationData.TryGetValue(id, out var data))
            {
                // TODO: Cleanup navigation data
                //this.navigationData.Remove(id);
                return data;
            }
            else
            {
                throw new NavigationDataNotFoundException(id);
            }
        }
        
    }
}

using Cnx.CAIMAN.Product.Fns.Core.Models;
using Cnx.CAIMAN.Product.Fns.JSONModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Product.Fns.Helpers
{
    public class MappingModels
    {
        public static Product_DTO MappingRootToProduct(JSONModels.Product item)
        {
            
            Product_DTO outputItem = new Product_DTO();
            outputItem.vcSAP = item.productCode;
            string descripcionProduct = string.Empty;
            var prod = item.productDesc.FirstOrDefault(x => x.languageCode == "ES");
            outputItem.vcNombre900 = prod?.description.Length < 50 ? prod?.description : prod?.description.Substring(0,49);
            outputItem.IdProd55 = item.Characteristic?.FirstOrDefault(x => x.ProductCharacteristic == "MATL_RECLASIF")?.ProductCharacteristicValue;
            outputItem.vcNombre55 = prod?.description.Length < 50 ? prod?.description : prod?.description.Substring(0, 49); ; //Aun hay duda
            outputItem.vc20UsuarioCreacion = "";
            outputItem.vc20UsuarioActualizacion = "";
            outputItem.nPesoVolumetrico = decimal.Parse(item.volume);
            outputItem.dtCreacion = DateTime.Now;
            outputItem.dtActualizacion = DateTime.Now;

            return outputItem;
        }
    }
}

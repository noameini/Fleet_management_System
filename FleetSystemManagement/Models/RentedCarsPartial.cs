using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class RentedCars
    {
        public static RentedCars searchRentedCar(List<RentedCars> arr, string id, string ln)
        {
            foreach (RentedCars val in arr)
                if (val.Costumer.Id == id && val.Veichle.LicenseNumber == ln)
                {
                    return val;
                }
            return null;
        }
    }
}
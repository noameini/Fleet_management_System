using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class PrivateCar: Veichle
    {
        public PrivateCar() { }
        public PrivateCar(string name, string licenseNumber, int model, int price, int seatsNumber, string company) 
            : base(name, licenseNumber, model, price, seatsNumber, company)
        {
        }

    }
}
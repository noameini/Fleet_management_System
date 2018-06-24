using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Truck: Commercial
    {
        public Truck() { }
        public Truck(string name, string licenseNumber, int model, int price, int seatsNumber, string company, bool hook, string truckType) 
            : base(name, licenseNumber, model, price, seatsNumber, company,hook)
        {
            this.truckType = truckType;
        }
        public string TruckType
        {
            get
            { return truckType; }
            set
            { truckType = value; }
        }
    }
}
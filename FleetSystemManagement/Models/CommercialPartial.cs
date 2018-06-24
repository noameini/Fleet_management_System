using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Commercial : Veichle
    {
        public Commercial() { }
        public Commercial(string name, string licenseNumber, int model, int price, int seatsNumber, string company, bool hook) 
            : base(name, licenseNumber, model, price, seatsNumber, company)
        {
            this.hook = hook;
        }
        public bool Hook
        {
            get
            { return hook; }
            set
            { hook = value; }
        }

    }
}
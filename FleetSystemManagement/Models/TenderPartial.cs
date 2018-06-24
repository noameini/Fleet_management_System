using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Tender : Commercial
    {
        public Tender() { }
        public Tender(string name, string licenseNumber, int model, int price, int seatsNumber, string company, bool hook, bool box) 
            : base(name, licenseNumber, model, price, seatsNumber, company,hook)
        {
            this.box = box;
        }
        public bool Box
        {
            get
            { return box; }
            set
            { box = value; }
        }
    }
}
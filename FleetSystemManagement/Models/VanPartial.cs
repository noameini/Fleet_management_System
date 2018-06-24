using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Van: Veichle
    {
        public Van() { }    
        public Van(string name, string licenseNumber, int model, int price, int seatsNumber, string company,int Volume,bool awning) : base(name, licenseNumber, model, price, seatsNumber, company)
        {
            this.awning =awning;
            this.volume = Volume;
        }
        public bool Awning
        {
            get
            { return awning; }
            set
            { awning = value; }
        }
        public int Volume
        {
            get
            { return volume ; }
            set
            { volume = value; }
        }
    }
}
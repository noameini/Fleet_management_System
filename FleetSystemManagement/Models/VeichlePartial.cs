using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Veichle
    {
        public Veichle() { }
        public Veichle(string name, string licenseNumber, int model, int price, int seatsNumber, string company)
        {
            this.name = name;
            this.licenseNumber = licenseNumber;
            this.model = model;
            this.price = price;
            this.seatsNumber = seatsNumber;
            this.company = company;
        }
        public string Name
        {
            get
            { return name; }
            set
            { name = value; }
        }
        public string LicenseNumber
        {
            get
            { return licenseNumber; }
            set
            { licenseNumber = value; }
        }
        public int Model
        {
            get
            { return model; }
            set
            { model = value; }
        }
        public int Price
        {
            get
            { return price; }
            set
            { price = value; }
        }
        public int SeatsNumber
        {
            get
            { return seatsNumber; }
            set
            { seatsNumber = value; }
        }
        public string Company
        {
            get
            { return company; }
            set
            { company = value; }
        }

        internal static Veichle searchVeichles(List<Veichle> veichles, string ln)
        {
            foreach(Veichle v in veichles)
            {
                if(v.LicenseNumber == ln)
                {
                    return v;
                }
            }
            return null;
        }
    }
}
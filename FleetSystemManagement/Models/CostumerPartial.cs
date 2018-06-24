using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FleetSystemManagement.Models
{
    public partial class Costumer
    {

        public Costumer(string ID, string FirstName, string LastName, string CreditName, string Phone, string Mail)
        {
            this.id = ID;
            this.firstName = FirstName;
            this.lastName = LastName;
            this.creditName = CreditName;
            this.phone = Phone;
            this.mail = Mail;
        }
        public string Id
        {
            get
            { return id; }
            set
            { id = value; }
        }
        public string FirstName
        {
            get
            { return firstName; }
            set
            { firstName = value; }
        }
        public string LastName
        {
            get
            { return lastName; }
            set
            { lastName = value; }
        }
        public string CreditName
        {
            get
            { return creditName; }
            set
            { creditName = value; }
        }
        public string Phone
        {
            get
            { return phone; }
            set
            { phone = value; }
        }
        public string Mail
        {
            get
            { return mail; }
            set
            { mail = value; }
        }

        public static Costumer searchCostumer(List<Costumer> arr,string id)
        {
            foreach(Costumer c in arr)
            {
                if (c.Id == id)
                    return c;
            }
            return null;
        }

    }
}
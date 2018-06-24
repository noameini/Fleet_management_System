using FleetSystemManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FleetSystemManagement.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            await initSessions();
            return View();
        }

        private async Task<bool> initSessions()
        {
            using (FleetMSEntities db = new FleetMSEntities())
            {
                //Add costumer list to session
                Costumer[] c = await db.Costumer.ToArrayAsync();
                HashSet<string> hs = new HashSet<string>();
                foreach (Costumer cost in c)
                {
                    hs.Add(cost.Id);
                }
                Session["Costumers"] = hs;
                Session["CostumersArray"] = c.ToList();
                //Add veichle list to session
                Veichle[] v = await db.Veichle.ToArrayAsync();
                hs = new HashSet<string>();
                foreach (Veichle car in v)
                {
                    hs.Add(car.LicenseNumber);
                }
                Session["Veichles"] = hs;
                Session["VeichlesArray"] = v.ToList();

                //Add Rented cars to session

                Dictionary<string, string> d = new Dictionary<string, string>();
                RentedCars[] rc = await db.RentedCars.ToArrayAsync();
                foreach (RentedCars cars in rc)
                {
                    d.Add(cars.Costumer.Id, cars.Veichle.LicenseNumber);
                }
                Session["RentedCars"] = d;

            }
            return true;
        }

        public async Task<JsonResult> AddTruck(Truck data)
        {
            if (ModelState.IsValid)
            {
                if (await AddVeichle(data))
                    return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> AddTender(Tender data)
        {
            if (ModelState.IsValid)
            {
                if (await AddVeichle(data))
                    return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> AddPrivateCar(PrivateCar data)
        {
            if (ModelState.IsValid)
            {
                if (await AddVeichle(data))
                    return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> AddVan(Van data)
        {
            if (ModelState.IsValid)
            {
                if (await AddVeichle(data))
                    return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> RemoveCar(string data)
        {
            if (ModelState.IsValid)
            {
                HashSet<string> hs = (HashSet<string>)Session["Veichles"];
                if (!hs.Contains(data)) // checking if car exist.
                {
                    return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
                }
                Dictionary<string, string> d = (Dictionary<string, string>)Session["RentedCars"];
                if (d.ContainsValue(data))  // check if the car is rented.
                {
                    return await Task.Run(() => Json(2, JsonRequestBehavior.AllowGet));
                }
                using (var db = new FleetMSEntities())
                {
                    try
                    {
                        List<Veichle> list = (List<Veichle>)Session["VeichlesArray"]; //remove car from list
                        Veichle toRemove = Veichle.searchVeichles(list, data);

                        
                        db.Veichle.Attach(toRemove);
                        db.Veichle.Remove(toRemove);
                        await db.SaveChangesAsync();

                        //updating sessions
                        list.Remove(toRemove);
                        Session["VeichlesArray"] = list;
                        hs.Remove(data);
                        Session["Veichles"] = hs;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }
                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> RentCar(string ln, string id)
        {
            if (ModelState.IsValid)
            {
                HashSet<string> hs1 = (HashSet<string>)Session["Veichles"];
                HashSet<string> hs2 = (HashSet<string>)Session["Costumers"];
                Dictionary<string, string> d = (Dictionary<string, string>)Session["RentedCars"];
                if (!hs1.Contains(ln) || !hs2.Contains(id) || (d.ContainsKey(id) && d[id].Equals(ln))) // checking if car and costumer are exist.
                {
                    return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
                }

                using (var db = new FleetMSEntities())
                {
                    try
                    {
                        var rc = new RentedCars();

                        //List<Costumer> costumerList = (List<Costumer>)Session["CostumersArray"];
                        //List<Veichle> veichleList = (List<Veichle>)Session["VeichlesArray"];
                        //rc.Costumer = Costumer.searchCostumer(costumers, id);
                        //rc.Veichle = Veichle.searchVeichles(veichles, ln);

                        Costumer[] costumers = await db.Costumer.ToArrayAsync();
                        Veichle[] veichles = await db.Veichle.ToArrayAsync();
                        rc.Costumer =Costumer.searchCostumer(costumers.ToList(), id); //find costumer
                        rc.Veichle =Veichle.searchVeichles(veichles.ToList(), ln);  //find veichle
                        if(d.ContainsKey(rc.Costumer.Id)){
                            return await Task.Run(() => Json(2, JsonRequestBehavior.AllowGet));
                        }

                        db.RentedCars.Add(rc);
                        await db.SaveChangesAsync();

                        //updating sessions
                        d.Add(rc.Costumer.Id,rc.Veichle.LicenseNumber);
                        Session["RentedCars"] = d;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }
                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> RemoveRentCar(string ln, string id)
        {
            if (ModelState.IsValid)
            {
                Dictionary<string, string> d = (Dictionary<string, string>)Session["RentedCars"];
                if (!(d.ContainsKey(id) && d[id].Equals(ln))) // checking if car or costumer are exist.
                {
                    return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
                }

                using (var db = new FleetMSEntities())
                {
                    try
                    {

                        RentedCars[] rentcar = await db.RentedCars.ToArrayAsync();

                        var rc = RentedCars.searchRentedCar(rentcar.ToList(), id, ln);

                        db.RentedCars.Attach(rc);
                        db.RentedCars.Remove(rc);
                        await db.SaveChangesAsync();

                        //updating sessions
                        d.Remove(id);
                        Session["RentedCars"] = d;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }
                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));

        }

        public async Task<JsonResult> AddCostumer(Costumer data)
        {
            if (ModelState.IsValid)
            {
                HashSet<string> hs = (HashSet<string>)Session["Costumers"];
                if (hs.Contains(data.Id)) // checking if costumer is already exist.
                {
                    return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
                }
                using (var db = new FleetMSEntities())
                {
                    try
                    {
                        db.Costumer.Add(data);
                        await db.SaveChangesAsync();

                        //updating sessions
                        hs.Add(data.Id);
                        Session["Costumers"] = hs;
                        List<Costumer> list = (List<Costumer>)Session["CostumersArray"];
                        list.Add(data);
                        Session["CostumersArray"] = list;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }
                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> RemoveCostumer(Costumer data)
        {
            if (ModelState.IsValid)
            {
                HashSet<string> hs = (HashSet<string>)Session["Costumers"];
                if (!hs.Contains(data.Id)) // checking if costumer is already exist.
                {
                    return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
                }
                using (var db = new FleetMSEntities())
                {
                    try
                    {
                        List<Costumer> list = (List<Costumer>)Session["CostumersArray"]; //remove costumer from list
                        Costumer toRemove = Costumer.searchCostumer(list, data.Id);

                        Dictionary<string, string> d = (Dictionary<string, string>)Session["RentedCars"];
                        var RemoveRentCars = await db.RentedCars.ToArrayAsync();
                        foreach (var item in RemoveRentCars)
                        {
                            if (item.Costumer.Id.Equals(data.Id))  // Check if the costumer rented car.
                            {
                                return await Task.Run(() => Json(2, JsonRequestBehavior.AllowGet));
                            }
                        }
                        
                        db.Costumer.Attach(toRemove);
                        db.Costumer.Remove(toRemove); // Remove costumer.
                        await db.SaveChangesAsync();

                        //updating sessions
                        hs.Remove(data.Id);
                        Session["Costumers"] = hs;
                        list.Remove(toRemove);
                        Session["CostumersArray"] = list;
                        d.Remove(data.Id);
                        Session["RentedCars"] = d;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }
                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

        public async Task<bool> AddVeichle(Veichle v)
        {
            HashSet<string> hs = (HashSet<string>)Session["Veichles"];
            if (hs.Contains(v.LicenseNumber)) // check if the license number already exist.
            {
                return false;
            }
            using (var db = new FleetMSEntities())
            {
                hs.Add(v.LicenseNumber);
                Session["Veichles"] = hs;
                List<Veichle> list = (List<Veichle>)Session["VeichlesArray"];
                list.Add(v);
                Session["VeichlesArray"] = list;
                db.Veichle.Add(v);
                await db.SaveChangesAsync();
            }
            return true;
        }

        public async Task<JsonResult> checkEditLicenseNumber(string data)
        {
            string res = "";
            List<Veichle> arr = (List<Veichle>)Session["VeichlesArray"];
            foreach (var v in arr)
            {
                if (v.LicenseNumber == data)
                {
                    res += v.Name + "=" + v.LicenseNumber + "=" + v.Model + "=" + v.Price + "=" + v.SeatsNumber + "=" + v.Company;
                    if (v is PrivateCar)
                    {
                        Session["EditType"] = "PrivateCar";
                        res = "PrivateCar=" + res;
                    }
                    if (v is Tender)
                    {
                        Session["EditType"] = "Tender";
                        res = "Tender=" + res + "=" + ((Tender)v).Hook + "=" + ((Tender)v).Box;
                    }
                    else if (v is Truck)
                    {
                        Session["EditType"] = "Truck";
                        res = "Truck=" + res + "=" + ((Truck)v).Hook + "=" + ((Truck)v).TruckType;
                    }
                    else if (v is Van)
                    {
                        Session["EditType"] = "Van";
                        res = "Van=" + res + "=" + ((Van)v).Awning + "=" + ((Van)v).Volume;
                    }
                    return await Task.Run(() => Json(res, JsonRequestBehavior.AllowGet));
                }
            }
            return await Task.Run(() => Json(-1, JsonRequestBehavior.AllowGet));
        }

        public async Task<JsonResult> EditCar(string name, string ln, int model, int price, int seatsNumber, string company, string hook, string box, string trucktype, string awning, string volume)
        {
            bool HookParam = false, AwningParam = false, BoxParam = false;
            if (hook.Equals("true"))
            {
                HookParam = true;
            }
            if (awning.Equals("true"))
            {
                AwningParam = true;
            }
            if (box.Equals("true"))
            {
                BoxParam = true;
            }
            if (ModelState.IsValid)
            {
                using (var db = new FleetMSEntities())
                {
                    Veichle car;
                    if (((string)Session["EditType"]).Equals("Truck")) //check if type is truck
                    {
                        car = new Truck(name, ln, model, price, seatsNumber, company, HookParam, trucktype);
                    }
                    else if (((string)Session["EditType"]).Equals("Van"))  //check if type is Van
                    {
                        car = new Van(name, ln, model, price, seatsNumber, company, int.Parse(volume), AwningParam);
                    }
                    else if (((string)Session["EditType"]).Equals("Tender"))  //check if type is Tender
                    {
                        car = new Tender(name, ln, model, price, seatsNumber, company, HookParam, BoxParam);
                    }
                    else
                    {
                        car = new PrivateCar(name, ln, model, price, seatsNumber, company);
                    }
                    try
                    {
                        List<Veichle> v = (List<Veichle>)Session["VeichlesArray"];
                        Veichle toRemove = Veichle.searchVeichles(v, ln);
                        
                        db.Veichle.Attach(toRemove);
                        db.Veichle.Remove(toRemove); //remove from db
                        if (car is Truck)
                        {
                            db.Veichle.Add(((Truck)car));
                        }
                        else if (car is Tender)
                        {
                            db.Veichle.Add(((Tender)car));
                        }
                        else if (car is Van)
                        {
                            db.Veichle.Add(((Van)car));
                        }
                        else if (car is PrivateCar)
                        {
                            db.Veichle.Add(((PrivateCar)car));
                        }
                        await db.SaveChangesAsync();

                        //updating sessions
                        v.Remove(toRemove);
                        v.Add(car);
                        Session["VeichlesArray"] = v;

                        return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Source);
                    }

                }
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }



        public async Task<JsonResult> searchCar(string data)
        {
            HashSet<string> hs = (HashSet<string>)Session["Veichles"];
            if (hs.Contains(data))
            {
                return await Task.Run(() => Json(0, JsonRequestBehavior.AllowGet));
            }
            return await Task.Run(() => Json(1, JsonRequestBehavior.AllowGet));
        }

    }
}
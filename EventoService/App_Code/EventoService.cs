using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for EventoService
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode
    = AspNetCompatibilityRequirementsMode.Allowed)]
[System.Web.Script.Services.ScriptService]
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class EventoService : System.Web.Services.WebService {

    public EventoService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string About()
    {
        return "Created by Dor Amir";
    }

    [WebMethod]
    public string RegisterUser(string firstName, string lastName, string username,string password, string email, string bDate,
        string tags, string alert)
    {
        string[] tagStrings = tags.Split(',');
        DateTime date = Convert.ToDateTime(bDate);
        bool a = false;
        if (alert == "true")
        {
            a = true;
        }
        Model.RegisterUser(firstName, lastName, username,password, email, date,a);
        foreach (string tag in tagStrings)
        {
            Model.RegisterTag(tag);
            Model.UserTag(tag, username);
        }
        return "OK";
    }
    [WebMethod]
    public string RegisterManager(string username, string password, string email)
    {
        bool ans = Model.RegisterManger(username, password, email);
        return ans.ToString();
    }
    [WebMethod]
    public string AdminLogin(string username, string password)
    {
        if (Model.LoginAdmin(username, password))
        {
            return "OK";
        }
        return "NO";
    }
    [WebMethod]
    public string AddEvent(string eventManager,string name, string date, string hour, string time, string artist,
        string description, string price, string numOfTickets,string longitude, string latitude)
    {
        double priceInt = Convert.ToDouble(price);
        DateTime myDate = DateTime.ParseExact(date + " " + hour + ":52,531", "dd-MM-yyyy HH:mm:ss,fff",
            System.Globalization.CultureInfo.InvariantCulture);
        //TODO: add event to DB
        List<string> art = artist.Split(',').ToList();
        Model.RegisterEvent(eventManager, name, myDate, Convert.ToDouble(time), description, Convert.ToDouble(price),
            Convert.ToInt32(numOfTickets), art, Convert.ToDouble(longitude), Convert.ToDouble(latitude));

        List<string> emailsList = Model.Location(Convert.ToDouble(longitude),Convert.ToDouble(latitude));
        foreach (string email in emailsList)
        {
            //TODO: send email to all the users in the same location

        }
        return "OK";
    }
}



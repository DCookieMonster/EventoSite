using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for EventoService
/// </summary>
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
    public string RegisterUser(string firstName, string lastName, string username, string email, string bDate,
        string tags)
    {
        string[] tagStrings = tags.Split(',');
        DateTime date = Convert.ToDateTime(bDate);
        Model.RegisterUser(firstName, lastName, username, email, date);
        foreach (string tag in tagStrings)
        {
            Model.RegisterTag(tag);
            Model.userTag(tag, username);
        }
        return "OK";
    }

    public string AdminLogin(string username, string password)
    {
        if (Model.LoginAdmin(username, password))
        {
            return "OK";
        }
        return "NO";
    }

    public string AddEvent(string name, string date, string hour, string time, string location, string artist,
        string description, string price)
    {
        double priceInt = Convert.ToDouble(price);
        DateTime myDate = DateTime.ParseExact(date + " " + hour + ":52,531", "dd-MM-yyyy HH:mm:ss,fff",
            System.Globalization.CultureInfo.InvariantCulture);
        //TODO: add event to DB

        List<string> emailsList = Model.Location(location);
        foreach (string email in emailsList)
        {
            //TODO: send email to all the users in the same location

        }
        return "OK";
    }
}



﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
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
        DateTime date = DateTime.ParseExact(bDate + " 00:00:52,531", "dd-MM-yyyy HH:mm:ss,fff",
            System.Globalization.CultureInfo.InvariantCulture);
        bool a = alert == "true";
        bool ans=Model.RegisterUser(firstName, lastName, username.ToLower(),password, email, date,a);
        if (!ans)
        {
            return "No";
        }
        foreach (string tag in tagStrings)
        {
            Model.RegisterTag(tag.ToLower());
            Model.UserTag(tag.ToLower(), username.ToLower());
        }
        return "OK";
    }
    [WebMethod]
    public string LoginUser(string username, string password, string lon, string lat)
    {
        return Model.LoginUser(username.ToLower(), password, Convert.ToDouble(lon), Convert.ToDouble(lat)).ToString();
    }
    [WebMethod]
    [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        ResponseFormat = WebMessageFormat.Json)]
    public string RegisterManager(string username, string password, string email)
    {
        bool ans = Model.RegisterManger(username.ToLower(), password, email);
        return ans.ToString();
    }
    [WebMethod]
    public string AdminLogin(string username, string password)
    {
        if (Model.LoginAdmin(username.ToLower(), password))
        {
            return "OK";
        }
        return "NO";
    }
    [WebMethod]
    public string AddEvent(string eventManager,string name, string date, string hour, string time, string artist,
        string description, string price, string numOfTickets,string longitude, string latitude,string tags,string address)
    {
        DateTime myDate = DateTime.ParseExact(date + " " + hour + ":52,531", "dd-MM-yyyy HH:mm:ss,fff",
            System.Globalization.CultureInfo.InvariantCulture);
        List<string> art = artist.Split(',').ToList();
        bool a=Model.RegisterEvent(eventManager.ToLower(), name, myDate, Convert.ToDouble(time), description, Convert.ToDouble(price),
            Convert.ToInt32(numOfTickets), art, Convert.ToDouble(longitude), Convert.ToDouble(latitude),address);
        if (!a)
            return "NO";
        string[] tagsStrings = tags.Split(',');
        foreach (string tag in tagsStrings)
        {
            Model.RegisterTag(tag.ToLower());
            Model.EventTags(tag.ToLower(), name);
        }
        return "OK";
    }

    
    [WebMethod]
    public string GetEventsNearBy(string longitude, string latitude,string dist)
    {
        List<Event> events = Model.Location(Convert.ToDouble(longitude), Convert.ToDouble(latitude),Convert.ToDouble(dist));

        var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        string sJSON = oSerializer.Serialize(events);
        return sJSON;

    }

    [WebMethod]
    public string GetEventsByHashTags(string username)
    {
        List<Event> events=new List<Event>();
        List<string> tags = Model.GetUserTags(username.ToLower());
        foreach (string tag in tags)
        {
            List<Event> eTmp = Model.EventsByTag(tag);
            foreach (Event e in eTmp)
            {
                bool b = true;
                foreach (Event @event in events)
                {
                    if ((e.EventName == @event.EventName && e.description == @event.description && e.date == @event.date))
                    {
                        b = false;
                    }
                }
                if(b) events.Add(e);
            }
        }
        var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        string sJSON = oSerializer.Serialize(events);
        return sJSON;

    }
}



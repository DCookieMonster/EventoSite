using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

/// <summary>
/// Summary description for Event
/// </summary>
public class Event
{
    public string EventName;
    public List<string> Artists;
    public double price;
    public int tickets;
    public DateTime date;
    public string duration;
    public string description;
    public string location;

	public Event()
	{
		//
		// TODO: Add constructor logic here
		//
	}

}
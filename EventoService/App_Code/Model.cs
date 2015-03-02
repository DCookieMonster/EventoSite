using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Web;

/// <summary>
/// Summary description for Model
/// </summary>
public class Model
{
    static string connGDM = ConfigurationManager.ConnectionStrings["ConnectionEvent"].ConnectionString;
    static SqlConnection SqlCon = new SqlConnection(connGDM);
    public Model()
    {

    }
    #region Register

    /// <summary>
    /// register user to the system
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="bDate"></param>
    /// <returns></returns>
    public static bool RegisterUser(string firstName, string lastName, string username,string password, string email, DateTime bDate,bool alert)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_firstName = new SqlParameter("FirstName", firstName);
        SqlParameter sql_password = new SqlParameter("Password", password);
        SqlParameter sql_LastName = new SqlParameter("LastName", lastName);
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_email = new SqlParameter("Email", email);
        SqlParameter sql_bDate = new SqlParameter("bDate", bDate);
        SqlParameter sql_alert =new SqlParameter("Alert",alert);

        string query = "INSERT INTO Users (firstName,lastName,email,hasAllerts,username,password,birthDate) VALUES (@FirstName, @LastName, @Email,@Alert,@Username,@Password,@bDate)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_firstName);
        cmd.Parameters.Add(sql_LastName);
        cmd.Parameters.Add(sql_username);
        cmd.Parameters.Add(sql_email);
        cmd.Parameters.Add(sql_bDate);
        cmd.Parameters.Add(sql_password);
        cmd.Parameters.Add(sql_alert);
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }
    /// <summary>
    /// save tags
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static bool RegisterTag(string tag)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sqlTag = new SqlParameter("Tag", tag);
        string queryTag = "SELECT id FROM HashTags WHERE hashtag=@Tag";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sqlTag);
        int id = -1;
        try
        {
            SqlCon.Open();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (id != -1)
            return ans;
        string query = "INSERT INTO HashTags (hashtag) VALUES (@Tag)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);
        SqlParameter sqlTag1 = new SqlParameter("Tag", tag);

        // Add Params to the query string
        cmd.Parameters.Add(sqlTag1);
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="email"></param>
    /// <returns></returns>
   public static bool RegisterManger(string username,string password,string email)
    {
          bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_firstName = new SqlParameter("Password", password);
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_email = new SqlParameter("Email", email);

        string query = "INSERT INTO EventManagers (username,password,email) VALUES (@Username, @Password, @Email)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_firstName);
        cmd.Parameters.Add(sql_username);
        cmd.Parameters.Add(sql_email);
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artistName"></param>
    /// <returns></returns>
    private static int RegisterArtist(string artistName)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_tag = new SqlParameter("artist", artistName.Trim().ToLower());
        string queryTag = "SELECT id FROM Artists WHERE artistName=@artist";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_tag);
        int id = -1;
        try
        {
            SqlCon.Open();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (id != -1)
            return id;
        SqlParameter sql_tag1 = new SqlParameter("artist", artistName);

        string query = "INSERT INTO Artists (artistName) VALUES (@artist)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_tag1);
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }


        return id;
    }

    /// <summary>
    /// save new event into the DB
    /// </summary>
    /// <param name="eventManager">the username of the event manager</param>
    /// <param name="eventName"></param>
    /// <param name="date"></param>
    /// <param name="duration"></param>
    /// <param name="description" />
    /// <param name="price"></param>
    /// <param name="numOfTickets"></param>
    /// <param name="artistsList"></param>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <returns>true for success and false for error</returns>
    public static bool RegisterEvent(string eventManager, string eventName, DateTime date, double duration, string description,
        double price, int numOfTickets, List<string> artistsList, double longitude,double latitude,string address)
    {
        List<int>artistId=new List<int>();
        foreach (string artist in artistsList)
        {
            artistId.Add(RegisterArtist(artist));
        }
        int managerId=GetEventManagerID(eventManager);
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_managerID = new SqlParameter("ManagerID", managerId);
        SqlParameter sql_eventName = new SqlParameter("EventName", eventName);
        SqlParameter sql_Date = new SqlParameter("Date", date);
        SqlParameter sql_duration = new SqlParameter("duration", duration);
        SqlParameter sql_description = new SqlParameter("description", description);
        SqlParameter sql_price = new SqlParameter("price", price);
        SqlParameter sql_ticket = new SqlParameter("tickets", numOfTickets);
        SqlParameter sql_long = new SqlParameter("longitude",longitude);
        SqlParameter sql_lat = new SqlParameter("latitude", latitude);
        SqlParameter sql_add = new SqlParameter("address", address);

        string query = "INSERT INTO Events (EventName,EventManagerId,date,duration,description,price,maxNumOfTickets,longitude,latitude,address)" +
                       " VALUES (@EventName, @ManagerID, @Date,@duration,@description,@price,@tickets,@longitude,@latitude,@address)";
        string idQ = "SELECT id from Events Where EventName=@EventName AND EventManagerId=@ManagerID";

        SqlCommand cmd = new SqlCommand(query, SqlCon);
        SqlCommand cmd2 = new SqlCommand(idQ, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_Date);
        cmd.Parameters.Add(sql_description);
        cmd.Parameters.Add(sql_duration);
        cmd.Parameters.Add(sql_eventName);
        cmd.Parameters.Add(sql_managerID);
        cmd.Parameters.Add(sql_price);
        cmd.Parameters.Add(sql_ticket);
        cmd.Parameters.Add(sql_lat);
        cmd.Parameters.Add(sql_long);
        cmd.Parameters.Add(sql_add);

        //cmd2
        SqlParameter sql_managerID1 = new SqlParameter("ManagerID", managerId);
        SqlParameter sql_eventName1 = new SqlParameter("EventName", eventName);
        cmd2.Parameters.Add(sql_eventName1);
        cmd2.Parameters.Add(sql_managerID1);
        int id = -1;
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
            id = (int) cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        foreach (int aId in artistId)
        {
            RegisterArtistToEvents(aId, id);

        }
        return ans;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aId"></param>
    /// <param name="eId"></param>
    /// <returns></returns>
    private static bool RegisterArtistToEvents(int aId, int eId)
    {
        string query = "INSERT INTO ArtistsInEvents (eventId,artistId) VALUES (" + eId + "," + aId + ")";
        SqlCommand cmd = new SqlCommand(query, SqlCon);
        bool ans = true;
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }
    #endregion

    #region FK
    public static bool UserTag(string tag, string username)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_tag = new SqlParameter("Tag", tag);
        string queryTag = "SELECT id FROM HashTags WHERE hashtag=@tag";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_tag);

        SqlParameter sql_user = new SqlParameter("User", username);
        string queryUser = "SELECT id FROM Users WHERE username=@User";
        SqlCommand cmd3 = new SqlCommand(queryUser, SqlCon);
        cmd3.Parameters.Add(sql_user);

        int tagId = -1;
        int userId = -1;
        try
        {
            SqlCon.Open();
            tagId = (int)cmd2.ExecuteScalar();
            userId = (int)cmd3.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (userId == -1 || tagId == -1)
            return false;
        string query = "INSERT INTO hashTagsOfUsers (userId,tagId) VALUES (" + userId + "," + tagId + ")";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }
    //TODO: neeed to fix
    public static bool EventTags(string tag, string eventName)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_tag = new SqlParameter("Tag", tag);
        string queryTag = "SELECT id FROM HashTags WHERE hashtag=@tag";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_tag);

        SqlParameter sql_user = new SqlParameter("event", eventName);
        string queryUser = "SELECT id FROM Events WHERE eventName=@event";
        SqlCommand cmd3 = new SqlCommand(queryUser, SqlCon);
        cmd3.Parameters.Add(sql_user);

        int tagId = -1;
        int userId = -1;
        try
        {
            SqlCon.Open();
            tagId = (int)cmd2.ExecuteScalar();
            userId = (int)cmd3.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (userId == -1 || tagId == -1)
            return false;
        string query = "INSERT INTO HastTagsOfEvents VALUES (" + userId + "," + tagId + ")";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return ans;
    }
    #endregion

    #region login
    public static bool LoginAdmin(string username, string password)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_password = new SqlParameter("Password", password);
        string queryTag = "SELECT id FROM EventManagers WHERE username=@Username AND password=@Password";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_username);
        cmd2.Parameters.Add(sql_password);
        int id = -1;
        try
        {
            SqlCon.Open();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (id == -1)
            return false;
        return ans;
    }
    /// <summary>
    /// check if user exisit in the system and update his longitude and latitude
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="longitude"></param>
    /// <param name="latitude"></param>
    /// <returns>true if he is in the system, else- false</returns>
    public static bool LoginUser(string username, string password, double longitude, double latitude)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_password = new SqlParameter("Password", password);
        SqlParameter sql_lon = new SqlParameter("longitude",longitude);
        SqlParameter sql_lat = new SqlParameter("latitude",latitude);
        string queryTag = "SELECT id FROM Users WHERE username=@Username AND password=@Password";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_username);
        cmd2.Parameters.Add(sql_password);
        int id = -1;
        try
        {
            SqlCon.Open();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        if (id == -1)
            return false;
     
         string qUser = "UPDATE Users SET longitude=@longitude, latitude=@latitude  WHERE id=" + id;
        
        SqlCommand cmd = new SqlCommand(qUser, SqlCon);
        cmd.Parameters.Add(sql_lon);
        cmd.Parameters.Add(sql_lat);
        try
        {
            SqlCon.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            ans = false;
        }
        finally
        {
            SqlCon.Close();
        }

        return ans;
    }
    #endregion

    #region Other
    
    public static List<Event> Location(double longitude, double latitude)
    {
        List<Event> columnData = new List<Event>();

        using (SqlConnection connection = new SqlConnection(connGDM))
        {
            string query = "SELECT * FROM Events";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double eventLongitude = (double) reader["longitude"];
                        double eventLatitude = (double) reader["latitude"];
                        if (reader["longitude"]!=null && Euclidean(eventLatitude, eventLongitude, latitude, longitude) < 15)
                        {
                            Event e=new Event
                            {
                                EventName = reader["eventName"].ToString(),
                                date = (DateTime) reader["date"],
                                description = reader["description"].ToString(),
                                duration = reader["duration"].ToString(),
                                location = reader["address"].ToString(),
                                price = Convert.ToDouble(reader["price"].ToString()),
                                tickets = Convert.ToInt32(reader["maxNumOfTickets"].ToString()),
                                Artists = ArtistInEvent(reader["id"].ToString())
                            };
                            columnData.Add(e);
                        }
                    }
                }
                connection.Close();
            }
        }
        return columnData;
    }

    private static List<string> ArtistInEvent(string eID)
    {
        List<string> columnData = new List<string>();

        using (SqlConnection connection = new SqlConnection(connGDM))
        {
            string query = "SELECT * FROM ArtistsInEvents Where eventId="+eID;
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SqlConnection tmpSqlConnection=new SqlConnection(connGDM);
                        string q = "SELECT * FROM Artists Where id=" + reader["artistId"];
                        SqlCommand cmd2 = new SqlCommand(q, tmpSqlConnection);
                        try
                        {
                            tmpSqlConnection.Open();
                            SqlDataReader reader1 = cmd2.ExecuteReader();
                            while (reader1.Read())
                            {
                                columnData.Add(reader1["artistName"].ToString());
                            }
                        }
                        catch
                        {
                            bool ans = false;
                        }
                        finally { SqlCon.Close(); }



                    }
                
                }
                connection.Close();
            }
        }
        return columnData;
    } 

    private static int GetEventManagerID(string eventManager)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_tag = new SqlParameter("username", eventManager);
        string queryTag = "SELECT id FROM EventManagers WHERE username=@username";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_tag);
        int id = -1;
        try
        {
            SqlCon.Open();
            id = (int)cmd2.ExecuteScalar();
        }
        catch
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        return id;
    }


    /// <summary>
    /// Return the distance between 2 points
    /// </summary>
    private static double Euclidean(double x1,double y1,double x2,double y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1-y2, 2));
    }
    #endregion
}
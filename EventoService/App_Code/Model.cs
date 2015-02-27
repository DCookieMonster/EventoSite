using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
    
    /// <summary>
    /// register user to the system
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="LastName"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="bDate"></param>
    /// <returns></returns>
    public static bool RegisterUser(string firstName, string lastName, string username, string email, DateTime bDate)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_firstName = new SqlParameter("FirstName", firstName);
        SqlParameter sql_LastName = new SqlParameter("LastName", lastName);
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_email = new SqlParameter("Email", email);
        SqlParameter sql_bDate = new SqlParameter("bDate", bDate);

        string query = "INSERT INTO users VALUES (@FirstName, @LastName, @Username, @Email, @bDate)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_firstName);
        cmd.Parameters.Add(sql_LastName);
        cmd.Parameters.Add(sql_username);
        cmd.Parameters.Add(sql_email);
        cmd.Parameters.Add(sql_bDate);
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
        SqlParameter sql_tag = new SqlParameter("Tag", tag);
        string queryTag = "SELECT id FROM tags WHERE tag=@tag";
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
            return ans;
        string query = "INSERT INTO tags VALUES (@tag)";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_tag);
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

    public static bool UserTag(string tag, string username)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_tag = new SqlParameter("Tag", tag);
        string queryTag = "SELECT id FROM tags WHERE tag=@tag";
        SqlCommand cmd2 = new SqlCommand(queryTag, SqlCon);
        cmd2.Parameters.Add(sql_tag);

        SqlParameter sql_user = new SqlParameter("User", username);
        string queryUser = "SELECT id FROM users WHERE username=@User";
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
        string query = "INSERT INTO users_tags VALUES (" + tagId + "," + userId + ")";
        SqlCommand cmd = new SqlCommand(query, SqlCon);

        // Add Params to the query string
        cmd.Parameters.Add(sql_tag);
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

    public static bool LoginAdmin(string username, string password)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_password = new SqlParameter("Password", password);
        string queryTag = "SELECT id FROM Musers WHERE username=@Username AND password=@Password";
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

    public static bool LoginUser(string username, string password, double longitude, double latitude)
    {
        bool ans = true;
        // Map SQL param names to C# param names
        SqlParameter sql_username = new SqlParameter("Username", username);
        SqlParameter sql_password = new SqlParameter("Password", password);
        SqlParameter sql_lon = new SqlParameter("longitude",longitude);
        SqlParameter sql_lat = new SqlParameter("latitude",latitude);
        string queryTag = "SELECT id FROM users WHERE username=@Username AND password=@Password";
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
        string userlog = "SELECT id from usersLog where username=@Username AND password=@Passowrd";
        SqlCommand cmd3 = new SqlCommand(userlog, SqlCon);
        cmd3.Parameters.Add(sql_username);
        cmd3.Parameters.Add(sql_password);
        int userid =-1;
        try
        {


        SqlCon.Open();
          userid  = (int)cmd3.ExecuteScalar();
        }
        catch (Exception)
        {
            ans = false;
        }
        finally { SqlCon.Close(); }
        string qUser = "INSERT INTO userLog (username,longitude,latitude) VALUE  (@username,@longitude,@latitude)";
        if (userid >= 0)
        {
          qUser = "UPDATE userLog SET longitude=" + longitude + " latitude=" + latitude + " WHERE id=" + userid;
        }
        SqlCommand cmd = new SqlCommand(qUser, SqlCon);
        cmd.Parameters.Add(sql_username);
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

    public static List<string> Location(double longitude, double latitude)
    {
        List<String> columnData = new List<String>();

        using (SqlConnection connection = new SqlConnection("conn_string"))
        {
            string query = "SELECT * FROM loginUsers";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double userLongitude = (double) reader["longitude"];
                        double userLatitude = (double) reader["latitude"];
                        if (Euclidean(userLatitude, userLongitude, latitude, longitude) < 15)
                        {
                            columnData.Add(reader["email"].ToString());
                        }
                    }
                }
            }
        }
        return columnData;
    }



    /// <summary>
    /// Return the distance between 2 points
    /// </summary>
    private static double Euclidean(double x1,double y1,double x2,double y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1-y2, 2));
    }

}
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;


public class BaseDataAccess
{
    protected string ConnectionString { get; set; }

    public BaseDataAccess()
    {
        string connStr = System.IO.File.ReadAllText("C:\\Users\\yaaco\\Documents\\Projects\\RoundForestWebApi\\Connection.config");
        this.ConnectionString = connStr; //"Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='C:\\Users\\yaaco\\Documents\\Projects\\RoundForestWebApi\\Data\\DatabaseRoundForest.MDF';Integrated Security=True;MultipleActiveResultSets=True;";
//         this.ConnectionString = Microsoft
//    .Extensions
//    .Configuration
//    .ConfigurationExtensions
//    .GetConnectionString(this.Configuration, "DefaultConnection");
    }

    public bool ProductExists(string sellerProductId,out int productId)
    {
        bool exists = false;
        SqlDataReader rdr = null;
        productId = 0;
        using (var sqlConnection1 = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand()
            {
                CommandText = "spProductExists",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection1
            };
        
            cmd.Parameters.Add("@sellerInternalId", SqlDbType.VarChar).Value = sellerProductId;
            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                productId = (int)rdr["Id"];
            }
            exists = productId != 0;
        }
        return exists;
    }

    public int AddProduct(Product newProduct)
    {
       int productId;
        using (var sqlConnection1 = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand()
            {
                CommandText = "spInsertProduct",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection1
            };
        
            cmd.Parameters.Add("@productName", SqlDbType.VarChar).Value = newProduct.ProductName;
            cmd.Parameters.Add("@sellerInternalId", SqlDbType.VarChar).Value = newProduct.SellerInternalId;
            SqlParameter idOut = cmd.Parameters.Add("@new_identity", SqlDbType.Int);
            idOut.Direction = ParameterDirection.ReturnValue;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            //productId = (int)cmd.Parameters["@new_identity"].Value;
            productId = GetProductIdBySellerInternalId(newProduct.SellerInternalId);
        
        }
        return productId;
    }

    public void AddReview(int productid,Review newReview)
    {
       
        using (var sqlConnection1 = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand()
            {
                CommandText = "spInsertReview",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection1
            };
            
            cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = productid;
            cmd.Parameters.Add("@Reviewer", SqlDbType.VarChar).Value = newReview.userNickname;
            cmd.Parameters.Add("@ReviewTitle", SqlDbType.VarChar).Value = newReview.ReviewTitle==null?"":newReview.ReviewTitle;
            cmd.Parameters.Add("@Stars", SqlDbType.Int).Value = newReview.Rating;
            cmd.Parameters.Add("@ReviewText", SqlDbType.VarChar).Value = newReview.ReviewText;
            cmd.Parameters.Add("@ReviewDate", SqlDbType.DateTime).Value = newReview.ReviewSubmissionTime;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteReviews(int productid)
    {
        using (var sqlConnection1 = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand()
            {
                CommandText = "spDeleteReviews",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection1
            };
            
            cmd.Parameters.Add("@productId", SqlDbType.Int).Value = productid;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public int GetProductIdBySellerInternalId(string sellerInternalId)
    {
        SqlDataReader rdr = null;
       int productId=0;
        using (var sqlConnection1 = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand()
            {
                CommandText = "spGetProductIdBySellerId",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection1
            };
            
            cmd.Parameters.Add("@SellerInternalId", SqlDbType.VarChar).Value = sellerInternalId;
            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                productId = (int)rdr["Id"];
            }
        }
        return productId;
    }
}   
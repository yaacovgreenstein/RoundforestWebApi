using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

public interface IMineProduct
{
    Task<string> MineProduct(string url);
}
/// <summary>
/// Summary description for MiningTools
/// </summary>
public class MineWallMart : IMineProduct
{
    public async Task<string> MineProduct(string url)
    {
        string pageContent = await HttpTools.DownloadPage(url);
        //get wallmart product details
        Product product = ExtractProduct(pageContent);
        BaseDataAccess bda = new BaseDataAccess();
        int productId=0;
        if(!bda.ProductExists(product.SellerInternalId,out productId))
        {
            productId = (int)bda.AddProduct(product);
            if(productId>0)
            {
                foreach(Review r in product.Reviews)
                {
                    bda.AddReview(productId,r);
                }
            }
        }
        else
        {
            bda.DeleteReviews(productId); //refill reviews
            foreach(Review r in product.Reviews)
            {
                bda.AddReview(productId,r);
            }
        }
        return product.SellerInternalId;
    }
    private Product ExtractProduct(string pageContent)
    {
        int productNumberIndex = pageContent.IndexOf("\"walmartItemNumber\"") + 22;
        int productNumberEndIndex = pageContent.IndexOf("\"brand\"", productNumberIndex) - 3;
        string productNumber = pageContent.Substring(productNumberIndex, productNumberEndIndex - productNumberIndex);
        int selectedProductIdIndex = pageContent.IndexOf("\"selectedProductId\"");
        int productTitleIndex = pageContent.IndexOf("\"title\"", selectedProductIdIndex) + 9;
        int productTitleEndIndex = pageContent.IndexOf("\"brand\"", selectedProductIdIndex) - 2;
        string productTitle = pageContent.Substring(productTitleIndex, productTitleEndIndex - productTitleIndex);
        List<Review> reviews = ExtractReviews(pageContent);
        Product chosenProduct =
        new Product
        {
            SellerInternalId = productNumber,
            ProductName = productTitle,
            Reviews = reviews
        };
        return chosenProduct;
    }
    private List<Review> ExtractReviews(string pageContent)
    {
        int customerReviewsIndex = pageContent.IndexOf("\"customerReviews\"") + 18;
        int customerReviewsEndIndex = pageContent.IndexOf("\"selected\"", customerReviewsIndex) - 3;
        string reviewListBlock = pageContent.Substring(customerReviewsIndex, customerReviewsEndIndex - customerReviewsIndex);
        var reviewList = JsonConvert.DeserializeObject<List<Review>>(reviewListBlock);
        return reviewList;

    }
}





using System;
public class Review
{
    public string userNickname { get; set; }
    public string ReviewTitle { get; set; }
    public Nullable<int> Rating { get; set; }
    public string ReviewText { get; set; }
    public Nullable<System.DateTime> ReviewSubmissionTime { get; set; }
}
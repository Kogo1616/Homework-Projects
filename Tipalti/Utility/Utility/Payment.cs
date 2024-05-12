namespace Utility;

public class Payment
{
    public AmountInfo Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Message { get; set; }
    public string RefCode { get; set; }
}
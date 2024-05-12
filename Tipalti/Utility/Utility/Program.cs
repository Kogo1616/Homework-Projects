using Utility;

Payment payment = new Payment
{
    Amount = new AmountInfo { Amount = 100, Currency = "USD!" },
    PaymentDate = new DateTime(2022, 1, 1),
    Message = "Payment@ from Tip@alti",
    RefCode = "Abc123$"
};

var utility = new PropertyManipulator();

var getProperties = utility.GetProperties(payment);
var filteredStrings = utility.GetRightStrings(getProperties);

utility.Print(filteredStrings);
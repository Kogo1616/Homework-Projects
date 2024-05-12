namespace Utility.Tests
{
    [TestFixture]
    public class PropertyManipulatorTests
    {
        private PropertyManipulator propertyManipulator;

        [SetUp]
        public void SetUp()
        {
            propertyManipulator = new PropertyManipulator();
        }

        [Test]
        public void GetProperties_ValidObject_ReturnsAllStringProperties()
        {
            // Arrange
            var obj = new Payment
            {
                Amount = new AmountInfo { Amount = 100, Currency = "USD!" },
                PaymentDate = new DateTime(2022, 1, 1),
                Message = "Payment@ from Tip@alti",
                RefCode = "Abc123$"
            };

            // Act
            var properties = propertyManipulator.GetProperties(obj);

            // Assert
            Assert.That(properties, Is.Not.Null);
            Assert.That(properties, Has.Exactly(3).Items);
            Assert.That(properties, Contains.Item(("Message", "Payment@ from Tip@alti")));
            Assert.That(properties, Contains.Item(("RefCode", "Abc123$")));
            Assert.That(properties, Contains.Item(("Currency", "USD!")));
        }

        [Test]
        public void GetProperties_NullObject_ReturnsEmptyList()
        {
            // Arrange
            object obj = null;

            // Act
            var properties = propertyManipulator.GetProperties(obj);

            // Assert
            Assert.That(properties, Is.Not.Null);
            Assert.That(properties, Is.Empty);
        }

        [Test]
        public void GetRightStrings_NullStrings_ReturnsEmptyList()
        {
            // Arrange
            var utility = new PropertyManipulator();

            // Act
            var result = utility.GetRightStrings(null);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
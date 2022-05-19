namespace Expressions.Test;

[TestClass]
public class PropertyAccessorTest
{
    public class TestObject
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
        public string Value { get; set; } = Guid.NewGuid().ToString();
    }

    [TestMethod]
    public void GetValueReturnsValueOfName()
    {
        var accessor = PropertyAccessor.From<TestObject, string>(x => x.Name);
        var obj = new TestObject();

        var name = accessor.GetValue(obj);

        Assert.IsNotNull(name);
        Assert.AreEqual(obj.Name, name);
    }

    [TestMethod]
    public void SetValueUpdatesValueOnObjectInstance()
    {
        var accessor = PropertyAccessor.From<TestObject, string>(x => x.Name);
        var obj = new TestObject();

        var name = Guid.NewGuid().ToString();

        Assert.AreNotEqual(name, obj.Name);

        accessor.SetValue(obj, name);

        Assert.AreEqual(name, obj.Name);
    }
}
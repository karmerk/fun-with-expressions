namespace Expressions.Test;

[TestClass]
public class PropertyAccessorTest
{
    public class TestClass
    {
        public string Name { get; set; } = Guid.NewGuid().ToString();
    }

    [TestMethod]
    public void GetValueReturnsValueOfName()
    {
        var accessor = PropertyAccessor.From<TestClass, string>(x => x.Name);
        var obj = new TestClass();

        var name = accessor.GetValue(obj);

        Assert.IsNotNull(name);
        Assert.AreEqual(obj.Name, name);
    }

    [TestMethod]
    public void SetValueUpdatesValueOnObjectInstance()
    {
        var accessor = PropertyAccessor.From<TestClass, string>(x => x.Name);
        var obj = new TestClass();

        var name = Guid.NewGuid().ToString();

        Assert.AreNotEqual(name, obj.Name);

        accessor.SetValue(obj, name);

        Assert.AreEqual(name, obj.Name);
    }

    public class TestClassWithoutSetter
    {
        public string Name { get; } = Guid.NewGuid().ToString();
    }

    [TestMethod]
    public void CanSetValueIsFalseWhenNoSetterInClass()
    {
        var accessor = PropertyAccessor.From<TestClassWithoutSetter, string>(x => x.Name);
        var obj = new TestClassWithoutSetter();

        Assert.IsTrue(accessor.CanGetValue);
        Assert.IsFalse(accessor.CanSetValue);
    }

    public record TestRecordWithoutSetter(string Name);

    [TestMethod]
    public void RecordClassCanBeModified()
    {
        // Ouch, i was expected records not to have setters, but they do.. and can be modified using reflection or the trick i am using

        var accessor = PropertyAccessor.From<TestRecordWithoutSetter, string>(x => x.Name);
        var obj = new TestRecordWithoutSetter(Guid.NewGuid().ToString());

        Assert.IsTrue(accessor.CanGetValue);
        Assert.IsTrue(accessor.CanSetValue);

        var name = Guid.NewGuid().ToString();
        accessor.SetValue(obj, name);

        Assert.AreEqual(name, obj.Name);
    }
}
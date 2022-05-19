# fun-with-expressions
Lets have fun with expressions

Okay - while having fun with REST i got an urgent need to have fun with expressions instead..


I want to explore the posibility to make some advance code from expressions. So that i can update properties that have been previously selected with a property selector. 

The use case is [here](https://github.com/karmerk/fun-with-rest/blob/38dcbbee60b89448d4145dcfd547aca866339a8e/src/ItemsApi/Program.cs#L94), from my having [fun-with-rest](https://github.com/karmerk/fun-with-rest) repo. But i have meet the scenario before.
 
```cs
public void SomeMethodThatNeedsToGetAndSetThePropertySelectedBySelector(Expression<Func<T,TProperty>> selector)
{
  // create a property set method from the selector
  // create a property get method from the selector
}
```

# Coding Convention

## Why do we need such strict coding convention

To put it simple. It's clean to look at. But in details, here are the reasons I can think of:

- Cleaner code means more scalability, maintainability.
- Easier code to understand. Your teammates are happier to read your code, and are more willing to fix your code if needed.
- Only one standard of coding, no argument over coding styles will ever happen.

## General rules
1. Generally, do not use `MonoBehavior` unless you need to. When do you need to use it? Normally, it's when you use `Start()`, `Awake()`, `Update()`, `OnDestroy()` methods.
2. No magic number in methods. Declare it as `const` field of the class instead
Magic number example (don't do this):
3. A class should try to only serve its purpose (High cohesion). And it should not try to call other classes' functions too much (Less coupling).

```C#
if (player.health <= 0.25f) // 0.25f is a magic number
{
    ActivateEmergencyItem();
}
```

Do this instead:
```C#
private const float emergencyThreshold = 0.25f;

...

if (player.health <= emergencyThreshold)
{
    ActivateEmergencyItem();
}
```

## Variable naming convention
### Cases

- All `public` variables and methods should be named in **PascalCase**.
- All `private` and `protected` variables and methods should be named in **camelCase**.
  
### Naming rules

1. Avoid using abbreviation at all cost. No `e` variable for `element`, or `rb` for `rigidbody` (I know I named that once, I'll fix it).
2. For any float variable, declare the value with full float number e.g. `3.0f`, not just `3`
3. For any bool variable, pick an associated verb to start with, e.g. `isDragonForm`, `hasItem`, `canTransform`.

## Brackets organization

All curly brackets `{}` should be **in the new line**. For example:

```C#
if (condition)
{
    ...
}
else
{
    ...
}

foreach(element e in array)
{
    ...
}
```
**Do not** write it like Go coding syntax or your usual JavaScript coding style:
```C#
// Don't do any of these
if (condition) {
    ...
}
else {
    ...
}

foreach(element e in array) {
    ...
}
```

After you close curly brackets, leave **one new line** after it. For a cleaner looking code (except `if/else` and `try/catch` of course)

### Example:
```C#
foreach(element e in array1)
{
    ...
}

foreach(element e in array2)
{
    ...
}
```


## Class fields and methods organization

Classes fields and method should be written in this order:

1. `public` variables and properties
2. `[SerializedField]` `private` and `protected` variables (Should write `[SerializeField]` and `private ...` in separated lines)
3. `private` and `protected` variables
4. `public` methods
5. `private` and `protected` methods

For each cluster of variable type (like public, protected, serialized private, private). There should be **a new line** to separate them as well.

Also, ALWAYS add private access modifier in front of every private methods. Even default generated `MonoBehaviour` methods like `Start()` or `Update()`

### Example
```C#
public class SomeClass
{
    public int SomeInt1 => someInt1
    public bool IsSomeStatus { get; private set; }
    // Don't forget new line here
    protected float someFloat = 0.24f

    [SerializeField]
    private int adjustableInt = 20;
    [SerializeField]
    private float adjustableFloat = 1.0f;

    private int someInt1;
    private int someInt2;

    public void SomePublicMethod()
    {

    }

    protected int SomeProtectedMethod()
    {
        return 0;
    }

    // ALWAYS add private access modifier in front of every private methods
    private void Start()
    {

    }

    private string SomePrivateMethod()
    {
        return "";
    }
}
```

## Properties

[C# Properties](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties) are one of the most useful features in C#, but it can be confusing at first. If you wonder when should you use property, I have a simple opinion: When you want to write **getters** or **setters**.

Traditional Java getters and setters (Don't do this):
```C#
// Don't do this
private int age = 10;

public int GetAge()
{
    return this.age;
}

private void SetAge(int age)
{
    this.age = age;
}
```

Using C# Property (Do this, it's easier and shorter)
```C#
public int Age { get; private set; }
```
```C#
// We access the Age variable by simply call the variable "Age"
// Get age
currentAge = Age;

// Set age
Age = ageWeWantToAssign;
```

OR 
```C#
public int Age => age;

private int age = 10;
```
```C#
// Same way to get, different way to set
// Get age
currentAge = Age;

// Set age
this.age = ageWeWantToAssign;
```

Property setter with condition:
```C#
public int Age 
{
    get;
    private set
    {
        if (value < 0)
        {
            value = 0;
        }

        Age = value;
    }
}
```
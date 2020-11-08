# Foxy.Web.Styling
A Style and Css builder for Blazor and ASP.NET Core web applications inspired by the [classnames](https://github.com/JedWatson/classnames) javascript library.

# Getting started
The main classes of the package injected through [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) so you first have to register it:

```csharp
    public void ConfigureServices(IServiceCollection services) {
        services.AddCssBuilder();
        services.AddStyleBuilder();
```

In case of the CssBuilder you can add options also:
```csharp
    public void ConfigureServices(IServiceCollection services) {
        services.AddCssBuilder(options => {
            options.PropertyToClassNameConverter = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;
            options.EnumToClassNameConverter = CssBuilderNamingConventions.KebabCaseWithUnderscoreToHyphen;
            options.ExcludeDuplication = false;
        });
```

Then you need to inject into the .cshtml or .razor page.
```csharp
    @inject ICssBuilder Css
    @inject IStyleBuilder Styles
```

And use it:
```
    <div class="@Css["foo", new { Bar = false, Baz = true}]"
         style="@Styles[("width", "100px"), new { Height = "200px" }]">...</div>
```
Output:
```html
    <div class="foo baz" style="width:100px;height:200px">...</div>
```

## Classname generation
When you use the CssBuilder or StyleBuilder with objects or enum values then the property name and enum value is converted to class name. It is determined at startup in configuration. The default is kebab-case with converting underscore to hyphen for bot properties and enums but it is configurable invidually. Examples:
 - BarBaz -> bar-baz
 - Bar_baz -> bar-baz
 - Bar_Baz -> bar--baz

# CssBuilder examples
The CssBuilder mainly works through its indexer and it accepts several type of parameters.

## Simple strings
```csharp
    Css["foo", "bar", "baz"] // -> "foo bar baz"
```

## ValueTuples
The first parameter is emitted if the second is true.
```csharp
    Css[("foo", true), ("bar", false), ("baz", true)] // -> "foo baz"
```

## String enumerables
Can be used with prepared list or with iterator methods.
```csharp
    var cssList = new List<string>{ "foo", "bar", "baz" };
    Css[cssList] // -> "foo bar baz"
```

## From aonther ClassList
```csharp
    var other = Css["bar", "baz"];
    Css["foo", other] // -> "foo bar baz"
```
## From enums
Can be used with enum values. The name generation is determined by ```options.EnumToClassNameConverter```. Default is kebab-case with converting underscore to hyphen.
 ```csharp
    public enum Values {
        Foo,
        Bar, 
        Baz
    }

    Css[Values.Foo, Values.Bar, Values.Baz] // -> "foo bar baz"
```

## From objects
Can be used with generic object (Mainly with anonymous) where all the properties needs to be bool property. The name generation is determined by ```options.PropertyToClassNameConverter```. Default is kebab-case with converting underscore to hyphen.
 ```csharp
    Css[new { Foo = true, Bar = false, Baz = true }] // -> "foo baz"
```

## From ```Dictionary<string, object>```
In Blazor you can capture unmatched values in a ```Dictionary<string, object>```. the CssBuilder can extract the value of the "class" key and add to the class list.

Parent .razor file:
```html
    <RazorComponent class="bar baz">...</RazorComponent>
```

RazorComponent.razor
```
    <div class="@Css["foo", Attributes]">...</div>
    @code {        
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }
    }
```

Output:
```html
    <div class="foo bar baz">...</div>
```

## Dynamic build with Create
You can also start with an empty CssClassList and build it with different way with the Create method. This method also can have an options also in which case uses that to determine how the output generated. If you use multiple setups then strongly recommended to make a singleton instance from these options since it is connected to the caching mechanism. [See later](#Caching-in-CssBuilder-and-StyleBuilder)

# StyleBuilder examples
...

# Caching in CssBuilder and StyleBuilder
The name generation and value processing on objects are process heavy operations so I made a caching mechanism for enum to class and object Property to class conversion. This cache is connected to the options since if different options is used it is excepted to work correctly also.

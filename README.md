# Foxy.Web.Styling
A Style and Css builder for Blazor and ASP.NET Core web applications inspired by the [classnames](https://github.com/JedWatson/classnames) javascript library.

# Css builder
## Getting started
The main classes of the package injected through [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection) so you first have to register it:

```csharp
    public void ConfigureServices(IServiceCollection services) {
        services.AddCssBuilder();
```

Or with options:
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
```

And use it:
```
    <div class="@Css["foo", new { Bar = false, Baz = true}]">...</div>
```
Output:
```html
    <div class="foo baz">...</div>
```

## Examples
The CssBuilder works through its indexer mainly and it accepts several type of parameters.

### Simple strings
```csharp
    Css["foo", "bar", "baz"] // -> "foo bar baz"
```

### ValueTuples
The first parameter is emitted if the second is true.
```csharp
    Css[("foo", true), ("bar", false), ("baz", true)] // -> "foo baz"
```

### String enumerables
Can be used with prepared list or with iterator methods.
```csharp
    var cssList = new List<string>{ "foo", "bar", "baz" };
    Css[cssList] // -> "foo bar baz"
```

### From aonther ClassList
```csharp
    var other = Css["bar", "baz"];
    Css["foo", other] // -> "foo bar baz"
```

### From ```Dictionary<string, object>```
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

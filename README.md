فیلتر و مرتب سازی کندو گرید در سمت سرور همراه با فیلتر تاریخ شمسی

Full example in source code(ASP.NET Core 2.1)

```csharp
[HttpPost]
public async Task<JsonResult> GetProducts([FromBody]Request request)
{
    var context = GetContext();

    var result = await context.Products.ToDataSourceAsync(request);

    return Json(result);
}
```

# فیلتر و مرتب سازی کندو گرید در سمت سرور همراه با فیلتر تاریخ شمسی
نحوه استفاده:

```csharp
[HttpPost]
public async Task<JsonResult> GetProducts([FromBody]Request request)
{
    var context = GetContext();

    var result = await context.Products.ToDataSourceAsync(request);

    return Json(result);
}
```
